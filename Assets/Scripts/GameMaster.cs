using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public QuadTree terrainFeatures;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}


public class QuadTree
{
    public Rect boundaries = new Rect(0, 0, 10, 10);
    public int capacity;
    public QuadTree[] parts;

    RoadObject block;

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

    public void Insert(RoadObject newBlock)
    {      
        if (parts == null && block != null)
        {
            block = newBlock;
            block.quad = this;
        }
        else
        {
            if (parts == null)
            {
                Subdivide();
            }
            if (block != null)
            {
                //reinsert current block
                for (int i = 0; i < 4; i++)
                {
                    if (boundaries.Contains(block.position))
                    {
                        parts[i].Insert(block);
                        break;
                    }
                }
                block = null;
            }
            //insert new block
            for (int i = 0; i < 4; i++)
            {
                if (boundaries.Contains(newBlock.position))
                {
                    parts[i].Insert(newBlock);
                    break;
                }
            }
        }
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
