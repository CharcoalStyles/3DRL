using System;
using Mogre;
using System.Collections;
using System.Text;

namespace TheGame
{
    class Level
    {

        public Tile[,] tiles;
        public ArrayList monsters = new ArrayList();


        public Vector2 lStart = new Vector2(0, 0);
        public Vector2 lEnd = new Vector2(1, 1);

        public Vector2 size;

        public Level(int x, int y)
        {
            size = new Vector2(x, y);

            //get LevelGenerator to make the tiles
            tiles = Program.Instance.lg.generate(new Vector2(x, y));

            //get the LevelGenerator to place some monsters
            monsters = Program.Instance.lg.placeMonsters(tiles, new Vector2(x, y));

            //parse features
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int o = 0; o < tiles.GetLength(1); o++)
                {
                    if (tiles[i, o].type == tileTypes.level_start) //level start
                        lStart = new Vector2(i, o);
                    else if (tiles[i, o].type == tileTypes.level_end) //level end
                        lEnd = new Vector2(i, o);
                }
            }

        }

        public Tile returnCell(int x, int y)
        {
            return tiles[x, y];
        }

        public Tile returnCell(Vector2 v2IN)
        {
            return tiles[(int)v2IN.x, (int)v2IN.y];
        }
        public Tile returnCell(int x, int y, int dir)
        {
            Tile ret = new Tile(new Vector2(1000, 1000), tileTypes.wall);
            switch (dir)
            {
                case 0:
                    ret = tiles[x, y - 1];
                    break;
                case 1:
                    ret = tiles[x + 1, y];
                    break;
                case 2:
                    ret = tiles[x, y + 1];
                    break;
                case 3:
                    ret = tiles[x - 1, y];
                    break;
            }

            return ret;
        }

        public void make3D()
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int o = 0; o < size.y; o++)
                {
                    tiles[i, o].make();
                }
            }

            foreach (Monster m in monsters)
            {
                m.make();
            }
        }

        public void destroy()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int o = 0; o < tiles.GetLength(1); o++)
                {
                    tiles[i, o].destroy();
                }
            }
             foreach (Monster m in monsters)
            {
                    m.destroy();
            }
        }

        public void update()
        {

            Console.WriteLine("Updating Level - in function");
            ArrayList destroyedMonsters = new ArrayList();
            
            foreach (Monster m in monsters)
            {
                if (m.destroyme)
                {
                    m.destroy();
                    destroyedMonsters.Add(m);
                }
            }

            foreach (Monster m in destroyedMonsters)
            {
                monsters.Remove(m);
            }

            Console.WriteLine("Monsters in level: " + monsters.Count);
        }

        
    }
}
