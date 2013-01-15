using System;
using Mogre;
using System.Collections.Generic;
using System.Text;


namespace TheGame
{
    public enum tileTypes
    {
        wall,
        corridor,
        room,
        door,
        level_start,
        level_end
    }

    public class Tile
    {

        Entity ent;
        public SceneNode sn;

        public static int unique;

        public TextureUnitState tus;

        public tileTypes type;

        public Vector2 pos;


        public Tile(Vector2 v2, tileTypes t)
        {
            type = t;
            pos = v2;
        }

        public void make()
        {
            if (type != tileTypes.wall)
            {
                unique++;
                //create a scene node, off the root scene node
                sn = Program.Instance.sceneManager.RootSceneNode.CreateChildSceneNode();
                //Load the mesh into the entity
                ent = Program.Instance.sceneManager.CreateEntity("tile" + unique, "Floor.mesh");
                //Attach the Entity to the scene node
                sn.AttachObject(ent);
                sn.Position = new Vector3(pos.x * Program.Instance.gameManager.tileSpacing, 0, pos.y * Program.Instance.gameManager.tileSpacing);


                MaterialPtr m = ent.GetSubEntity(0).GetMaterial();
                m.Clone("Bmat" + unique);
                MaterialPtr ma = MaterialManager.Singleton.GetByName("Bmat" + unique);
                Technique t = ma.GetTechnique(0);
                Pass p = t.GetPass(0);
                tus = p.GetTextureUnitState(0);

                switch (type)
                {
                    case tileTypes.corridor:
                        tus.SetTextureName("dirt_" + Program.Instance.random.Next(0, 5).ToString() + ".png");
                        break;
                    case tileTypes.room:
                        tus.SetTextureName("room_" + Program.Instance.random.Next(0, 10).ToString() + ".png");
                        break;
                    case tileTypes.door:
                        tus.SetTextureName("cdoor.png");
                        break;
                    case tileTypes.level_start:
                        tus.SetTextureName("up.png");
                        break;
                    case tileTypes.level_end:
                        tus.SetTextureName("down.png");
                        break;
                }

                ent.GetSubEntity(0).MaterialName = "Bmat" + unique;
            }
            else
            {
                if (pos.x > 0 && pos.y > 0 && pos.x < Program.Instance.gameManager.currentLevel.size.x - 1 && pos.y < Program.Instance.gameManager.currentLevel.size.y - 1)
                {
                    Console.WriteLine("MAKING WALL TILE: " + pos.ToString());
                    bool makeWall = false;
                    if (Program.Instance.gameManager.currentLevel.tiles[(int)pos.x - 1, (int)pos.y].type != tileTypes.wall)
                    {
                        makeWall = true;
                    }
                    if (Program.Instance.gameManager.currentLevel.tiles[(int)pos.x + 1, (int)pos.y].type != tileTypes.wall && !makeWall)
                    {
                        makeWall = true;
                    }
                    if (Program.Instance.gameManager.currentLevel.tiles[(int)pos.x, (int)pos.y - 1].type != tileTypes.wall && !makeWall)
                    {
                        makeWall = true;
                    }
                    if (Program.Instance.gameManager.currentLevel.tiles[(int)pos.x, (int)pos.y + 1].type != tileTypes.wall && !makeWall)
                    {
                        makeWall = true;
                    }

                    if (makeWall)
                    {
                        unique++;
                        //create a scene node, off the root scene node
                        sn = Program.Instance.sceneManager.RootSceneNode.CreateChildSceneNode();
                        //Load the mesh into the entity
                        ent = Program.Instance.sceneManager.CreateEntity("wall" + unique, "wall.mesh");
                        //Attach the Entity to the scene node
                        sn.AttachObject(ent);
                        sn.Position = new Vector3(pos.x * Program.Instance.gameManager.tileSpacing, 0, pos.y * Program.Instance.gameManager.tileSpacing);

                    }
                }

            }
        }

        public void destroy()
        {
            if(sn != null)
                Program.Instance.sceneManager.RootSceneNode.RemoveAndDestroyChild(sn.Name);
        }
    }
}
