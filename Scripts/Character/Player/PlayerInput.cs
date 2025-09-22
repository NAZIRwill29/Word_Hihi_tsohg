using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] protected InputManagerSO m_InputManagerSO;
    private Vector3 m_InputVector;
    public Vector3 InputVector => m_InputVector;

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        HandleInput(m_InputManagerSO.MoveAction);
    }

    private void HandleInput(InputAction inputAction)
    {
        Vector2 move = inputAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            m_InputVector.Set(move.x, 0, move.y);
            /*
            Normalize on moveDirection, which sets its length to 1 but keeps its direction the same.
            For example, a Vector2 set to (3,0) stores a position one unit to the right of the center of the scene, 
            but it also stores the direction right — if you trace an arrow from (0,0) to (3,0) you get an arrow 
            pointing to the right. Normalizing this vector will set it to (1,0), still pointing to the right but 
            with a length of 1. In general, you normalize vectors that store direction because length is not 
            important.
            Important: Do not normalize vectors storing positions. Normalizing changes the x and y values, 
            so normalizing position vectors will change the position. 
            */
            m_InputVector.Normalize();
        }
    }
}
