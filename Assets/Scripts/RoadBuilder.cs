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

    List<RoadSection> sections;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        halfQuadSize = quadSize / 2;

        sections = new List<RoadSection>();
    }

    public void Start()
    {
        RoadSection road = new RoadSection();
        CreateStraightRoadQuad(Vector3.zero, (int)rotation, ref road);
        sections.Add(road);
        road = new RoadSection();
        CreateStraightRoadQuad(new Vector3(quadSize, 0, 0), (int)rotation, ref road);
        sections.Add(road);
        road = new RoadSection();
        CreateStraightRoadQuad(new Vector3(quadSize * 2, 0, 0), (int)rotation, ref road);
        sections.Add(road);
        //CreateBigCurveSideWalk(Vector3.zero,(int)rotation, ref road);
        //TestCurves(Vector3.zero, (int)rotation, ref road);
        CombineMesh();
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

        CreateQuad(ref road,
            new Vector3(-sideWidth, 0, halfQuadSize).RotateVector(rotation) + innerHeightOffset,
            new Vector3(-sideWidth, 0, -halfQuadSize).RotateVector(rotation) + innerHeightOffset,
            new Vector3(sideWidth, 0, halfQuadSize).RotateVector(rotation) + innerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize).RotateVector(rotation) + innerHeightOffset,
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
            rotate.GetOppositeRotation();
        }

        CreateQuad(ref road,
            new Vector3(fullWidth, 0, halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            new Vector3(fullWidth, 0, -halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            new Vector3(fullWidth, 0, halfQuadSize).RotateVector(rotate) + offset,
            new Vector3(fullWidth, 0, -halfQuadSize).RotateVector(rotate) + offset,
            roadSettings.sideWalkColor);

        CreateQuad(ref road,
            new Vector3(sideWidth, 0, halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            new Vector3(fullWidth, 0, halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            new Vector3(fullWidth, 0, -halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            roadSettings.sideWalkColor);


        CreateQuad(ref road,
            new Vector3(sideWidth, 0, halfQuadSize).RotateVector(rotate) + innerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize).RotateVector(rotate) + innerHeightOffset,
            new Vector3(sideWidth, 0, halfQuadSize).RotateVector(rotate) + outerHeightOffset,
            new Vector3(sideWidth, 0, -halfQuadSize).RotateVector(rotate) + outerHeightOffset,
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
            CreateQuad(ref road, 
                outerCurve[i] + offset, 
                outerCurve[i + 1] + offset, 
                outerCurve[i] + outerHeightOffset, 
                outerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
            //floor
            CreateQuad(ref road, 
                outerCurve[i] + outerHeightOffset, 
                outerCurve[i + 1] + outerHeightOffset, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
            //inner wall
            CreateQuad(ref road, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset, 
                innerCurve[i] + innerHeightOffset, 
                innerCurve[i + 1] + innerHeightOffset,
                roadSettings.sideWalkColor);
        }
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
            CreateQuad(ref road, 
                outerCurve[i]+ outerHeightOffset,
                outerCurve[i + 1] + outerHeightOffset, 
                outerCurve[i] + offset, 
                outerCurve[i + 1] + offset,
                roadSettings.sideWalkColor);
            //floor
            CreateQuad(ref road, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset, 
                outerCurve[i] + outerHeightOffset, 
                outerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
            //inner wall
            CreateQuad(ref road, 
                innerCurve[i] + innerHeightOffset, 
                innerCurve[i + 1] + innerHeightOffset, 
                innerCurve[i] + outerHeightOffset, 
                innerCurve[i + 1] + outerHeightOffset,
                roadSettings.sideWalkColor);
        }
    }

    void CreateQuad(ref RoadSection road, Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, Color color)
    {
        int offset = road.vertices.Count;
        road.vertices.Add(a1);
        road.vertices.Add(a2);
        road.vertices.Add(b1);
        road.vertices.Add(b2);

        road.colors.Add(color);
        road.colors.Add(color);
        road.colors.Add(color);
        road.colors.Add(color);

        road.triangles.Add(offset + 0);
        road.triangles.Add(offset + 2);
        road.triangles.Add(offset + 1);

        road.triangles.Add(offset + 1);
        road.triangles.Add(offset + 2);
        road.triangles.Add(offset + 3);

        
    }

    void CombineMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();


        for(int i = 0; i < sections.Count; i++)
        {
            int trianglesOffset = vertices.Count;
            vertices.AddRange(sections[i].vertices);
            triangles.AddRange(sections[i].GetTriangles(trianglesOffset));
            colors.AddRange(sections[i].colors);
        }
        SetMesh(vertices, triangles, colors);
    }

    void SetMesh(List<Vector3> vertices, List<int> triangles, List<Color> colors)
    {
        mesh.SetVertices(vertices.ToArray());
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.SetColors(colors);
        mesh.RecalculateNormals();
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

    public int[] GetTriangles(int offset)
    {
        int[] result = new int[triangles.Count];

        for(int i = 0; i < triangles.Count; i++)
        {
            result[i]=triangles[i] + offset;
        }

        return result;
    }
}
