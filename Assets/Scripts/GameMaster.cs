using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public QuadTree terrainFeatures;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class QuadTree
{
    public Rect boundaries = new Rect(-5, -5, 10, 10);
    public int capacity;
    public QuadTree[] parts;

    public QuadTree(Rect boundaries)
    {
        this.boundaries = boundaries;
        capacity = 0;
        /*parts = new Quadrant[4];
        for(int i = 0; i < 4; i++)
        {
            parts[i] = new Quadrant();
        }*/
    }

    public void Insert(Vector2 position)
    {
        capacity++;
    }

    public void Subdivide()
    {
        parts = new QuadTree[4];
        float width = boundaries.width / 2;
        float height = boundaries.height / 2;
        float quadWidth = boundaries.width / 4;
        float quadHeight = boundaries.height / 4;
        for (int i = 0; i < 4; i++)
        {
            parts[i] = new QuadTree(new Rect(boundaries.x + Util.quadOffsetsMultiplier[i].x* quadWidth, boundaries.y + Util.quadOffsetsMultiplier[i].y * quadHeight, width, height));
        }
    }
}
