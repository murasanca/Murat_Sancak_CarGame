using UnityEngine;

// Car class that controls the movement and behavior of a car.
public class Car:MonoBehaviour
{
    // Transform based on relative upward direction.
    [Header("Transform Based on Relative Upward Direction"), SerializeField]
    private Transform relativeUp;

    // Speed of the car.
    [Space(8)]
    [SerializeField]
    private float speed = 32f;

    // Reference to the GameManager.
    private GameManager gameManager;

    /// <summary>
    /// r2D: Rigidbody2D component of the car.
    /// </summary>
    private Rigidbody2D r2D;

    // Local position and rotation of the car.
    private Vector3
        localEulerAngles,
        localPosition;


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Find and store a reference to the GameManager.
        gameManager=FindObjectOfType<GameManager>();

        // Get and store a reference to the Rigidbody2D component.
        r2D=GetComponent<Rigidbody2D>();

        // Store the initial local position and rotation of the car.
        localEulerAngles=transform.localEulerAngles;
        localPosition=transform.localPosition;
    }

    // FixedUpdate is called every fixed frame-rate frame.
    private void FixedUpdate() => r2D.MovePosition(r2D.position+(Vector2)(speed*Time.deltaTime*relativeUp.up));


    // OnBecameInvisible is called when the renderer is no longer visible by any camera.
    private void OnBecameInvisible()
    {
        // Restart the game and reset the car's transform.
        gameManager.Restart();
        ResetTransform();
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Finish"))
        {
            // If the car collides with a finish line, disable its script, hide the finish line, and move on to the next car or level.
            enabled=false;

            collision.transform.parent.gameObject.SetActive(false);
            gameManager.Next();
        }
        else // if(collision.transform.CompareTag("Obstacle"))
        {
            // If the car collides with an obstacle, restart the game and reset the car's transform.
            gameManager.Restart();
            ResetTransform();
        }
    }


    /// <summary>
    /// ResetTransform: Resets the car's local position and rotation to their initial values.
    /// </summary>
    public void ResetTransform()
    {
        transform.localEulerAngles=localEulerAngles;
        transform.localPosition=localPosition;
    }
}