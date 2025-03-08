using System;
using UnityEngine;

namespace Helpers.Extensions
{
    public static class MathfUtilities
    {
        public static Vector2 RoundVector(this Vector2 vector2)
        {
            return new Vector2((float)Math.Round(vector2.x, 2), (float)Math.Round(vector2.y, 2));
        }
    }
}