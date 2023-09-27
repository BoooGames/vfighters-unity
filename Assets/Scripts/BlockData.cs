using UnityEngine;

[CreateAssetMenu(menuName = "Fighters/BlockData")]
public class BlockData : ScriptableObject
{
    public BlockType type;
    public Block prefab;
    public float spawnRatio = 1f;
}