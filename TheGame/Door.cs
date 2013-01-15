using System;
using Mogre;
using System.Collections.Generic;
using System.Text;

namespace TheGame
{
    public class Door : Tile
    {
        public bool isOpen = false;
        public Door(Vector2 v2, tileTypes t) : base(v2,t)
        {

        }

        public void open()
        {
            if (!isOpen)
            {
                Program.Instance.gameManager.addMessage("You try to open the door. You succeed");
                isOpen = true;
                tus.SetTextureName("odoor.png");
            }
        }
        public void close()
        {
            if (isOpen)
            {
                Program.Instance.gameManager.addMessage("You close the door.");
                isOpen = false;
                tus.SetTextureName("cdoor.png");
            }
        }
    }
}
