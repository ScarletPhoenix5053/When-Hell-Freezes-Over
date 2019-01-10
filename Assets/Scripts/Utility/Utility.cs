using UnityEngine;
using System;

namespace Sierra
{
    static class Utility
    {
        public static float FramesToSeconds(int frames, int framesPerSecond = 60)
        {
            return Convert.ToSingle(frames) / Convert.ToSingle(framesPerSecond);
        }
        /// <summary>
        /// Use to throw an exception when a monobehaviour is missing an essential component.
        /// </summary>
        /// <param name="name">Name of the <see cref="MonoBehaviour"/></param>
        public static void ThrowNoComponentException(string name)
        {
            throw new MissingComponentException(name + " is missing an essential component!");
        }
    }
}