using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardController : MonoBehaviour
{
    public GameGrid grid;

    public void MoveRight()
    {
        var bg =  grid.GetCurrentPiece();
        bg.MoveHorizontal(1);
    }

    public void MoveLeft()
    {
        var bg =  grid.GetCurrentPiece();
        bg.MoveHorizontal(-1);
    }

    public void MoveDown()
    {
        var bg =  grid.GetCurrentPiece();
        bg.SnapDown();
    }

    public void RotateRight()
    {
        var bg =  grid.GetCurrentPiece();
        bg.RotateRight();
    }

    public void RotateLeft()
    {
        var bg =  grid.GetCurrentPiece();
        bg.RotateLeft();
    }

    public virtual void Move(int x)
    {
        var bg =  grid.GetCurrentPiece();
    }
}
