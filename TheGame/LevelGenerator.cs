using System;
using Mogre;
using System.Collections;
using System.Text;
using System.IO;

namespace TheGame
{
    class LevelGenerator
    {
        public LevelGenerator()
        {
        }

        public Tile[,] generate(Vector2 size)
        {
            Tile[,] workingLevel = new Tile[(int)size.x, (int)size.y];

            //Initalise the tiles as blank, first
            for (int i = 0; i < size.x; i++)
            {
                for (int o = 0; o < size.y; o++)
                {
                    workingLevel[i, o] = new Tile(new Vector2(i, o), 0);
                }
            }

            //debug counter, to see how often the RDG fails an needs to start an operation again.
            int dou = 0;

            //Total amount of tiles in this level
            int tiles = 0;

            //next direction the coridoor is going
            int nextSmallJob = 0;

            //last Direction the corodor moved
            int lastEvent = 0;

            //Working tile coords
            int wx = 0;
            int wy = 0;

            //temporary coridoor length
            int tempCoridoorLength = 0;

            //Temporary Bad counter
            int badSpawns = 0;

            //Iteration Counters
            int cIterations = 0;
            int rIterations = 0;

            //max room size
            int maxRoomWidth = (int)size.x / 5;
            int maxRoomHeight = (int)size.y / 5;

            //Number of good rooms
            int goodRooms = 0;

            //Making an Origin - pick a random origin, near-ish to the middle
            wx = Program.Instance.random.Next(5, (int)size.x - 5);
            wy = Program.Instance.random.Next(5, (int)size.y - 5);
            //workingLevel[wx, wy].type = 1;    //don't need to set that now we set the working sqaure for making a room, before making a room
            Console.WriteLine("Origin: " + wx + "|" + wy);

            //make the layout of the coridoors, until the level is at least 1/4 full
            while (tiles < (size.x * size.y) / 4 && cIterations < 10000)
            {
                //making a coorroodoor
                //Check if we want to make the coridor go in the same direction as last time
                if (Program.Instance.random.Next(0, 7) == 1)
                {
                    //no we don't, new direction (That might be the same as the old one anyway)
                    //and nextSmallJob was already assigned to the last one at the end of the last generation
                    nextSmallJob = Program.Instance.random.Next(0, 4);
                }

                Console.WriteLine("Working from:" + wx + "|" + wy);
                Console.WriteLine("Next Move::" + nextSmallJob);

                //check to see if it goes out of the bounds of the level
                //then check to see if ahead is already a coridor
                //then check to see if there is a coridore beside me now and ahead (on same side)
                //finalls, same as above, on other side

                bool badmove = false;
                switch (nextSmallJob)
                {
                    case 0:
                        //up
                        if (wy < 5)
                        {
                            //bad move, Edge of the world
                            Console.WriteLine("Bad move: Top of the world");
                            badmove = true;
                        }
                        //    else if (workingLevel[wx, wy - 1].type != 0)
                        //   {
                        //bad move, already occupied
                        //       badmove = true;
                        //   }
                        else if (workingLevel[wx - 1, wy].type == tileTypes.corridor && workingLevel[wx - 1, wy - 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else if (workingLevel[wx + 1, wy].type == tileTypes.corridor && workingLevel[wx + 1, wy - 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else
                        {
                            //good move, let's move the working cordinate
                            wy--;
                        }
                        break;
                    case 1:
                        //right
                        if (wx > size.x - 5)
                        {
                            //bad move, Edge of the world
                            Console.WriteLine("Bad move: Right of the world");
                            badmove = true;
                        }
                        // else if (workingLevel[wx + 1, wy].type != 0)
                        // {
                        //bad move, already occupied
                        //     badmove = true;
                        // }
                        else if (workingLevel[wx, wy - 1].type == tileTypes.corridor && workingLevel[wx + 1, wy - 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else if (workingLevel[wx, wy + 1].type == tileTypes.corridor && workingLevel[wx + 1, wy + 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else if (workingLevel[wx, wy - 1].type == tileTypes.room && workingLevel[wx + 1, wy - 1].type == tileTypes.room)
                        {
                            //bad move, room next to me now, and after move
                            badmove = true;
                        }
                        wx++;
                        break;
                    case 2:
                        //down
                        if (wy > size.y - 5)
                        {
                            //bad move, Edge of the world
                            Console.WriteLine("Bad move: Bottom of the world");
                            badmove = true;
                        }
                        //else if (workingLevel[wx, wy + 1].type != 0)
                        // {
                        //bad move, already occupied
                        //     badmove = true;
                        // }
                        else if (workingLevel[wx - 1, wy].type == tileTypes.corridor && workingLevel[wx - 1, wy + 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else if (workingLevel[wx + 1, wy].type == tileTypes.corridor && workingLevel[wx + 1, wy + 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else
                        {
                            //good move, let's move the working cordinate
                            wy++;
                        }
                        break;
                    case 3:
                        //left
                        if (wx < 5)
                        {
                            //bad move, Edge of the world
                            Console.WriteLine("Bad move: Left of the world");
                            badmove = true;
                        }
                        //else if (workingLevel[wx - 1, wy].type != 0)
                        //{
                        //bad move, already occupied
                        //    badmove = true;
                        //}
                        else if (workingLevel[wx, wy - 1].type == tileTypes.corridor && workingLevel[wx - 1, wy - 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else if (workingLevel[wx, wy + 1].type == tileTypes.corridor && workingLevel[wx - 1, wy + 1].type == tileTypes.corridor)
                        {
                            //bad move, Coridoor next to me now, and after move
                            badmove = true;
                        }
                        else
                        {
                            //good move, let's move the working cordinate
                            wx--;
                        }
                        break;
                }

                if (badmove)
                {
                    //make next small job (hopefully) not the same, so if it tries to do the same move
                    //which is likely, it won't go through the generation of this direction again (hopefully)
                    nextSmallJob = Program.Instance.random.Next(0, 4);
                    dou++;
                    badSpawns++;
                    if (badSpawns < 30)
                    {
                        badSpawns = 0;
                        bool good = false;
                        while (!good)
                        {
                            Console.WriteLine("STILLL GETTTINGGGGG STUCK IN A BAD CORRIDOR LOOOOOP");
                            wx = Program.Instance.random.Next(1, (int)size.x - 2);
                            wy = Program.Instance.random.Next(1, (int)size.y - 2);
                            if (workingLevel[wx, wy].type == tileTypes.corridor)
                                good = true;
                        }
                    }
                }
                else
                {
                    //it was a "good" move, so let's make it a coridor
                    workingLevel[wx, wy].type = tileTypes.corridor;
                    tiles++;
                    tempCoridoorLength++;
                    lastEvent = nextSmallJob;
                }

                cIterations++;
            }

            while (goodRooms < ((int)size.x + (int)size.y) /10 )
            {
                //make some rooms!
                //random width+height
                int rw = Program.Instance.random.Next(2, maxRoomWidth);
                int rh = Program.Instance.random.Next(2, maxRoomHeight);
                //random position, that will always make a room not going out of the bounds!
                wx = Program.Instance.random.Next(3, (int)size.x - (maxRoomWidth + 3));
                wy = Program.Instance.random.Next(3, (int)size.y - (maxRoomHeight + 3));


                Console.WriteLine("Worker origin: " + wx + "|" + wy);
                Console.WriteLine("Proposed Room Size:" + rw + "|" + rh);

                //room score
                int roomScore = 0;

                //Check to see if this place and size is a good place for a room
                for (int i = wx; i < wx + rw; i++)
                {
                    for (int o = wy; o < wy + rh; o++)
                    {
                        //if an adjacent cell is a room, remove a point

                        if (workingLevel[i + 1, o].type == tileTypes.room)
                        {
                            roomScore--;
                        }
                        if (workingLevel[i - 1, o].type == tileTypes.room)
                        {
                            roomScore--;
                        }
                        if (workingLevel[i, o + 1].type == tileTypes.room)
                        {
                            roomScore--;
                        }
                        if (workingLevel[i, o - 1].type == tileTypes.room)
                        {
                            roomScore--;
                        }

                        //if an adjacent cell is a coridoor, add a point

                        if (workingLevel[i + 1, o].type == tileTypes.corridor)
                        {
                            roomScore++;
                        }
                        if (workingLevel[i - 1, o].type == tileTypes.corridor)
                        {
                            roomScore++;
                        }
                        if (workingLevel[i, o + 1].type == tileTypes.corridor)
                        {
                            roomScore++;
                        }
                        if (workingLevel[i, o - 1].type == tileTypes.corridor)
                        {
                            roomScore++;
                        }
                    }
                }
                Console.WriteLine("RoomScore: " + roomScore);

                if (roomScore > 5)
                {
                    //make room from top left origin
                    for (int i = wx; i < wx + rw; i++)
                    {
                        for (int o = wy; o < wy + rh; o++)
                        {
                            if (workingLevel[i, o].type == tileTypes.corridor || workingLevel[i, o].type == tileTypes.room)
                            {
                                workingLevel[i, o].type = tileTypes.room;
                            }
                            else
                            {
                                workingLevel[i, o].type = tileTypes.room;
                                tiles++;
                            }
                        }
                    }
                    goodRooms++;
                    roomScore = 0;
                }
                rIterations++;
            }

            //debug, number of culled coridors
            int culledCoridoors = 0;


            //let's get rid of corridors that run along room edges!
            //50 iterations
            for (int cull = 0; cull < 10; cull++)
            {
                for (int i = 1; i < size.x - 2; i++)
                {
                    for (int o = 1; o < size.y - 2; o++)
                    {
                        if (workingLevel[i, o].type == tileTypes.room)
                        {
                            if (workingLevel[i - 1, o].type == tileTypes.corridor && workingLevel[i - 1, o - 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i - 1, o].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i - 1, o].type == tileTypes.corridor && workingLevel[i - 1, o + 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i - 1, o].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i + 1, o].type == tileTypes.corridor && workingLevel[i + 1, o - 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i + 1, o].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i + 1, o].type == tileTypes.corridor && workingLevel[i + 1, o + 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i + 1, o].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i, o - 1].type == tileTypes.corridor && workingLevel[i - 1, o - 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i, o - 1].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i, o - 1].type == tileTypes.corridor && workingLevel[i + 1, o - 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i, o - 1].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i, o + 1].type == tileTypes.corridor && workingLevel[i + 1, o + 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i, o + 1].type = tileTypes.room;
                                culledCoridoors++;
                            }
                            else if (workingLevel[i, o + 1].type == tileTypes.corridor && workingLevel[i - 1, o + 1].type == tileTypes.corridor)
                            {
                                //bad move, Coridoor next to me now, and after move
                                workingLevel[i, o + 1].type = tileTypes.room;
                                culledCoridoors++;
                            }
                        }
                    }
                }
            }

            //So, culling the "parralell" corridors makes the rooms have one corridor cell inside that room that 9/10 does nothing
            //here's a little fix to make those cells part of the room
            for (int i = 1; i < size.x - 2; i++)
            {
                for (int o = 1; o < size.y - 2; o++)
                {
                    if (workingLevel[i, o].type == tileTypes.corridor)
                    {
                        if (workingLevel[i + 1, o].type == tileTypes.room || workingLevel[i - 1, o].type == tileTypes.room)
                        {
                            if (workingLevel[i, o + 1].type == tileTypes.room || workingLevel[i, o - 1].type == tileTypes.room)
                            {
                                workingLevel[i, o].type = tileTypes.room;
                                culledCoridoors++;
                            }
                        }
                    }
                }
            }

           

            //let's get rid of the deadends!
            //total of 50 iterations
            for (int deCull = 0; deCull < 50; deCull++)
            {
                for (int i = 1; i < size.x - 2; i++)
                {
                    for (int o = 1; o < size.y - 2; o++)
                    {
                        int aCells = 0;
                        //check to see if the adjacent cells have stuff
                        if (workingLevel[i, o].type == tileTypes.corridor)
                        {
                            if (workingLevel[i + 1, o].type > 0)
                            {
                                aCells++;
                            }
                            if (workingLevel[i - 1, o].type > 0)
                            {
                                aCells++;
                            }
                            if (workingLevel[i, o + 1].type > 0)
                            {
                                aCells++;
                            }
                            if (workingLevel[i, o - 1].type > 0)
                            {
                                aCells++;
                            }

                            //if there's less than 2 stuff, cull
                            if (aCells < 2)
                            {
                                workingLevel[i, o].type = 0;
                                culledCoridoors++;
                            }
                        }
                    }
                }
            }


            //debug, number of doors
            int doors = 0;

            //let's place some doors!
            for (int i = 1; i < size.x - 2; i++)
            {
                for (int o = 1; o < size.y - 2; o++)
                {
                    if (workingLevel[i, o].type == tileTypes.corridor)
                    {
                        bool makeDoor = false;
                        //check to see if the adjacent cells are rooms
                        //then if the opisite adjacent cel is a coridor
                        if (workingLevel[i + 1, o].type == tileTypes.corridor && workingLevel[i - 1, o].type == tileTypes.room)
                        {
                            makeDoor = true;
                        }
                        if (workingLevel[i + 1, o].type == tileTypes.room && workingLevel[i - 1, o].type == tileTypes.corridor)
                        {
                            makeDoor = true;
                        }
                        if (workingLevel[i, o + 1].type == tileTypes.corridor && workingLevel[i, o - 1].type == tileTypes.room)
                        {
                            makeDoor = true;
                        }
                        if (workingLevel[i, o + 1].type == tileTypes.room && workingLevel[i, o - 1].type == tileTypes.corridor)
                        {
                            makeDoor = true;
                        }

                        int aCells = 0;
                        //check to see if the adjacent cells have corridors
                        if (workingLevel[i + 1, o].type == tileTypes.corridor)
                        {
                            aCells++;
                        }
                        if (workingLevel[i - 1, o].type == tileTypes.corridor)
                        {
                            aCells++;
                        }
                        if (workingLevel[i, o + 1].type == tileTypes.corridor)
                        {
                            aCells++;
                        }
                        if (workingLevel[i, o - 1].type == tileTypes.corridor)
                        {
                            aCells++;
                        }

                        //if any of the potential doors are true
                        //and there's only the one adjacent coridoor
                        //make a 1/3 random to see if it makes a door
                        if (makeDoor && aCells == 1 && Program.Instance.random.Next(0, 3) == 1)
                        {
                            workingLevel[i, o] = new Door(new Vector2(i,o), tileTypes.door);
                            doors++;
                        }
                    }
                }
            }

            //Now for features!
            bool Done = false;
            while (!Done)
            {
                wx = Program.Instance.random.Next(0, (int)size.x);
                wy = Program.Instance.random.Next(0, (int)size.y);

                if (workingLevel[wx, wy].type == tileTypes.room)
                {
                    workingLevel[wx, wy].type = tileTypes.level_start;
                    Done = true;
                }
            }
            Done = false;
            while (!Done)
            {
                wx = Program.Instance.random.Next(0, (int)size.x);
                wy = Program.Instance.random.Next(0, (int)size.y);

                if (workingLevel[wx, wy].type == tileTypes.room)
                {
                    workingLevel[wx, wy].type = tileTypes.level_end;
                    Done = true;
                }
            }
            Done = false;
            // create a writer and open the file
            TextWriter tw = new StreamWriter(DateTime.Today.Day + " - Level" + size.x+"-"+size.y + ".txt");

            // write a line of text to the file
            string s = "Coridor Iterations: " + cIterations + Environment.NewLine +
                "Room Iterations: " + rIterations + Environment.NewLine +
                "Good Rooms: " + goodRooms + Environment.NewLine +
                "Culled Corridors: " + culledCoridoors + Environment.NewLine +
                "Doors: " + doors + Environment.NewLine +
                "Doubles: " + dou + Environment.NewLine +
                "Tiles: " + tiles + Environment.NewLine + 
                "Width: " + size.x + " Height: " +size.y;

            Console.WriteLine(s);
            tw.WriteLine(s + Environment.NewLine + Environment.NewLine + "Generated Map:");


            //Build the 3D
            //swapping around the order I usually do it, so the Text map comes out good
            for (int o = 0; o < size.y; o++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    switch (workingLevel[i, o].type)
                    {
                        case tileTypes.wall:
                            tw.Write(" ");
                            break;
                        case tileTypes.corridor:
                            tw.Write("X");
                            break;
                        case tileTypes.room:
                            tw.Write("O");
                            break;
                        case tileTypes.door:
                            tw.Write("+");
                            break;
                        case tileTypes.level_start:
                            tw.Write("<");
                            break;
                        case tileTypes.level_end:
                            tw.Write(">");
                            break;
                    }
                   
                }
                tw.Write(tw.NewLine);
            }
            tw.Close();
            Console.WriteLine("end making level arraythingy");
            return workingLevel;

        }

        public ArrayList placeMonsters(Tile[,] level, Vector2 size)
        {
            ArrayList r = new ArrayList();
            int wx = 0;
            int wy = 0;

            bool Done = false;
            while (!Done)
            {
                wx = Program.Instance.random.Next(0, (int)size.x);
                wy = Program.Instance.random.Next(0, (int)size.y);

                if (level[wx, wy].type == tileTypes.room)
                {
                    Monster m = new Monster(new Vector2(wx, wy));
                    m.cRace = characterRaces.nonHuman;
                    m.setupCharacter();
                    Program.Instance.gameManager.addMessage("Mon HP:" + m.maxHP + " Mon Damage: " + m.drMeleeDamage.info());
                    r.Add(m);
                    m.writeCharacter("Mons" + Monster.unique);
                    m.make();
                    Done = true;
                }
            }
            Done = false;

            return r;
        }
    }
}
