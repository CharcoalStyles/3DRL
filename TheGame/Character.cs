using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TheGame
{
    public enum characterClasses
    {
        fighter,
        archer,
        wizard
    }
    public enum characterRaces
    {
        human,
        nonHuman
    }
    public enum birthMonth
    {
        horsey,
        piggy,
        ducky
    }

    public enum damageType
    {
        blunt,
        slash,
        stab,
        magic
    }
    class diceRoll
    {
        public int diceSides;
        public int diceRolls;
        public int mod;

        public diceRoll(int ds, int dr, int m)
        {
            diceSides = ds;
            diceRolls = dr;
            mod = m;
        }

        public int roll()
        {
            int ret = 0;
            for (int i = 0; i < diceRolls; i++)
            {
                int roll = Program.Instance.random.Next(diceSides) + 1;
                ret += roll;
                Console.Write("Roll(" + (i + 1) + "/" + diceRolls + "): " + roll + " ");
            }

            Console.Write("Total(nomod): " + ret + Environment.NewLine);
            ret += mod;

            return ret;
        }

        public string info()
        {
            string ret = diceRolls.ToString() + "d" + diceSides + " +" + mod;
            return ret;
        }
    }
    class Character
    {
        //stuff
        public int level = 0;
        public int HP = 0;
        public int maxHP = 0;
        public int MP = 0;
        public int maxMP = 0;

        //Base stats
        //1 upped every five(?) levels
        public int STR = 10;///melee, armour
        public int AGL = 10;///ranged, dodging
        public int CON = 10;///Health, resistance
        public int INT = 10;///Spells and related
        public int LUC = 10;///Luck

        //Primary Stats
        //upped at levels
        ///<summary>Primary Stat</summary>
        public int melee = 0; ///<summary>Primary Stat</summary>
        public int armour = 0;///<summary>Primary Stat</summary>
        public int ranged = 0;///<summary>Primary Stat</summary>
        public int dodge = 0;///<summary>Primary Stat</summary>
        public int healing = 0;///<summary>Primary Stat</summary>
        public int resistance = 0;///<summary>Primary Stat</summary>
        public int spells = 0;///<summary>Primary Stat</summary>
        public int mana = 0;

        //dice rolls
        //often use dice rolls
        public diceRoll drMeleeToHit = new diceRoll(0, 0, 0);
        public diceRoll drMeleeDamage = new diceRoll(0, 0, 0);
        public diceRoll drRangeToHit = new diceRoll(0, 0, 0);
        public diceRoll drRangeDamage = new diceRoll(0, 0, 0);
        public diceRoll drArmourDefelect = new diceRoll(0, 0, 0);
        public diceRoll drArmourProtect = new diceRoll(0, 0, 0);
        //utility dice rolls
        public diceRoll drMaxHP = new diceRoll(0, 0, 0);
        public diceRoll drMaxMP = new diceRoll(0, 0, 0);


        //characterstuff
        public characterClasses cClass;
        public characterRaces cRace;
        public birthMonth cMonth;

        //helperstuff

        public bool destroyme;
        public bool isPlayer;

        public Character()
        {
            destroyme = false;
        }

        public void setupCharacter()
        {
            level++;
            //setup the basic stats
            switch (cMonth)
            {
                case birthMonth.ducky:
                    break;
                case birthMonth.horsey:
                    STR++;
                    AGL++;
                    break;
                case birthMonth.piggy:
                    break;
            }
            switch (cRace)
            {
                case characterRaces.human:
                    healing += 5;
                    LUC += 5;
                    break;
                case characterRaces.nonHuman:
                    STR -= 2;
                    AGL -= 2;
                    CON -= 2;
                    LUC += 5;
                    break;
            }
            switch (cClass)
            {
                case characterClasses.archer:
                    AGL += 5;
                    STR -= 2;
                    INT -= 1;
                    CON -= 2;
                    ranged = 5;
                    dodge = 5;
                    break;
                case characterClasses.fighter:
                    STR += 4;
                    CON += 2;
                    AGL -= 2;
                    INT -= 3;
                    LUC -= 1;
                    melee = 5;
                    armour = 5;
                    break;
                case characterClasses.wizard:
                    INT += 5;
                    AGL += 1;
                    STR -= 3;
                    CON -= 3;
                    spells = 5;
                    mana = 5;
                    break;
            }

            //setup stats
            melee += STR + Program.Instance.random.Next(LUC/2);
            armour += (CON * 2 + STR / 2) / 2 + Program.Instance.random.Next(LUC / 2);
            ranged += AGL + Program.Instance.random.Next(LUC / 2);
            dodge += AGL + Program.Instance.random.Next(LUC / 2);
            healing += CON + Program.Instance.random.Next(LUC / 2);
            resistance += (INT + CON) / 2 + Program.Instance.random.Next(LUC / 2); ;
            spells += INT + Program.Instance.random.Next(LUC / 2);
            mana += INT + Program.Instance.random.Next(LUC / 2);

            setupDiceRolls();

            maxHP = drMaxHP.roll();
            HP = maxHP;
            maxMP = drMaxMP.roll();
            MP = maxMP;
        }

        public void setupDiceRolls()
        {
            if (true) //melee unarmed
            {
                //setup unarmed dicerolls
                drMeleeToHit.diceRolls = melee/10;
                drMeleeToHit.diceSides = (STR + melee + level) / 15 + (LUC / 10);
                drMeleeToHit.mod = STR - 12 + level;

                drMeleeDamage.diceRolls = (int)(level * 0.333f) + 1;
                drMeleeDamage.diceSides = (STR + melee) / 15 + (LUC / 10);
                drMeleeDamage.mod = STR - 12 + level;
            }
            else
            {
                //armed
            }

            if (true) //ranged 
            {
                //armed
            }

            if (true) //has no armour equipped
            {
                //setup NoArmour dicerolls
                drArmourDefelect.diceRolls = armour / 10;
                drArmourDefelect.diceSides = (CON + armour + level) / 15;
                drArmourDefelect.mod = CON - 10 + level + (LUC / 8);

                drArmourProtect.diceRolls = (int)(level * 0.333f) + 1;
                drArmourProtect.diceSides = (CON + armour) / 15;
                drArmourProtect.mod = CON - 10 + level + (LUC / 8);
            }
            else
            {
                //has armour
            }

            //setupHP
            drMaxHP.diceRolls = level;
            drMaxHP.diceSides = healing + (CON/10);
            drMaxHP.mod = healing - 10 + level;

            //setupMP
            drMaxMP.diceRolls = level + (INT/10);
            drMaxMP.diceSides = mana + INT;
            drMaxMP.mod = mana - 10 + level;
        }

        public int damageCharacter(int i, damageType d)
        {
            Console.WriteLine("Doing Damage");
            int tDamage = i;

            switch (d)
            {
                case damageType.blunt:
                    break;
                case damageType.magic:
                    break;
                case damageType.slash:
                    break;
                case damageType.stab:
                    break;
            }
            
            if (tDamage > HP)
            {
                //character is dead
                Console.WriteLine("Killing: " + this.ToString());
                die();
            }
            else
            {
                //is still alive
                HP -= tDamage;
            }

            return tDamage;
        }

        public void writeCharacter(String s)
        {
            // create a writer and open the file
            TextWriter tw = new StreamWriter(s + ".txt");

            // write a line of text to the file
            tw.WriteLine(s + Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-");
            tw.WriteLine("Race: " + cRace + " Class: " + cClass);
            tw.WriteLine("HP: " + HP + "/" + maxHP + Environment.NewLine + "MP: " + MP + "/" + maxMP);
            tw.WriteLine(Environment.NewLine + "Dice Rolls:" + Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-");
            tw.WriteLine("Melee Roll ToHit: " + drMeleeToHit.info() + Environment.NewLine + "Melee Roll Damage: " + drMeleeDamage.info());
            tw.WriteLine("Armour Roll Defelect: " + drArmourDefelect.info() + Environment.NewLine + "Armour Protect: " + drArmourProtect.info());
            tw.WriteLine("drMaxHP:" + drMaxHP.info());
            tw.WriteLine("drMaxMP:" + drMaxMP.info());
            tw.WriteLine(Environment.NewLine + "Some Sample Attacks:" + Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-");
            for (int i = 0; i < 10; i++)
            {
                tw.WriteLine("To hit: " + drMeleeToHit.roll() + "  |Damage: " + drMeleeDamage.roll());
            } 
            tw.WriteLine(Environment.NewLine + "Some Sample Defends:" + Environment.NewLine + "-=-=-=-=-=-=-=-=-=-=-=-=-");
            for (int i = 0; i < 10; i++)
            {
                tw.WriteLine("Defelect: " + drArmourDefelect.roll() + "  |Protect: " + drArmourProtect.roll());
            }

            // close the stream
            tw.Close();
        }

        public virtual void die() {}
    }
}
