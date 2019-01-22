using UnityEngine;
using System;
using System.Collections;

namespace Sierra
{
    static class Utility
    {
        public static float FramesToSeconds(int frames, int framesPerSecond = 60)
        {
            return Convert.ToSingle(frames) / Convert.ToSingle(framesPerSecond);
        }
        public static IEnumerator FrameTimer(int timerDuration, int timer)
        {
            while (timer < timerDuration)
            {
                if (!GameManager.Instance.HitStopActive) timer++;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}