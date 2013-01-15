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
    class Player : Character
    {

        Entity ent;
        public SceneNode sn;

        public Light l;
        public SceneNode light_node;

        public static int unique;

        public Vector2 position;

        public Player(Vector3 oPos, Quaternion oOrent)
        {
            unique++;
            //create a scene node, off the root scene node
            sn = Program.Instance.sceneManager.RootSceneNode.CreateChildSceneNode();
            //Load the mesh into the entity
            ent = Program.Instance.sceneManager.CreateEntity("playa" + unique, "PC_01.mesh");
            //Attach the Entity to the scene node
            sn.AttachObject(ent);

            l = Program.Instance.sceneManager.CreateLight("Sun");
            l.DiffuseColour = new ColourValue(0.9f, 0.6f, 0.2f);
            light_node = sn.CreateChildSceneNode();
            light_node.AttachObject(l);
            light_node.Position = new Vector3(0, 0, -55);

            sn.Orientation = new Quaternion(new Degree(90), Vector3.UNIT_X);

            position = new Vector2(0, 0);


            isPlayer = true;
        }

        //Public functions

        public void update()
        {
            sn.Position = new Vector3(position.x * Program.Instance.gameManager.tileSpacing, 0, position.y * Program.Instance.gameManager.tileSpacing);
        }

        public override void die()
        {
            Program.Instance.gameManager.addMessage("You Die...");
        }
    }
}
