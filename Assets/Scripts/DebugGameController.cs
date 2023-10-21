using UnityEngine;

public class DebugGameController : MonoBehaviour
{
    public GameGrid grid;
    public float timeScale = 0.25f;
    public bool gameRunning = true;


    void Start()
    {
        grid.Spawn();
    }

    void Update()
    {
        if(gameRunning)
        {
            gameRunning = grid.Tick(Time.deltaTime*timeScale);

            if(!gameRunning)
            {
                Debug.LogError("Game Over!");
            }
        }
    }

    public void Clean()
    {

    }
}