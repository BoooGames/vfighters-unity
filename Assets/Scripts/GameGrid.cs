using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private int sizeX = 5;
    [SerializeField]
    private int sizeY = 10;

    [Header("Scene references")]
    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private GameObject container;

    [Header("Debug")]
    [SerializeField]
    private BlockType[] grid = new BlockType[10*5];

    [SerializeField]
    private BlockGroup group;

    public BlockGroup GetCurrentPiece()
    {
        throw new NotImplementedException();
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        group = spawner.Spawn( (int) (sizeX/2) - 1, sizeY, 2, container);
    }
    
    [ContextMenu("Next")]
    public void Next()
    {
        group.Tick();
    }
}
