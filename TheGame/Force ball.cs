using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Text;
using Mogre;
using MogreNewt;
using Mogre.Cegui;
using CeguiDotNet;

namespace TheGame
{
    class ForceBall
    {
        public Entity ent;
        public SceneNode sn;
        public MogreNewt.Body body;
        float mass = 250;
        public float size;

        static int unique;

        public float live = 0.0f;
        ParticleSystem trail;

        public ColourValue colour;

        public int owner;

        Timer time;

        public ForceBall(SceneManager sceneManager, MogreNewt.World physicsWorld, float tempSize, int own)
        {

            MogreNewt.ConvexCollision col;
            Mogre.Vector3 offset;
            Mogre.Vector3 inertia;

            unique++;

            // Create the visible mesh (no physics)
            ent = sceneManager.CreateEntity("forceball" + unique, "Sph.mesh");
            sn = sceneManager.RootSceneNode.CreateChildSceneNode();
            sn.AttachObject(ent);
            Console.WriteLine("end ball create");
            size = tempSize;


            sn.SetScale(size, size, size);

            // Create the collision hull
            col = new MogreNewt.CollisionPrimitives.ConvexHull(physicsWorld, sn);
            col.calculateInertialMatrix(out inertia, out offset);

            // Create the physics body. This body is what you manipulate. The graphical Ogre scenenode is automatically synced with the physics body
            body = new MogreNewt.Body(physicsWorld, col);
            col.Dispose();

            //body.setPositionOrientation(new Vector3(0, 10, 0), Quaternion.IDENTITY);

            body.attachToNode(sn);
            body.setContinuousCollisionMode(1);
            body.setMassMatrix(mass, mass * inertia);
            body.IsGravityEnabled = true;
            body.setUserData(this);

            trail = sceneManager.CreateParticleSystem("awesome" + StringConverter.ToString(unique), "Char/Smoke");
            trail.CastShadows = true;
            trail.GetEmitter(0).EmissionRate = 100;
            sn.AttachObject(trail);

            Console.WriteLine("player stuff");

            owner = own;
            Player tempP = (Player)Program.p1;
            colour = tempP.cv;
        }

        public void Dispose()
        { 
            body.Dispose();
            ent.Visible = false;

            trail.GetEmitter(0).EmissionRate = 0;
            trail.GetEmitter(0).Dispose();
            trail.Dispose();
           
            sn.Dispose();
        }
    }
}
