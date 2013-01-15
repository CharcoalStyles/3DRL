using System;
using System.Text;
using Mogre;
using FSLOgreCS;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace TheGame
{
    public enum gameState
    {
        programInit,
        loading,
        menu,
        inGame
    }
    public enum menus
    {
        main,
        ccStart,
        ccRace,
        ccClass,
        levelUp
    }
    class GameManager
    {
        public gameState gameState;
        public menus menu;
        public int menuOption = 0;

        public bool hasJoy;

        public ArrayList levels = new ArrayList();

        public Level currentLevel;
        public int level = 0;
        public bool goingDown = true;
        public bool levelChange = false;

        public string[] messageBuffer;

        public Player player;

        public bool cammoving;
        public float camAddX;
        public float camAddY;

        public float tileSpacing = 39.25f;

        public float flameFlicker = 0;

        public GameManager()
        {
            //initalise the game
            gameState = gameState.programInit;
            menu = menus.main;

            hasJoy = false;

            messageBuffer = new string[50];
            for (int i = 0; i < 50; i++)
            {
                messageBuffer[i] = "";
            }

            cammoving = false;
            camAddX = 0;
            camAddY = 0;

            player = new Player(new Vector3(0, 0, 0), Quaternion.IDENTITY);
        }

        public void gameStart()
        {
            //set gameState

            //Program.Instance.vp.Camera = Program.Instance.oCam;
        }

        public void gameOver()
        {

        }

        public void makeLevel()
        {
            Console.WriteLine("Start Make Level");
            if (currentLevel == null)
            {
                currentLevel = new Level(50, 50);
                levels.Add(currentLevel);
                player.position = currentLevel.lStart;

            }
            else
            {
                //Destroys the 3D of the tiles for this level, before generating and or making a new one.
                currentLevel.destroy();
                if (level == levels.Count)
                {
                    currentLevel = new Level(Program.Instance.random.Next(0, 50) + 50, Program.Instance.random.Next(0, 50) + 50);

                    levels.Add(currentLevel);
                    player.position = currentLevel.lStart;
                }
                else
                {
                    currentLevel = (Level)levels[level];
                    //put the 3d monsters
                    foreach (Monster m in currentLevel.monsters)
                    {
                        m.make();
                    }

                    if (goingDown)
                        player.position = currentLevel.lStart;
                    else
                        player.position = currentLevel.lEnd;
                }
            }


            currentLevel.make3D();
            Console.WriteLine("End Make Level");
            addMessage("Random Dungeon Level " + level);
            player.update();

        }


        public void gameUpdate()
        {
            switch (gameState)
            {
                case gameState.programInit:
                    Console.WriteLine("Program Init");
                    Program.Instance.overlayGui.setupMenu();
                    gameState = gameState.menu;
                    break;
                case gameState.loading:
                    //Loading a Level
                    Console.WriteLine("Loading a level");

                    //make a level
                    makeLevel();

                    //set to "play"
                    gameState = gameState.inGame;
                    break;
                case gameState.inGame:
                    //ingame
                    //camera movement, following player.
                    if (!cammoving)
                    {
                        camAddX /= 1.05f;
                        camAddY /= 1.05f;
                    }

                    if (camAddY > 6)
                    {
                        camAddY = 6;
                    }
                    else if (camAddY < -2.2f)
                    {
                        camAddY = -2.2f;
                    }

                    Program.Instance.oCam.Position = player.sn.Position + new Vector3(20, 900, 0) + new Vector3(Mogre.Math.Sin(camAddX) * 500, (camAddY * 400) + 100, Mogre.Math.Cos(camAddX) * 500);
                    
                    Program.Instance.oCam.LookAt(player.sn.Position);
                    //UI update
                    Program.Instance.overlayGui.statArea.Caption = "HP: " + player.HP + "/" + player.maxHP + Environment.NewLine + "MP: " + player.MP + "/" + player.maxMP;
                    if (levelChange)
                    {
                        gameState = gameState.loading;
                        levelChange = false;
                    }

                    //flame flicker
                    flameFlicker += Mogre.Math.RangeRandom(-0.3f,0.3f);
                    player.l.Position = new Vector3(Mogre.Math.Sin(flameFlicker), Mogre.Math.Cos(flameFlicker), 0);

                    break;
                case gameState.menu:
                    //menu
                    Program.Instance.overlayGui.updateMenu();
                    break;
            }
        }

        public void addMessage(string s)
        {
            Console.WriteLine(s);
            for (int i = 0; i < messageBuffer.Length - 1; i++)
            {
                messageBuffer[i] = messageBuffer[i + 1];
            }

            messageBuffer[messageBuffer.Length - 1] = s;

            Program.Instance.overlayGui.messageArea.Caption = messageBuffer[messageBuffer.Length - 3] + Environment.NewLine + messageBuffer[messageBuffer.Length - 2] + Environment.NewLine + messageBuffer[messageBuffer.Length - 1];
        }

        public void endCharacterCreation()
        {
            //make the character
            player.setupCharacter();
            player.writeCharacter("Player");

            //blank out The menu
            Program.Instance.overlayGui.titleArea.Caption = "";
            for (int i = 0; i < 10; i++)
            {
                Program.Instance.overlayGui.optionAreas[i].Caption = "";
            }

            //set the gamestate to ingame
            gameState = gameState.loading;

        }


        public void fight(Character c1, Character c2)
        {
            int c1ToHit = c1.drMeleeToHit.roll();
            int c1Damage = c1.drMeleeDamage.roll();

            int c2ToHit = c2.drMeleeToHit.roll();
            int c2Damage = c2.drMeleeDamage.roll();

            int c1Defelect = c1.drArmourDefelect.roll();
            int c1Protect = c1.drArmourProtect.roll();

            int c2Defelect = c2.drArmourDefelect.roll();
            int c2Protect = c2.drArmourProtect.roll();

            addMessage("c1 - ToHit: " + c1ToHit + " Dam: " + c1Damage + " Def: " + c1Defelect + " Pro: " + c1Protect);
            addMessage("c2 - ToHit: " + c2ToHit + " Dam: " + c2Damage + " Def: " + c2Defelect + " Pro: " + c2Protect);

            string rMessage = "";


            Console.WriteLine("Checking c1 hit");
            if (c1ToHit > c2Defelect)
            {
                int i = c2.damageCharacter(c1Damage - c2Protect, damageType.blunt);
                if (c1.isPlayer)
                {
                    rMessage += "You Hit for " + i + " damage. ";
                }
            }


            Console.WriteLine("Checking c2 hit");
            if (!c2.destroyme)
            {
                if (c2ToHit > c1Defelect)
                {
                    int i = c1.damageCharacter(c2Damage - c1Protect, damageType.blunt);
                    if (c1.isPlayer)
                    {
                        rMessage += "You are Hit for " + i + " damage. ";
                    }
                }
            }


            Console.WriteLine("Printing message to message log");
            addMessage(rMessage);
        }
    }
}
