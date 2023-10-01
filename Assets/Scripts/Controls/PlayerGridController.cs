using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.Interactions;

public class PlayerGridController : BoardController
{
    private SimpleControls m_Controls;
    
    public void Awake()
    {
        m_Controls = new SimpleControls();

        m_Controls.puzzle.Right.performed +=
            ctx =>
        {
            MoveRight();
        };

        m_Controls.puzzle.Left.performed +=
            ctx =>
        {
           MoveLeft();
        };

        m_Controls.puzzle.Down.performed +=
            ctx =>
        {
           MoveDown();
        };


        m_Controls.puzzle.RotateRight.performed +=
            ctx =>
        {
           RotateRight();
        };

        m_Controls.puzzle.RotateLeft.performed +=
            ctx =>
        {
           RotateLeft();
        };
    }

    public void OnEnable()
    {
        m_Controls.Enable();
    }

    public void OnDisable()
    {
        m_Controls.Disable();
    }
}
