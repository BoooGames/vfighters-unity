using Unity.VisualScripting;
using UnityEngine;

public class GridVisual : MonoBehaviour, IVisualGrid
{
    private Block[,] grid;

    [SerializeField]
    private Spawner spawner;

    private GameObject container;

    public void Initialize(int siezeX, int sizeY, GameObject container)
    {
        grid = new Block[sizeY, siezeX];
        this.container = container;
    }

    public void AddBlockAt(Vector2Int position, Block b)
    {
        grid[position.y, position.x] = b;        
        b.transform.localPosition = new Vector3(position.x, position.y, 0);
    }

    public void AddBlockAt(Vector2Int position, int encodedBlockData)
    {
        Block block = spawner.Spawn(position, container, encodedBlockData);
        grid[position.y, position.x] = block;        
    }

    public void RemoveBlockAt(Vector2Int position)
    {
        Block b = grid[position.y, position.x];
        Destroy(b.gameObject);
        grid[position.y, position.x] = null;
    }

    public void MoveBlock(Vector2Int origin, Vector2Int dest)
    {        
        Block b = grid[origin.y, origin.x];
        grid[origin.y, origin.x] = null;

        b.transform.localPosition = new Vector3(dest.x, dest.y, 0);

        grid[dest.y, dest.x] = b;
    }
}