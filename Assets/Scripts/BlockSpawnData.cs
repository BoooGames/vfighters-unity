using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fighters/BlockSpawnData")]
public class BlockSpawnData : ScriptableObject
{
    private int randomCount = -1;
    private Dictionary<int, BlockData> blockTypeDict = new Dictionary<int, BlockData>();

    public List<BlockData> AvailableBlocks = new List<BlockData>();


    private void Initialize()
    {
        if(blockTypeDict.Count > 0)
        {
            return;
        }

        randomCount = 0;

        foreach (var blockData in AvailableBlocks)
        {
            randomCount += blockData.spawnRatio;
            int key = BlockDataHelper.Encode(blockData.type, blockData.isBomb, 0);
            if(!blockTypeDict.ContainsKey(key))
            {
                blockTypeDict.Add(key, blockData);
            }
            else
            {
                Debug.LogError("WOW WOW WOW!");
            }
        }
    }

    public BlockData GetRandomBlock()
    {
        Initialize();

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

    public BlockData GetDataFor(BlockType type, bool isBomb)
    {
        Initialize();
        int key = BlockDataHelper.Encode(type, isBomb, 0);

        return blockTypeDict[key];
    }
}