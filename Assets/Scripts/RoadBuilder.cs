using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour
{
    public bool isRight;
    public QuadSide rotation = QuadSide.TopRight;
    public float quadSize = 1f;
    float halfQuadSize;
    public RoadSettings roadSettings;
    public MeshFilter meshFilter;
    private Mesh mesh;

    Dictionary<string, RoadSection> meshSections;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        halfQuadSize = quadSize / 2;
    }

    public void Start()
    {
        RoadSection road = new RoadSection();
        CreateStraightRoadQuad(Vector3.zero, (int)rotation, ref road);
        //CreateBigCurveSideWalk(Vector3.zero,(int)rotation, ref road);
        //TestCurves(Vector3.zero, (int)rotation, ref road);
        SetMesh(road);
    }
    public void PlaceRoad()
    {

    }

    void CreateRoad(Vector3 offset)
    {

    }

    void CreateStraightRoadQuad(Vector3 offset, int rotation, ref RoadSection road)
    {
        Vector3 innerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight, offset.z);

        float fullWidth = halfQuadSize * roadSettings.fullWidth;
        float sideWidth = fullWidth - fullWidth * roadSettings.sideWidth;

        CreateStraightSideWalk(offset, rotation, true, ref road);
        CreateStraightSideWalk(offset, rotation, false, ref road);

        CreateQuad(road,
            new Vector3(-fullWidth, 0, halfQuadSize) + innerHeightOffset,
            new Vector3(-fullWidth, 0, -halfQuadSize) + innerHeightOffset,
            new Vector3(fullWidth, 0, halfQuadSize) + innerHeightOffset,
            new Vector3(fullWidth, 0, -halfQuadSize) + innerHeightOffset,
            rotation,
                roadSettings.roadColor);
    }

    void CreateStraightSideWalk(Vector3 offset, int rotation, bool isRight, ref RoadSection road)
    {
        Vector3 outerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight + roadSettings.sideWalkHeight, offset.z);
        Vector3 innerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight, offset.z);

        float fullWidth = halfQuadSize * roadSettings.fullWidth;
        float sideWidth = fullWidth - fullWidth * roadSettings.sideWidth;
        int rotate = rotation;
        if (!isRight)
        {
            rotate = (rotation + 2)%4;
        }

        CreateQuad(road,
            new Vector3(fullWidth, 0, halfQuadSize) + outerHeightOffset,
            new Vector3(fullWidth, 0, -halfQuadSize) + outerHeightOffset,
            new Vector3(fullWidth, 0, halfQuadSize) + offset,
            new Vector3(fullWidth, 0, -halfQuadSize) + offset,
            rotate,
                roadSettings.sideWalkColor);

        CreateQuad(road,
            new Vector3(sideWidth, 0, halfQuadSize) + outerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize) + outerHeightOffset,
            new Vector3(fullWidth, 0, halfQuadSize) + outerHeightOffset,
            new Vector3(fullWidth, 0, -halfQuadSize) + outerHeightOffset,
            rotate,
                roadSettings.sideWalkColor);


        CreateQuad(road,
            new Vector3(sideWidth, 0, halfQuadSize) + innerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize) + innerHeightOffset,
            new Vector3(sideWidth, 0, halfQuadSize) + outerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize) + outerHeightOffset,
            rotate,
                roadSettings.sideWalkColor);

    }

    void CreateBigCurveSideWalk(Vector3 offset, int rotation, ref RoadSection road)
    {
        Vector3 outerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight +roadSettings.sideWalkHeight, offset.z);
        Vector3 innerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight, offset.z);
       // RoadSection road = new RoadSection();

        List<Vector3> outerCurve = Util.GetCirclePoint(roadSettings.bigCurvePolyCount, halfQuadSize * roadSettings.fullWidth + halfQuadSize, halfQuadSize, rotation);
        List<Vector3> innerCurve = Util.GetCirclePoint(roadSettings.bigCurvePolyCount, halfQuadSize * (1 - roadSettings.fullWidth * (1 - roadSettings.sideWidth)) + halfQuadSize, halfQuadSize, rotation);
           
        for(int i = 0; i < roadSettings.bigCurvePolyCount; i++)
        {
            //outer wall
            CreateQuad(road, 
                outerCurve[i] + offset, 
                outerCurve[i + 1] + offset, 
                outerCurve[i] + outerHeightOffset, 
                outerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
            //floor
            CreateQuad(road, 
                outerCurve[i] + outerHeightOffset, 
                outerCurve[i + 1] + outerHeightOffset, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
            //inner wall
            CreateQuad(road, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset, 
                innerCurve[i] + innerHeightOffset, 
                innerCurve[i + 1] + innerHeightOffset,
                roadSettings.sideWalkColor);
        }
        //return road;
        //SetMesh(road);
    }



    void CreateSmallCurveSideWalk(Vector3 offset, int rotation, ref RoadSection road)
    {
        Vector3 outerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight + roadSettings.sideWalkHeight, offset.z);
        Vector3 innerHeightOffset = new Vector3(offset.x, offset.y + roadSettings.roadHeight, offset.z);

        //RoadSection road = new RoadSection();

        List<Vector3> innerCurve = Util.GetCirclePoint(roadSettings.smallCurvePolyCount, halfQuadSize * (1 - roadSettings.fullWidth * (1 - roadSettings.sideWidth)), halfQuadSize, rotation);
        List<Vector3> outerCurve = Util.GetCirclePoint(roadSettings.smallCurvePolyCount, halfQuadSize * (1 - roadSettings.fullWidth), halfQuadSize, rotation);

        for (int i = 0; i < roadSettings.smallCurvePolyCount; i++)
        {
            //outer wall
            CreateQuad(road, 
                outerCurve[i]+ outerHeightOffset,
                outerCurve[i + 1] + outerHeightOffset, 
                outerCurve[i] + offset, 
                outerCurve[i + 1] + offset,
                roadSettings.sideWalkColor);
            //floor
            CreateQuad(road, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset, 
                outerCurve[i] + outerHeightOffset, 
                outerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
            //inner wall
            CreateQuad(road, 
                innerCurve[i] + innerHeightOffset, 
                innerCurve[i + 1] + innerHeightOffset, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
        }
        //return road;
        //SetMesh(road);
    }
    void CreateQuad(RoadSection road, Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, int rotation, Color color)
    {
        CreateQuad(road,
            Quaternion.AngleAxis(Util.rotationAngle[rotation], Vector3.up) * a1,
            Quaternion.AngleAxis(Util.rotationAngle[rotation], Vector3.up) * a2,
            Quaternion.AngleAxis(Util.rotationAngle[rotation], Vector3.up) * b1,
            Quaternion.AngleAxis(Util.rotationAngle[rotation], Vector3.up) * b2,
            color);

    }

    void CreateQuad(RoadSection road, Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, Color color)
    {
        int offsetVertices = road.vertices.Count;
        road.vertices.Add(a1);
        road.vertices.Add(a2);
        road.vertices.Add(b1);
        road.vertices.Add(b2);

        road.colors.Add(color);
        road.colors.Add(color);
        road.colors.Add(color);
        road.colors.Add(color);

        road.triangles.Add(offsetVertices + 0);
        road.triangles.Add(offsetVertices + 2);
        road.triangles.Add(offsetVertices + 1);

        road.triangles.Add(offsetVertices + 1);
        road.triangles.Add(offsetVertices + 2);
        road.triangles.Add(offsetVertices + 3);

        
    }

    void SetMesh(RoadSection road)
    {
        mesh.SetVertices(road.vertices.ToArray());
        mesh.SetTriangles(road.triangles.ToArray(), 0);
        mesh.SetColors(road.colors);
        mesh.RecalculateNormals();
    }

    void CreateBigCircle(Vector3 offset)
    {
        RoadSection road = new RoadSection();

    }

    void CreateFullRoad(Vector3 offset)
    {
        RoadSection road = new RoadSection();

        for (int i = 0; i < 4; i++)
        {
            road.vertices.Add(new Vector3(halfQuadSize * Util.quadOffsetsMultiplier[i].x + offset.x,
                offset.y,
                halfQuadSize * Util.quadOffsetsMultiplier[i].y + offset.z));
            road.colors.Add(roadSettings.roadColor);
        }

        road.triangles.Add(0);
        road.triangles.Add(1);
        road.triangles.Add(2);
        road.triangles.Add(0);
        road.triangles.Add(2);
        road.triangles.Add(3);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        //Gizmos.DrawWireSphere(Vector3.zero, quadSize / 2f);
        Gizmos.DrawWireSphere(Vector3.zero, quadSize/2f * roadSettings.fullWidth * roadSettings.sideWidth);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(quadSize/2f,0,quadSize/2f), quadSize/2 * (1 - roadSettings.fullWidth*(1-roadSettings.sideWidth)));

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(quadSize / 2f, 0, quadSize / 2f), quadSize/2 * (1-roadSettings.fullWidth));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3((quadSize/2)*roadSettings.fullWidth,0, quadSize / 2), Vector3.one * 0.05f);
        Gizmos.DrawCube(new Vector3((quadSize / 2) * roadSettings.fullWidth, 0, -quadSize / 2), Vector3.one * 0.05f);
        Gizmos.DrawCube(new Vector3(-(quadSize / 2) * roadSettings.fullWidth, 0, -quadSize / 2), Vector3.one * 0.05f);
        Gizmos.DrawCube(new Vector3(-(quadSize / 2) * roadSettings.fullWidth, 0, quadSize / 2), Vector3.one * 0.05f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(new Vector3((quadSize / 2) * roadSettings.fullWidth*(1-roadSettings.sideWidth), 0, quadSize / 2), Vector3.one * 0.05f);
        Gizmos.DrawCube(new Vector3((quadSize / 2) * roadSettings.fullWidth * (1 - roadSettings.sideWidth), 0, -quadSize / 2), Vector3.one * 0.05f);
        Gizmos.DrawCube(new Vector3(-(quadSize / 2) * roadSettings.fullWidth * (1 - roadSettings.sideWidth), 0, -quadSize / 2), Vector3.one * 0.05f);
        Gizmos.DrawCube(new Vector3(-(quadSize / 2) * roadSettings.fullWidth * (1 - roadSettings.sideWidth), 0, quadSize / 2), Vector3.one * 0.05f);
    }
}

public class RoadSection
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Color> colors;

    public RoadSection()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    public List<int> GetTriangles(int offset)
    {
        List<int> result = new List<int>();

        for(int i = 0; i < triangles.Count; i++)
        {
            result.Add(triangles[i] + offset);
        }

        return result;
    }
}
