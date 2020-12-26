using UnityEngine;
using System.Collections;

public static class Util
{
    public static Vector2[] quadOffsetsMultiplier = new Vector2[]
    {
        new Vector2(-1,1),new Vector2(1,1),new Vector2(1,-1),new Vector2(-1,-1)
    };

    public static int[] rotation = new int[]
    {
        270,0,90,180
    };


    public static Vector2[] GetSmallCirclePoint(int count, float radius, float quadSize)
    {
        Vector2[] result = new Vector2[count + 1];

        result[0] = new Vector2(radius, halfQuadSize);
        result[count] = new Vector2(halfQuadSize, radius);

        float angleStep = 90 / count;

        for(int i = 1; i < count; i++)
        {
            result[i] = new Vector3(radius * Mathf.Sin(Mathf.Deg2Rad*(angleStep*i)),
                radius * Mathf.Sin(Mathf.Deg2Rad * (90-(angleStep * i))));
        }

        return result;
    }
}

public enum QuadSide
{
   TopLeft = 0, TopRight = 1,
   BottomRight = 2, BottomLeft = 3
}
