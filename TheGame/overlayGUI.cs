using System;
using Mogre;

namespace TheGame
{
    class OverlayGUI
    {

        Overlay overlay;
        OverlayManager overlayManager;
        OverlayContainer panel;

        public TextAreaOverlayElement messageArea;
        public TextAreaOverlayElement statArea;

        public TextAreaOverlayElement titleArea;
        public TextAreaOverlayElement[] optionAreas = new TextAreaOverlayElement[10];
        public int maxOptions;

        public ColourValue top;
        public ColourValue bot;
        public ColourValue seltop;
        public ColourValue selbot;

        float spinCounter;

        /*FSO state
         *0 = blank screen
         */

        public int fsoState = 0;

        public OverlayGUI()
        {
            //setup the colours
            top = new ColourValue(0.9f, 0.9f, 0.9f);
            bot = new ColourValue(0.8f, 0.8f, 0.8f);
            seltop = new ColourValue(0.9f, 0.7f, 0.7f);
            selbot = new ColourValue(0.8f, 0.6f, 0.6f);

            //Overlay
            overlayManager = OverlayManager.Singleton;
            // Create a panel
            panel = (OverlayContainer)overlayManager.CreateOverlayElement("Panel", "PanelName");
            panel.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            panel.SetPosition(0, 0);
            panel.SetDimensions(Program.Instance.rWindow.Width, Program.Instance.rWindow.Height);

            panel.MaterialName = "fsO/Blank";  // Optional background material

            // Create a text area
            messageArea = (TextAreaOverlayElement)overlayManager.CreateOverlayElement("TextArea", "TextArea");
            messageArea.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            messageArea.SetPosition(0, 0);
            messageArea.SetDimensions(Program.Instance.rWindow.Width, 100);
            messageArea.CharHeight = 24;
            messageArea.FontName = "damn";
            messageArea.ColourTop = top;
            messageArea.ColourBottom = bot;
            messageArea.Caption = "";

            // Status text area
            statArea = (TextAreaOverlayElement)overlayManager.CreateOverlayElement("TextArea", "StatTextArea");
            statArea.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            statArea.SetPosition(0, Program.Instance.rWindow.Height - 50);
            statArea.SetDimensions(Program.Instance.rWindow.Width, 50);
            statArea.CharHeight = 24;
            statArea.FontName = "damn";
            statArea.ColourTop = top;
            statArea.ColourBottom = bot;
            statArea.Caption = "this is a test" + Environment.NewLine + "This is the test's second line";

            //Menus Text Areas
            titleArea = (TextAreaOverlayElement)overlayManager.CreateOverlayElement("TextArea", "TitleTextArea");
            titleArea.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            titleArea.SetPosition(64, 16);
            titleArea.SetDimensions(Program.Instance.rWindow.Width, 32);
            titleArea.CharHeight = 32;
            titleArea.FontName = "damn";
            titleArea.ColourTop = top;
            titleArea.ColourBottom = bot;
            titleArea.Caption = "Title";

            for (int i = 0; i < 10; i++)
            {
                optionAreas[i] = (TextAreaOverlayElement)overlayManager.CreateOverlayElement("TextArea", i+"TextArea");
                optionAreas[i].MetricsMode = GuiMetricsMode.GMM_PIXELS;
                optionAreas[i].SetPosition(32, 64 + (i*26));
                optionAreas[i].SetDimensions(Program.Instance.rWindow.Width, 24);
                optionAreas[i].CharHeight = 24;
                optionAreas[i].FontName = "damn";
                optionAreas[i].ColourTop = top;
                optionAreas[i].ColourBottom = bot;
                optionAreas[i].Caption = "Option " + i.ToString();
            }

            maxOptions = 10;


            // Create an overlay, and add the panel
            overlay = overlayManager.Create("OverlayName");

            overlay.Add2D(panel);
            // Add the text area to the panel
            panel.AddChild(messageArea);
            panel.AddChild(titleArea);
            panel.AddChild(statArea);
            for (int i = 0; i < 10; i++)
            {
                panel.AddChild(optionAreas[i]);
            }

            // Show the overlay
            overlay.Show();
        }


        /*FSO state
         *0 = blank screen
         */
        public void switchFSO(int state)
        {
            fsoState = state;

            switch (fsoState)
            {
                case 0:
                    panel.MaterialName = "fsO/Blank";
                    break;
            }
        }

        public void setupMenu()
        {
            Console.WriteLine("Making the menu");
            switch (Program.Instance.gameManager.menu)
            {
                case menus.main:
                    titleArea.Caption = "Charcoal's 3D Rouge Like";
                    optionAreas[0].Caption = "New Game";
                    optionAreas[1].Caption = "Exit";

                    for (int i = 2; i < 10; i++)
                    {
                        optionAreas[i].Caption = "";
                    }

                    maxOptions = 2;

                    break;

                case menus.ccStart:
                    titleArea.Caption = "Character Creation - Starsign";
                    optionAreas[0].Caption = "Not Yet Implimented - Horse (+1 STR, +1AGL)";

                    for (int i = 1; i < 10; i++)
                    {
                        optionAreas[i].Caption = "";
                    }

                    maxOptions = 1;

                    break;
                case menus.ccRace:
                    titleArea.Caption = "Character Creation - Race";
                    optionAreas[0].Caption = "characterRaces.human";
                    optionAreas[1].Caption = "characterRaces.nonHuman";

                    for (int i = 2; i < 10; i++)
                    {
                        optionAreas[i].Caption = "";
                    }

                    maxOptions = 2;

                    break;
                case menus.ccClass:
                    titleArea.Caption = "Character Creation - Class";
                    optionAreas[0].Caption = "characterClasses.fighter";
                    optionAreas[1].Caption = "characterClasses.archer";
                    optionAreas[2].Caption = "characterClasses.wizard";

                    for (int i = 3; i < 10; i++)
                    {
                        optionAreas[i].Caption = "";
                    }

                    maxOptions = 3;

                    break;
            }
        }

        public void updateMenu()
        {
            spinCounter += 0.04f;

            seltop = new ColourValue((Mogre.Math.Sin(spinCounter) + 1)/2, 0.7f, 0.7f);


            for (int i = 0; i < 10; i++)
            {
                optionAreas[i].ColourTop = top;
                optionAreas[i].ColourBottom = bot;
            }

            optionAreas[Program.Instance.gameManager.menuOption].ColourBottom = selbot;
            optionAreas[Program.Instance.gameManager.menuOption].ColourTop = seltop;
                    
        }
    }
}
