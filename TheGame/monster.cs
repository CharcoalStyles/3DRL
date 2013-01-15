using System;
using Mogre;
using FSLOgreCS;

// An empty class.
// - New classes are created by right clicking 'TheGame' in the 'Solution explorer' tab to the right.
// - Then 'Add' . 'New item'
// - Type your class name in the pop up window at the bottom (make sure 'class' is selected in the top of the window)
// - Click 'Add'
// - Copy the lines starting with 'using' up the top here to your new class (there will be a few there by default, just overwrite them)

namespace TheGame
{
    class Monster : Character
    {

        Entity ent;
        public SceneNode sn;

        public SceneNode light_node;

        public static int unique;

        public Vector2 position;


        public Monster(Vector2 oPos)
        {
            position = oPos;
            isPlayer = false;
        }

        //Public functions

        public void make()
        {
            unique++;
            Console.WriteLine("MAKING A MONSTYER");
            //create a scene node, off the root scene node
            sn = Program.Instance.sceneManager.RootSceneNode.CreateChildSceneNode();
            //Load the mesh into the entity
            ent = Program.Instance.sceneManager.CreateEntity("Monsta" + unique, "Player.mesh");
            //Attach the Entity to the scene node
            sn.AttachObject(ent);
            sn.Position = new Vector3(0, 3, 0);

            update();

        }

        public void update()
        {
            sn.Position = new Vector3(position.x * Program.Instance.gameManager.tileSpacing, 3, position.y * Program.Instance.gameManager.tileSpacing);
        }

        public void destroy()
        {
            if (sn != null)
                Program.Instance.sceneManager.RootSceneNode.RemoveAndDestroyChild(sn.Name);
        }

        public override void die()
        {
            Console.WriteLine("Dieing: " + this.ToString());
            destroyme = true;
            Program.Instance.gameManager.addMessage("You Killed the monster");
        }
    }
}
