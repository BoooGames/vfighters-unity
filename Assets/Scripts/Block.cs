using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockType blockType;
    private bool isBomb;
    private int timer;
   
    public void Initialize(BlockType blockType, bool isBomb, int timer)
    {
        this.blockType = blockType;
        this.isBomb = isBomb;
        this.timer = timer;
    }

    public BlockType GetBlockType()
    {
        return blockType;
    }

    public bool GetIsBomb()
    {
        return isBomb;
    }

    public int GetTimer()
    {
        return timer;
    }
}
