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

    [SerializeField]
    public float blockSize = 1f;

    [Header("Scene references")]
    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private GameObject container;

    [SerializeField]
    private IVisualGrid visualGrid = null;

    [Space(10)]
    [Header("Shared Data Model")]
    [SerializeField]
    private List<Vector2Int> explodingKittens = new List<Vector2Int>();

    [SerializeField]
    private int[,] grid;

    [Header("Debug")]
    [SerializeField]
    private BlockGroup group;

    Vector2Int[] checkPositions = new Vector2Int[] { new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(-1,0)};


    public BlockGroup GetCurrentPiece()
    {
        return group;
    }

    void Awake()
    {
        grid = new int[sizeY, sizeX];
        visualGrid = GetComponentInChildren<IVisualGrid>();
        visualGrid.Initialize(sizeX, sizeY, container);
    }

    [ContextMenu("Spawn")]
    public bool Spawn()
    {
        var spawnResponse =  spawner.Spawn( (int) (sizeX/2) - 1, sizeY - 1, 2, container, this);
        
        group = spawnResponse.group;
        return spawnResponse.spawnSucceeded;
    }
    
    [ContextMenu("Next")]
    public bool Tick(float delta)
    {
        //TODO: When rotating this should be ordered by y or else...
        for(int i = group.blocks.Count - 1; i >= 0; i--)
        {
            Block b = group.blocks[i];

            if(!CanMoveTo(b.transform.localPosition + new Vector3(0, -delta - blockSize/2f, 0)))
            {
                LockBlock(b);
                group.blocks.RemoveAt(i);
            }
            else
            {
                b.transform.localPosition += new Vector3(0, -delta, 0);
            }
        }

        ProcessExplosions();

        if(group.blocks.Count == 0)
        {
            bool didSpawn = Spawn();

            return didSpawn;
        }

        return true;
    }

    public bool CanMoveTo(Vector2 pos)
    {
        var gridPosition = ToGridPosition(pos);
        return IsInGridRange(gridPosition) && grid[gridPosition.y, gridPosition.x].DecodeCellType() == BlockType.Empty;
    }

    private void LockBlock(Block b)
    {
        var pos = ToGridPosition(b);
        grid[pos.y, pos.x] = BlockDataHelper.Encode(b.GetBlockType(), b.GetIsBomb(), 0);

        if(b.GetIsBomb())
        {
            explodingKittens.Add(pos);
        }

        visualGrid.AddBlockAt(pos, b);
    }

    public void ProcessExplosions()
    {
        for(int i = explodingKittens.Count - 1; i >= 0; i--)
        {
            var pos = explodingKittens[i];
            
            if(grid[pos.y, pos.x].DecodeCellType() == BlockType.Empty)
            {
                explodingKittens.RemoveAt(i);
                continue;
            }

            if(grid[pos.y, pos.x].DecodeCellType() == BlockType.X)
            {
                var adjacents = GetAdjacentBlocks(pos, BlockType.Empty);

                if(adjacents.Count > 0)
                {
                    explodingKittens.RemoveAt(i);

                    BoomColor( grid[adjacents[0].y, adjacents[0].x].DecodeCellType(), pos);
                    DestroyBlock(pos);

                    RearrangeGrid();
                }
            }
            else
            {
                List<Vector2Int> bombing = Boom(pos);
                
                if(bombing.Count > 0)
                {
                    explodingKittens.RemoveAt(i);

                    foreach(var gb in bombing)
                    {
                        DestroyBlock(gb);
                    }
                    DestroyBlock(pos);

                    RearrangeGrid();
                }
            }
        }
    }

    public List<Vector2Int> Boom(Vector2Int bombBlock)
    {
        List<Vector2Int> boomBlocks = new List<Vector2Int>();

        bool[,] checkedPositions = new bool[sizeY, sizeX];
        Queue<Vector2Int> checkingBlocks = new Queue<Vector2Int>();
        checkingBlocks.Enqueue(bombBlock);
        checkedPositions[bombBlock.y, bombBlock.x] = true;

        while(checkingBlocks.Count > 0)
        {
            Vector2Int b = checkingBlocks.Dequeue();
            List<Vector2Int> adjacent = GetAdjacentBlocks(ToGridPosition(b), grid[b.y, b.x].DecodeCellType());

            foreach(var ab in adjacent)
            {
                if(!checkedPositions[ab.y, ab.x])
                {
                    checkedPositions[ab.y, ab.x] = true;
                    checkingBlocks.Enqueue(ab);
                    boomBlocks.Add(ab);
                }
            }
        }

        return boomBlocks;
    }

    public void BoomColor(BlockType type, Vector2 gridPostion)
    {
        for(int y = 0; y < grid.GetLength(0); y++)
        {
            for(int x = 0; x < grid.GetLength(1); x++)
            {
                if(grid[y,x].DecodeCellType() != BlockType.Empty && grid[y,x].DecodeCellType() == type)
                {
                    DestroyBlock(new Vector2Int(x,y));
                }
            }
        }
    }

    private List<Vector2Int> GetAdjacentBlocks(Vector2Int gridPosition, BlockType type)
    {
        List<Vector2Int> adjacentBlocks = new List<Vector2Int>();

        foreach(var cp in checkPositions)
        {
            var adjacentPosition = gridPosition + cp;

            if(IsInGridRange(adjacentPosition))
            {
                var block = grid[adjacentPosition.y, adjacentPosition.x].DecodeCellType();

                if(block != BlockType.Empty)
                {
                    if(type == BlockType.Empty || type == block)
                    {
                        adjacentBlocks.Add(adjacentPosition);
                    }
                }
            }
        }
        
        return adjacentBlocks;
    }

    //TODO: NETCODE!
    private void DestroyBlock(Vector2Int pos)
    {
        grid[pos.y,pos.x] = 0;
        visualGrid.RemoveBlockAt(pos);
    }

    public void RearrangeGrid()
    {
        for(int x = 0; x < grid.GetLength(1); x++)
        {
            int freeY = grid.GetLength(0);

            for(int y = 0; y < grid.GetLength(0); y++)
            {
                if(grid[y,x].DecodeCellType() == BlockType.Empty)
                {
                    if(y < freeY)
                    {
                        freeY = y;
                    }
                }
                else
                {
                    //TODO: Combo blocks should move together
                    if(y > freeY)
                    {
                        grid[freeY,x] = grid[y,x];
                        grid[y,x] = 0;

                        visualGrid.MoveBlock(new Vector2Int(x,y), new Vector2Int(x, freeY));
                        freeY++;
                    }   
                }
            }
        }
    }

#region GridHelpers
    public Vector2Int ToGridPosition(Block block)
    {
        return new Vector2Int( Mathf.CeilToInt(block.transform.localPosition.x), Mathf.CeilToInt(block.transform.localPosition.y));
    }

    public Vector2Int ToGridPosition(Vector2 position)
    {
        return new Vector2Int( Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
    }

    public Vector2Int ToGridPosition(Vector3 position)
    {
        return new Vector2Int( Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
    }

    public bool IsInGridRange(Vector2Int position)
    {
        return position.x >= 0 && position.x < sizeX && position.y >= 0 && position.y < sizeY;
    }
#endregion
}
