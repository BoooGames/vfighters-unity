using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    public bool isBomb = false;

    public BlockType GetBlockType()
    {
        return blockType;
    }
}
