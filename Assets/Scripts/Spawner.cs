using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<BlockData> AvailableBlocks = new List<BlockData>();

    int randomCount = 0;

    void Awake()
    {
        randomCount = 0;

        foreach (var blockData in AvailableBlocks)
        {
            randomCount += blockData.spawnRatio;
        }
    }

    public (bool spawnSucceeded, BlockGroup group) Spawn(int x, int y, int ammount, GameObject container, GameGrid grid)
    {
        List<Block> blocks = new List<Block>();

        bool canSpawn = true;

        for(int i = 0; i < ammount; i++)
        {
            Vector2 pos = new Vector2(x + i,y);
            canSpawn = canSpawn && grid.CanMoveTo(pos);
        }
        
        if(canSpawn)
        {
            for(int i = 0; i < ammount; i++)
            {
                var block = GetRandomBlock();
                var blockInstance = Instantiate(block.prefab, container.transform);
                blockInstance.transform.localPosition = new Vector3(x + i,y,0);
                blocks.Add(blockInstance.GetComponent<Block>());
            }
        }

        return(canSpawn, new BlockGroup(blocks, grid));
    }

    private BlockData GetRandomBlock()
    {
        int rd = Random.Range(0, randomCount);
        int count = 0;

        foreach (var blockData in AvailableBlocks)
        {
            count += blockData.spawnRatio;

            if(rd < count)
            {
                return blockData;
            }
        }

        return AvailableBlocks[0];
    } 
}
