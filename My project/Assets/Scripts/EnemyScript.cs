using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed = 4f;

    public enum MovementDirection { LeftRight, UpDown }
    public MovementDirection movementDirection = MovementDirection.LeftRight;

    private bool movePositive = true;

    void FixedUpdate()
    {
        switch (movementDirection)
        {
            case MovementDirection.LeftRight:
                MoveInDirection(Vector3.right);
                break;
            case MovementDirection.UpDown:
                MoveInDirection(Vector3.up);
                break;
            default:
                break;
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        Vector3 temp = transform.position;
        if (!movePositive)
        {
            direction *= -1f;
        }
        temp += direction * moveSpeed * Time.fixedDeltaTime;
        transform.position = temp;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Bounce")
        {
            movePositive = !movePositive;
        }
    }
}
