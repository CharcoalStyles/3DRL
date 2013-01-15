using System;
using System.Collections.Generic;
using System.Text;
using Mogre;

// WOAH, what the hell are you doing here??!?! ABORT ABORT you'll freak out if you read this...

namespace TheGame
{
    class Launcher
    {
        static void Main(string[] args)
        {
            try
            {
                Program app = Program.Instance;// new Program();
                
                app.Go();
            }
            catch (System.Runtime.InteropServices.SEHException)
            {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    Mogre.Demo.ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
}
