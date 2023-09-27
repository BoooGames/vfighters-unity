using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<BlockData> AvailableBlocks = new List<BlockData>();

    public BlockGroup Spawn(int x, int y, int ammount, GameObject container)
    {
        List<Block> blocks = new List<Block>();

        for(int i = 0; i < ammount; i++)
        {
            int block = Random.Range(0, AvailableBlocks.Count);
            var blockInstance = Instantiate(AvailableBlocks[block].prefab, container.transform);
            blockInstance.transform.localPosition = new Vector3(x + i,y,0);
            blocks.Add(blockInstance.GetComponent<Block>());
        }

        return new BlockGroup { blocks = blocks };
    }
}
