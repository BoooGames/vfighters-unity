using UnityEngine;

public interface IVisualGrid
{
    public void AddBlockAt(Vector2Int position, Block b);
    public void AddBlockAt(Vector2Int position, int encodedBlockData);
    public void RemoveBlockAt(Vector2Int b);
    public void MoveBlock(Vector2Int origin, Vector2Int dest); 
    public void Initialize(int siezeX, int sizeY, GameObject container);
}