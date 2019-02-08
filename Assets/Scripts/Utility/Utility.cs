using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace Sierra
{
    static class Utility
    {
        private static System.Random Random = new System.Random();

        public static float FramesToSeconds(int frames, int framesPerSecond = 60)
        {
            return Convert.ToSingle(frames) / Convert.ToSingle(framesPerSecond);
        }
        public static IEnumerator FrameTimer(int timerDuration, int timer)
        {
            while (timer < timerDuration)
            {
                timer++;
                yield return new WaitForFixedUpdate();
            }
        }

        public static float GetRandomFloat()
        {
            return Convert.ToSingle(Random.NextDouble());
        }

        public static void DrawCircle(Vector2 pos, float radius, Color colour)
        {
            Handles.color = colour;
            Handles.DrawWireDisc(pos, Vector3.forward, radius);
        }
    }
}