using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Util
{
    public static Vector2[] quadOffsetsMultiplier = new Vector2[]
    {
        new Vector2(1,1),new Vector2(1,-1),new Vector2(-1,-1),new Vector2(-1,1)
    };

    public static int[] rotationAngle = new int[]
    {
        0,90,180,270
    };

    public static void GetOppositeRotation(this int rotation)
    {
        rotation = (rotation + 2) % 4;
    }

    public static Vector3 RotateVector(this Vector3 point, int rotation)
    {
        return Quaternion.AngleAxis(Util.rotationAngle[rotation], Vector3.up) * point;
    }

    public static List<Vector3> GetCirclePoint(int count, float radius, float quadSize, int rotation)
    {
        //Debug.Log(radius);
        List<Vector3> result = new List<Vector3>();

        //result[0] = new Vector3(quadSize - radius, 0, quadSize);
        //result[count] = new Vector3(quadSize, 0, quadSize - radius);
        //Debug.Log(quadOffsetsMultiplier[rotation]);
        float angleStep = 90 / count;

        //Debug.Log(result[0]);
        for (int i = 0; i < count + 1; i++)
        {

            Vector3 res = Quaternion.AngleAxis(rotationAngle[rotation], Vector3.up) * (new Vector3(-radius * Mathf.Sin(Mathf.Deg2Rad * (90 - angleStep * i)) + quadSize,
                0,
                -radius * Mathf.Sin(Mathf.Deg2Rad * ((angleStep * i))) + quadSize));
            //res.x *= quadOffsetsMultiplier[rotation].x;
            //res.z *= quadOffsetsMultiplier[rotation].y;
            result.Add(res);
           // Debug.Log(result[i]);
        }
        //Debug.Log(result[count]);

        return result;
    }
}


public enum QuadSide
{
   TopLeft = 3, TopRight = 0,
   BottomRight = 1, BottomLeft = 2
}
