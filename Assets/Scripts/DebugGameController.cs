using UnityEngine;

public class DebugGameController : MonoBehaviour
{
    public GameGrid grid;
    public float timeScale = 0.25f;

    void Start()
    {
        grid.Spawn();
    }

    void Update()
    {
        grid.Tick(Time.deltaTime*timeScale);
    }
}