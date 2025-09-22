using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerInput2D), typeof(CharacterAudioMulti), typeof(PlayerMovement2D))]
public class Player2D : Character
{
    //[SerializeField] private InputManager m_InputManager;

    [SerializeField]
    [Tooltip("LayerMask to identify obstacles in the game environment.")]
    LayerMask m_ObstacleLayer;

    // Components for handling different aspects of player functionality.
    PlayerInput2D m_PlayerInput2D;
    //PlayerMovement2D m_playerMovement2D;
    public List<SpriteWithName> SpriteEmotions;
    public SpriteRenderer PlayerCharSR;

    // Sets up component references.
    protected override void Initialize()
    {
        base.Initialize();
        // m_playerMovement2D = GetComponent<PlayerMovement2D>();
        m_PlayerInput2D = GetComponent<PlayerInput2D>();
    }

    protected void LateUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsAlive) return;
        // Get the input vector from the PlayerInput component.
        Vector2 inputVector = m_PlayerInput2D.InputVector;
        Vector2 move = m_PlayerInput2D.Move;

        // Move the player based on the input vector.
        if (CharacterMovement is PlayerMovement2D playerMovement2D)
            playerMovement2D.Move(inputVector, move);
    }
}