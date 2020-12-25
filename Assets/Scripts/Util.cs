using UnityEngine;
using System.Collections;

public static class Util
{
    public static Vector2[] quadOffsetsMultiplier = new Vector2[]
    {
        new Vector2(-1,1),new Vector2(1,1),new Vector2(1,-1),new Vector2(-1,-1)
    };

}

public enum QuadSide
{
   TopLeft = 0, TopRight = 1,
   BottomRight = 2, BottomLeft = 3
}
