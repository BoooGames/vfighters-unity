using System.Collections.Generic;
using UnityEngine;

public struct BlockGroup
{
    public List<Block> blocks;

    public void Move()
    {

    }


    public void Rotate()
    {

    }

    public void Tick()
    {
        foreach(var b in blocks)
        {
            b.transform.position += Vector3.down;
        }
    }
}