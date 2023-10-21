using UnityEngine;

[CreateAssetMenu(menuName = "Fighters/BlockData")]
public class BlockData : ScriptableObject
{
    public BlockType type;
    public Block prefab;
    public int spawnRatio = 1;
}