using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using Mogre;

namespace TheGame
{
    class Control
    {

        //Input definitions
        MOIS.InputManager mInputManager;
        MOIS.Keyboard mKeyBoard;
        MOIS.Mouse mMouse;
        MOIS.JoyStick mJoystick;
        MOIS.ForceFeedback mFF;
        MOIS.ForceFeedback.Const_SupportedEffectList mSupportedEffectList;

        //input modifier switches
        bool shiftDown = false;
        bool ctrlDown = false;
        bool altDown = false;

        public Control(RenderWindow window)
        {
            // Create input manager
            MOIS.ParamList pl = new MOIS.ParamList();
            IntPtr windowHnd;
            window.GetCustomAttribute("WINDOW", out windowHnd);
            pl.Insert("WINDOW", windowHnd.ToString());
            mInputManager = MOIS.InputManager.CreateInputSystem(pl);

            mKeyBoard = (MOIS.Keyboard)mInputManager.CreateInputObject(MOIS.Type.OISKeyboard, true);

            mMouse = (MOIS.Mouse)mInputManager.CreateInputObject(MOIS.Type.OISMouse, true);


            try
            {
                mJoystick = (MOIS.JoyStick)mInputManager.CreateInputObject(MOIS.Type.OISJoyStick, true);
                Program.Instance.gameManager.hasJoy = true;
                try
                {
                    mFF = (MOIS.ForceFeedback)mJoystick.QueryInterface(MOIS.Interface.IType.ForceFeedback);
                    mSupportedEffectList = mFF.GetSupportedEffects();

                }
                catch
                {
                    //Console.WriteLine("No ForceFeedback");
                }
            }
            catch
            {
                //Console.WriteLine("No Joystick");
            }

            //Callbacks for Inputs
            mKeyBoard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyBoard_KeyPressed);
            mKeyBoard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyBoard_KeyReleased);
            mMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            mMouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(mMouse_MousePressed);
            mMouse.MouseReleased += new MOIS.MouseListener.MouseReleasedHandler(mMouse_MouseReleased);

