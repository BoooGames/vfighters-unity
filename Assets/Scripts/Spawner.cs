using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public BlockSpawnData spawnData;

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
                var blockData =  spawnData.GetRandomBlock();
                var blockInstance = Spawn(new Vector2Int(x + i,y), container, blockData.type, blockData.isBomb);
                blocks.Add(blockInstance.GetComponent<Block>());
            }
        }

        return(canSpawn, new BlockGroup(blocks, grid));
    }

    public Block Spawn(Vector2Int position, GameObject container,  int encodedData)
    {
        return Spawn(position, container, encodedData.DecodeCellType(), encodedData.DecodeIsBomb(), encodedData.DecodeTimer());
    }

    public Block Spawn(Vector2Int position, GameObject container,  BlockType b, bool isBomb, int timer = 0)
    {
        BlockData data = spawnData.GetDataFor(b, isBomb);
       
        var blockInstance = Instantiate(data.prefab, container.transform);
        blockInstance.Initialize(b, isBomb, timer);
        blockInstance.transform.localPosition = new Vector3(position.x,position.y,0);

        return blockInstance;
    }
}
