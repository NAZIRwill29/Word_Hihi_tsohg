using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInput2D : MonoBehaviour
{
    [SerializeField] private InputManagerSO m_InputManagerSO;
    private Vector2 m_InputVector, m_move;

    public Vector2 InputVector => m_InputVector;
    public Vector2 Move => m_move;

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        HandleInput(m_InputManagerSO.MoveAction);
    }

    private void HandleInput(InputAction inputAction)
    {
        if (inputAction == null) return; // Prevent NullReferenceException

        m_move = inputAction.ReadValue<Vector2>();

        if (m_move.sqrMagnitude > 0) // More optimized than checking each component separately
        {
            m_InputVector = m_move.normalized; // Direct assignment + Normalization
        }
        else
        {
            m_InputVector = Vector2.zero;
        }
    }
}
// private void HandleInput(InputAction inputAction)
// {
//     m_move = inputAction.ReadValue<Vector2>();

//     if (!Mathf.Approximately(m_move.x, 0.0f) || !Mathf.Approximately(m_move.y, 0.0f))
//     {
//         m_InputVector.Set(m_move.x, m_move.y);
//         /*
//         Normalize on moveDirection, which sets its length to 1 but keeps its direction the same.
//         For example, a Vector2 set to (3,0) stores a position one unit to the right of the center of the scene, 
//         but it also stores the direction right — if you trace an arrow from (0,0) to (3,0) you get an arrow 
//         pointing to the right. Normalizing this vector will set it to (1,0), still pointing to the right but 
//         with a length of 1. In general, you normalize vectors that store direction because length is not 
//         important.
//         Important: Do not normalize vectors storing positions. Normalizing changes the x and y values, 
//         so normalizing position vectors will change the position. 
//         */
//         m_InputVector.Normalize();
//     }
//     else
//         m_InputVector.Set(0, 0);
// }
