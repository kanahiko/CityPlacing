using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour
{
    public float quadSize = 1f;
    float halfQuadSuze;
    public RoadSettings roadSettings;
    public MeshFilter meshFilter;
    private Mesh mesh;

    Dictionary<string, RoadSection> meshSections;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        halfQuadSuze = quadSize / 2;
    }
    public void PlaceRoad()
    {

    }

    void CreateStraightRoadQuad(Vector2 position, int rotation)
    {
        float fullWidth = quadSize * roadSettings.fullWidth;
        float roadWidth = fullWidth - quadSize * roadSettings.sideWidth;
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
            road.vertices.Add(new Vector3(halfQuadSuze * Util.quadOffsetsMultiplier[i].x + offset.x,
                offset.y,
                halfQuadSuze * Util.quadOffsetsMultiplier[i].y + offset.z));
            road.colors.Add(roadSettings.roadColor);
        }

        road.triangles.Add(0);
        road.triangles.Add(1);
        road.triangles.Add(2);
        road.triangles.Add(0);
        road.triangles.Add(2);
        road.triangles.Add(3);
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
