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

    [Header("Debug")]
    [SerializeField]
    private Block[,] grid;

    [SerializeField]
    private BlockGroup group;

    Vector2Int[] checkPositions = new Vector2Int[] { new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(-1,0)};

    //Debug!
    [SerializeField]
    private List<Block> explodingKittens = new List<Block>();

    public BlockGroup GetCurrentPiece()
    {
        return group;
    }

    void Awake()
    {
        grid = new Block[sizeY, sizeX];
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

    public bool CanMoveTo(int posX, int posY)
    {
        return grid[posY, posX] == null;
    }

    public bool CanMoveTo(Vector2 pos)
    {
        var gridPosition = ToGridPosition(pos);
        return IsInGridRange(gridPosition) && grid[gridPosition.y, gridPosition.x] == null;
    }

    private void LockBlock(Block b)
    {
        var pos = ToGridPosition(b);
        b.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        grid[pos.y, pos.x] = b;

        if(b.GetBlockType() == BlockType.X || b.isBomb)
        {
            explodingKittens.Add(b);
        }
    }

    public void ProcessExplosions()
    {
        for(int i = explodingKittens.Count - 1; i >= 0; i--)
        {
            if(explodingKittens[i] == null)
            {
                explodingKittens.RemoveAt(i);
                continue;
            }

            Block b = explodingKittens[i];
            var pos = ToGridPosition(b);

            if(b.GetBlockType() == BlockType.X)
            {
                var adjacents = GetAdjacentBlocks(pos, BlockType.Empty);

                if(adjacents.Count > 0)
                {
                    explodingKittens.RemoveAt(i);

                    BoomColor(adjacents[0].GetBlockType(), pos);
                    DestroyBlock(b);

                    RearrangeGrid();
                }
                else
                {
                    b.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                    grid[pos.y, pos.x] = b;
                }
            }
            else
            {
                List<Block> bombing = Boom(b);
                
                if(bombing.Count > 0)
                {
                    explodingKittens.RemoveAt(i);

                    foreach(var gb in bombing)
                    {
                        DestroyBlock(gb);
                    }
                    DestroyBlock(b);

                    RearrangeGrid();
                }
            }
        }
    }

    public List<Block> Boom(Block bombBlock)
    {
        List<Block> boomBlocks = new List<Block>();

        bool[,] checkedPositions = new bool[sizeY, sizeX];
        Queue<Block> checkingBlocks = new Queue<Block>();
        checkingBlocks.Enqueue(bombBlock);

        var pos = ToGridPosition(bombBlock);
        checkedPositions[pos.y, pos.x] = true;

        while(checkingBlocks.Count > 0)
        {
            Block b = checkingBlocks.Dequeue();
            List<Block> adjacent = GetAdjacentBlocks(ToGridPosition(b), bombBlock.GetBlockType());

            foreach(var ab in adjacent)
            {
                Vector2Int abPos = ToGridPosition(ab);

                if(!checkedPositions[abPos.y, abPos.x])
                {
                    checkedPositions[abPos.y, abPos.x] = true;
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
                if(grid[y,x] != null && grid[y,x].GetBlockType() == type)
                {
                    DestroyBlock(grid[y,x]);
                }
            }
        }
    }

    private List<Block> GetAdjacentBlocks(Vector2Int gridPosition, BlockType type)
    {
        List<Block> adjacentBlocks = new List<Block>();

        foreach(var cp in checkPositions)
        {
            var adjacentPosition = gridPosition + cp;

            if(IsInGridRange(adjacentPosition))
            {
                var block = grid[adjacentPosition.y, adjacentPosition.x];

                if(block != null)
                {
                    if(type == BlockType.Empty || type == block.GetBlockType())
                    {
                        adjacentBlocks.Add(block);
                    }
                }
            }
        }
        
        return adjacentBlocks;
    }

    private void DestroyBlock(Block b)
    {
        var pos = ToGridPosition(b);
        var rb = grid[pos.y,pos.x];
        Destroy(b.gameObject);
        grid[pos.y,pos.x] = null;
    }

    private bool IsInGridRange(Vector2Int position)
    {
        return position.x >= 0 && position.x < sizeX && position.y >= 0 && position.y < sizeY;
    }

    public void RearrangeGrid()
    {
        for(int x = 0; x < grid.GetLength(1); x++)
        {
            int freeY = grid.GetLength(0);

            for(int y = 0; y < grid.GetLength(0); y++)
            {
                if(grid[y,x] == null)
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
                        grid[y,x] = null;
                        grid[freeY,x].transform.localPosition = new Vector3(x, freeY, 0);
                        freeY++;
                    }   
                }
            }
        }
    }

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
}
