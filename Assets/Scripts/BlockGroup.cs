using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public struct BlockGroup
{
    public List<Block> blocks;
    private GameGrid grid;

    public BlockGroup(List<Block> blocks, GameGrid grid)
    {
        this.blocks = blocks;
        this.grid =  grid;
    }

    public void MoveHorizontal(int increment)
    {   
        foreach(var b in blocks)
        {
            int newX = ((int) b.transform.localPosition.x + increment) % grid.sizeX; 
            newX = newX < 0 ? grid.sizeX - newX - 2 : newX;            
            b.transform.localPosition = new Vector3(newX, b.transform.localPosition.y , b.transform.localPosition.z);
        }
    }

    public void SnapDown()
    {
        Debug.LogWarning("SNAP!");
    }


    public void RotateRight()
    {

    }

    public void RotateLeft()
    {

    }
}