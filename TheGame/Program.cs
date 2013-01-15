//****************************************************************
// Initialise some stuff
// - copy all the lines starting with 'using' into any new files you create for new classes
// - These tell the system which bits of other peoples code we'll be using
//****************************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using Mogre;
using FSLOgreCS;

// Everything is in a 'namespace'. Don't fret over it now ;)
namespace TheGame
{
    //****************************************************************
    // The default class used for the whole program. Yes! Everything is a class even the program itself!
    // - The second half of the class definition is telling it to use an existing class to help us setup irritating stuff
    //****************************************************************
    class Program : Mogre.Demo.ExampleApplication.Example
    {
        // Variables for use throughout this whole class are defined here
        // *** YOUR CODE GOES UNDER HERE ***

        public ArrayList alStatics = new ArrayList();

        //global Random
        public Random random = new Random();

        //mainGUI
        public OverlayGUI overlayGui;

        //main thread timer
        public Timer timer;

        // Ogre
        public SceneManager sceneManager;

        //Input System
        public Control control;

        //Game MAnager
        public GameManager gameManager;

        //window
        public RenderWindow rWindow;

        //Cam
        public Camera oCam;

        //viewport Pointer
        public Viewport vp;

        //sound thingy
        public FSLSoundManager fslSM;

        //A Level Generator
        public LevelGenerator lg = new LevelGenerator();

        //****************************************************************
        // - 'CreateScene' is a method used as a place to initialise and setup the scene.
        // - IT IS ONLY RUN ONCE at the start
        //****************************************************************
        public override void CreateScene()
        {
            // First we have to initialise some stuff
            sceneManager = sceneMgr;

            // Ask the sceneManager to set some common shadow settings
            sceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;
            sceneMgr.AmbientLight = new ColourValue(0.5f, 0.55f, 0.6f);
            sceneMgr.ShadowColour = new ColourValue(0.23f, 0.27f, 0.3f);
            // sceneMgr.SetFog(FogMode.FOG_EXP2, new ColourValue(0.92f, 0.92f, 0.98f), 0.0015f);
            //sceneMgr.SetFog(FogMode.FOG_LINEAR, new ColourValue(0.92f, 0.92f, 0.98f), 0.002f, 300, 910);
            //give us a pretty skybox
            //sceneMgr.SetSkyBox(true, "Examples/CloudyNoonSkyBox", 500f);

            oCam = sceneManager.CreateCamera("ocam");
            oCam.Position = new Vector3(0, 100, 0.1f);
            oCam.LookAt(0, 0, 0);

            //create sound manager
            fslSM = FSLSoundManager.Instance;
            fslSM.InitializeSound(oCam);

            //Set the viewport to the player's chase camera.
            vp = viewport;
            vp.Camera = oCam;
            vp.ShadowsEnabled = true;

            //make a pointer to window
            rWindow = window;

            //Create GUI
            overlayGui = new OverlayGUI();

            //Create the Game manager
            gameManager = new GameManager();

            //create Input System
            control = new Control(window);

            //Create Timer
            timer = new Timer();

 }

        //****************************************************************
        // - 'Framestarted' is a method used to update or test anything that is constantly changing, eg player movement
        // - IT IS RUN CONSTANTLY. Anywhere from 60 to 100 times a second the code here with be run (everytime your monitor updates)
        //****************************************************************
        bool FrameStarted(FrameEvent evt)
        {
            // *** YOUR CODE GOES UNDER HERE ***
            //Capture the input
            control.CaptureAll();

            gameManager.gameUpdate();
            return true;
        }


        //****************************************************************
        // Standard startup stuff, you don't have to understand this just yet ;)
        //****************************************************************
        public override void CreateFrameListener()
        {

            base.CreateFrameListener();
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);

            //debugOverlay.Hide();
        }

        // SINGLETON STATIC THING
        static Program instance = null;
        public static Program Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Program();
                }
                return instance;
            }
        }
    }
}
