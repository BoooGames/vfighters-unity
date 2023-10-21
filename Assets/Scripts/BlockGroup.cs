using System;
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
        bool canMove = true;
        foreach(var b in blocks)
        {
           canMove = canMove && grid.CanMoveTo(new Vector2(ComputeNewX(b, grid), b.transform.localPosition.y));
        }

        if(canMove)
        {
            foreach(var b in blocks)
            {
                b.transform.localPosition = new Vector3(ComputeNewX(b, grid), b.transform.localPosition.y , b.transform.localPosition.z);
            }
        }

        float ComputeNewX(Block b, GameGrid grid)
        {
            int newX = ((int) b.transform.localPosition.x + increment) % grid.sizeX; 
            newX = newX < 0 ? grid.sizeX - newX - 2 : newX;
            return newX;
        }
    }

    public void MoveVertical(int increment)
    {   
        bool canMove = true;
        foreach(var b in blocks)
        {
           canMove = canMove && grid.CanMoveTo(new Vector2(b.transform.localPosition.x, ComputeNewY(b, grid)));
        }

        if(canMove)
        {
            foreach(var b in blocks)
            {
                b.transform.localPosition = new Vector3(b.transform.localPosition.x, ComputeNewY(b, grid) , b.transform.localPosition.z);
            }
        }

        float ComputeNewY(Block b, GameGrid grid)
        {
            return Math.Clamp(b.transform.localPosition.y + increment, 0, grid.sizeY - 1);
        }
    }

    public void SnapDown()
    {
        //TODO: SNAP ON MOBILE
        //Debug.LogWarning("SNAP!");
    }


    public void RotateRight()
    {
        
    }

    public void RotateLeft()
    {

    }
}