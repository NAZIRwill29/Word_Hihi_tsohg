using UnityEngine;

// Moves the player one space and checks for walls
public class PlayerMover2D : PlayerMover
{
    public override bool IsValidMove(Vector3 movement)
    {
        // Check for obstacles using a 2D raycast
        return !Physics2D.Raycast(transform.position, movement, k_BoardSpacing, m_ObstacleLayer);
    }
}
