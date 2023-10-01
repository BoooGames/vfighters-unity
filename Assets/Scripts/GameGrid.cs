using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    public int sizeX = 5;
    [SerializeField]
    public int sizeY = 10;

    [Header("Scene references")]
    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private GameObject container;

    [Header("Debug")]
    [SerializeField]
    private BlockType[,] grid;

    [SerializeField]
    private BlockGroup group;

    public BlockGroup GetCurrentPiece()
    {
        return group;
    }

    void Awake()
    {
        grid = new BlockType[sizeY, sizeX];
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        group = spawner.Spawn( (int) (sizeX/2) - 1, sizeY, 2, container, this);
    }
    
    [ContextMenu("Next")]
    public void Tick(float delta)
    {
        for(int i = group.blocks.Count - 1; i >= 0; i--)
        {
            Block b = group.blocks[i];
            b.transform.localPosition += new Vector3(0, -delta, 0);
            var pos = ToGridPosition(b);

            if(pos.y == 0)
            {
                LockBlock(b);
                group.blocks.RemoveAt(i);
            }
        }

        if(group.blocks.Count == 0)
        {
            Spawn();
        }
    }

    private void LockBlock(Block b)
    {
        var pos = ToGridPosition(b);

        b.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        Debug.LogWarning("Update grid?");
    }

    public (int x, int y) ToGridPosition(Block block)
    {
        return ( Mathf.CeilToInt(block.transform.localPosition.x), Mathf.CeilToInt(block.transform.localPosition.y));
    }

    private void OnGui()
    {
        
    }
}