            if (mInputManager.GetNumberOfDevices(MOIS.Type.OISJoyStick) > 0)
            {
                mJoystick.ButtonPressed += new MOIS.JoyStickListener.ButtonPressedHandler(mJoystick_ButtonPressed);
                mJoystick.ButtonReleased += new MOIS.JoyStickListener.ButtonReleasedHandler(mJoystick_ButtonReleased);
                mJoystick.AxisMoved += new MOIS.JoyStickListener.AxisMovedHandler(mJoystick_AxisMoved);
            }
        }

        public void CaptureAll()
        {
            //Capture the Inputs
            mKeyBoard.Capture();
            mMouse.Capture();

            if (mInputManager.GetNumberOfDevices(MOIS.Type.OISJoyStick) > 0)
            {
                mJoystick.Capture();
            }
        }

        //Callbacks
        //Input catchers
        bool mKeyBoard_KeyPressed(MOIS.KeyEvent arg)
        {
            //Console.WriteLine("KB press: " + arg.ToString());
            switch (Program.Instance.gameManager.gameState)
            {
                case gameState.inGame:
                    //Menu

                    Vector2 tPos = new Vector2(0, 0);
                    bool moving = false;
                    switch (arg.key)
                    {
                        case MOIS.KeyCode.KC_UP:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(0, -1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_DOWN:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(0, 1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_LEFT:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(-1, 0);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_RIGHT:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(1, 0);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD8:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(0, -1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD2:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(0, 1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD4:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(-1, 0);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD6:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(1, 0);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD7:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(-1, -1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD9:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(1, -1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD1:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(-1, 1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_NUMPAD3:
                            tPos = Program.Instance.gameManager.player.position + new Vector2(1, 1);
                            moving = true;
                            break;
                        case MOIS.KeyCode.KC_PERIOD:
                            if (Program.Instance.gameManager.currentLevel.returnCell(Program.Instance.gameManager.player.position).type == tileTypes.level_end)
                            {
                                Program.Instance.gameManager.level++;
                                Program.Instance.gameManager.goingDown = true;
                                Program.Instance.gameManager.levelChange = true;

                            }
                            break;
                        case MOIS.KeyCode.KC_COMMA:
                            if (Program.Instance.gameManager.currentLevel.returnCell(Program.Instance.gameManager.player.position).type == tileTypes.level_start)
                            {
                                Program.Instance.gameManager.level--;
                                Program.Instance.gameManager.goingDown = false;
                                Program.Instance.gameManager.levelChange = true;
                            }
                            break;

                        case MOIS.KeyCode.KC_C:
                            for (int i = -1; i < 2; i++)
                            {
                                for (int o = -1; o < 2; o++)
                                {
                                    if (Program.Instance.gameManager.currentLevel.returnCell(Program.Instance.gameManager.player.position + new Vector2(i, o)).type == tileTypes.door)
                                    {
                                        Door tDoor = (Door)Program.Instance.gameManager.currentLevel.returnCell(Program.Instance.gameManager.player.position + new Vector2(i, o));
                                        tDoor.close();
                                    }
                                }
                            }
                            break;
                        default:
                            Program.Instance.gameManager.addMessage("Key Not Handled");
                            break;


                    }
                    //just some hacky feedback, for character movemnt.
                    //Will be rolled in to a new player class soon-ish
                    if (moving)
                    {
                        bool noMonster = true;
                        foreach (Monster m in Program.Instance.gameManager.currentLevel.monsters)
                        {
                            if (tPos == m.position)
                            {
                                noMonster = false;
                                Program.Instance.gameManager.fight(Program.Instance.gameManager.player, m);
                            }
                        }
                        if (noMonster)
                        {
                            switch (Program.Instance.gameManager.currentLevel.returnCell((int)tPos.x, (int)tPos.y).type)
                            {
                                case tileTypes.wall:
                                    Program.Instance.gameManager.addMessage("You ran into a wall");
                                    break;
                                case tileTypes.room:
                                    Program.Instance.gameManager.player.position = tPos;
                                    break;
                                case tileTypes.corridor:
                                    Program.Instance.gameManager.player.position = tPos;
                                    break;
                                case tileTypes.door:
                                    Door tDoor = (Door)Program.Instance.gameManager.currentLevel.returnCell((int)tPos.x, (int)tPos.y);
                                    if (tDoor.isOpen)
                                    {
                                        Program.Instance.gameManager.player.position = tPos;
                                    }
                                    else
                                    {
                                        tDoor.open();
                                    }
                                    break;
                                case tileTypes.level_end:
                                    Program.Instance.gameManager.player.position = tPos;
                                    break;
                                case tileTypes.level_start:
                                    Program.Instance.gameManager.player.position = tPos;
                                    break;
                            }
                        }
                    }

                    Console.WriteLine("Updating PLayer");
                    Program.Instance.gameManager.player.update();
                    Console.WriteLine("Updating Level");
                    Program.Instance.gameManager.currentLevel.update();



                    break;
                case gameState.menu:
                    switch (arg.key)
                    {
                        case MOIS.KeyCode.KC_UP:
                            if (Program.Instance.gameManager.menuOption == 0)
                            {
                                Program.Instance.gameManager.menuOption = Program.Instance.overlayGui.maxOptions - 1;
                            }
                            else
                            {
                                Program.Instance.gameManager.menuOption--;
                            }
                            break;
                        case MOIS.KeyCode.KC_DOWN:
                            if (Program.Instance.gameManager.menuOption == Program.Instance.overlayGui.maxOptions - 1)
                            {
                                Program.Instance.gameManager.menuOption = 0;
                            }
                            else
                            {
                                Program.Instance.gameManager.menuOption++;
                            }
                            break;

                        case MOIS.KeyCode.KC_RETURN:
                            switch (Program.Instance.gameManager.menu)
                            {
                                case menus.main:
                                    switch (Program.Instance.gameManager.menuOption)
                                    {
                                        case 0:
                                            // New Game
                                            Program.Instance.gameManager.menu = menus.ccStart;
                                            Program.Instance.overlayGui.setupMenu();
                                            break;
                                        case 1:
                                            // Exit
                                            break;

                                    }
                                    break;
                                case menus.ccStart:
                                    Program.Instance.gameManager.player.cMonth = birthMonth.horsey;
                                    Program.Instance.gameManager.menu = menus.ccRace;
                                    Program.Instance.overlayGui.setupMenu();
                                    break;

                                case menus.ccRace:
                                    switch (Program.Instance.gameManager.menuOption)
                                    {
                                        case 0:
                                            // human
                                            Program.Instance.gameManager.player.cRace = characterRaces.human;
                                            Program.Instance.gameManager.menu = menus.ccClass;
                                            Program.Instance.overlayGui.setupMenu();
                                            break;
                                        case 1:
                                            // non-human
                                            Program.Instance.gameManager.player.cRace = characterRaces.nonHuman;
                                            Program.Instance.gameManager.menu = menus.ccClass;
                                            Program.Instance.overlayGui.setupMenu();
                                            break;

                                    }
                                    break;
                                case menus.ccClass:
                                    switch (Program.Instance.gameManager.menuOption)
                                    {
                                        case 0:
                                            // fighter
                                            Program.Instance.gameManager.player.cClass = characterClasses.fighter;
                                            Program.Instance.gameManager.endCharacterCreation();
                                            break;
                                        case 1:
                                            // archer
                                            Program.Instance.gameManager.player.cClass = characterClasses.archer;
                                            Program.Instance.gameManager.endCharacterCreation();
                                            break;
                                        case 2:
                                            // wizard
                                            Program.Instance.gameManager.player.cClass = characterClasses.wizard;
                                            Program.Instance.gameManager.endCharacterCreation();
                                            break;

                                    }
                                    break;
                            }
                            break;
                    }
                    break;

            }
            return true;
        }

        bool mKeyBoard_KeyReleased(MOIS.KeyEvent arg)
        {
            //Console.WriteLine("KB release: " + arg.key.ToString());
            return true;
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {
            //Console.WriteLine("mouse: " + arg.ToString());
            if (Program.Instance.gameManager.cammoving)
            {
                //Console.WriteLine("MouseX Rel/5: " + (arg.state.X.rel / 5) + "  - ABS: " + arg.state.X.abs);
                Program.Instance.gameManager.camAddX += (float)arg.state.X.rel / 100;
                Program.Instance.gameManager.camAddY += (float)arg.state.Y.rel / (300 - Program.Instance.gameManager.camAddY);
            }
            return true;
        }


        bool mMouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            //Console.WriteLine("Mouse press: " + id.ToString());
            if (id == MOIS.MouseButtonID.MB_Left)
            {
                Program.Instance.gameManager.cammoving = false ;
                Program.Instance.gameManager.addMessage("Camera Movement Deactivated");
            }
            return true;
        }

        bool mMouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            //Console.WriteLine("Mouse release: " + id.ToString());
            if (id == MOIS.MouseButtonID.MB_Left)
            {
                Program.Instance.gameManager.cammoving = true;
                Program.Instance.gameManager.addMessage("Camera Movement Activated");
            }
            return true;
        }

        bool mJoystick_AxisMoved(MOIS.JoyStickEvent arg, int axis)
        {
            //Console.WriteLine("JoyAxis:" + axis);
            //Console.WriteLine("Joyrange:" + mJoystick.JoyStickState.GetAxis(axis).abs.ToString());

            /*Joystick axis
             *Axis 0 - Left Stick Y Axis
             *      - Positive = Down
             *          -range 0 to 32767
             *      - Negative = Up
             *          -range 0 to -32768
             *Axis 1 - Left Stick X Axis
             *      - Positive = Right
             *          -range 0 to 32767
             *      - Negative = Left
             *          -range 0 to -32768
             *Axis 2 - Right Stick Y Axis
             *      - Positive = Down
             *          -range 0 to 32767
             *      - Negative = Up
             *          -range 0 to -32768
             *Axis 3 - Right Stick X Axis
             *      - Positive = Right
             *          -range 0 to 32767
             *      - Negative = Left
             *          -range 0 to -32768
             * Axis 4 - Trigger
             *      - Positive = Left Trigger
             *          -range 0 to 32640
             *      - Negative = Right Trigger
             *          -range 0 to -32640
            */


            if (mJoystick.JoyStickState.GetAxis(axis).abs > 520 || mJoystick.JoyStickState.GetAxis(axis).abs < -820)
            {
                switch (axis)
                {
                    case 0:
                        //Move Axis 0 - Left Stick Y Axis
                        break;
                    case 1:
                        //Move Axis 1 - Left Stick X Axis
                        break;
                    case 2:
                        //Move Axis 2 - Right Stick Y Axis
                        break;
                    case 3:
                        //Move Axis 3 - Right Stick X Axis
                        break;
                    case 4:
                        //Move Axis 4 - Trigger
                        break;
                }
            }
            else
            {
                switch (axis)
                {
                    case 0:
                        //Zero Axis 0 - Left Stick Y Axis
                        break;
                    case 1:
                        //Zero Axis 1 - Left Stick X Axis
                        break;
                    case 2:
                        //Zero Axis 2 - Right Stick Y Axis
                        break;
                    case 3:
                        //Zero Axis 3 - Right Stick X Axis
                        break;
                    case 4:
                        //Zero Axis 4 - Trigger
                        break;
                }
            }
            return true;
        }

        bool mJoystick_ButtonPressed(MOIS.JoyStickEvent arg, int button)
        {
            //Console.WriteLine("Joy Press:" + button);
            switch (Program.Instance.gameManager.gameState)
            {
                case 0:
                    //Menu
                    if (button == 0)
                    {
                    }
                    break;
            }

            return true;
        }

        bool mJoystick_ButtonReleased(MOIS.JoyStickEvent arg, int button)
        {
            //Console.WriteLine("Joy Release:" + button);
            return true;
        }

    }
}
