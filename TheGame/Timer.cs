using System;
using Mogre;

namespace TheGame
{
    class Timer
    {
        
       //     Root.Singleton.Timer.Milliseconds

        public float globalTime;
        float gameStartTime;
        public float gameLength;
        public bool gameOver;

        public Timer()
        {
            globalTime = 0;
            gameOver = true;
        }

        public void GameStart(float length)
        {
            gameStartTime = globalTime;
            gameOver = false;
            gameLength = length;
        }

        public void GameStart(int length)
        {

        }

        public float GetGameTimer()
        {
            float gametime = globalTime - gameStartTime;
            return gametime;
        }

        public void update(float frametime)
        {
            globalTime += frametime;

        }

    }
}
