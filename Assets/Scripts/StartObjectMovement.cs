using UnityEngine;

public class StartObjectMovement : MonoBehaviour
{
    public Transform pointA; // First position
    public Transform pointB; // Second position
    public float speed = 1.0f; // Speed of movement
    public float rotationSpeed = 0.0001f;
    private bool movingToB = true; // Track the current target position

    void Rotation()
    {
        // Generate random rotation values
        float randomX = Random.Range(0f, 0.1f);
        float randomY = Random.Range(0f, 0.1f);
        float randomZ = Random.Range(0f, 0.1f);
        // Create a rotation vector
        Vector3 randomRotation = new Vector3(randomX, randomY, randomZ);

        // Apply the rotation to the object
        transform.Rotate(randomRotation * rotationSpeed * Time.deltaTime);
    }

    void Update()
    {
        Move();
        Rotation();
    }

    void Move()
    {
        // Determine the target position based on the current state
        Vector3 targetPosition = movingToB ? pointB.position : pointA.position;

        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the object has reached the target position
        if (transform.position == targetPosition)
        {
            // Switch target
            movingToB = !movingToB;
        }
    }
}

