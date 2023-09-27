using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameGrid grid;


    public virtual void Move(int x)
    {
        var bg =  grid.GetCurrentPiece();
    }

    public virtual void Rotate()
    {
        var bg =  grid.GetCurrentPiece();
    }
}
