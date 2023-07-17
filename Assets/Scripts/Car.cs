using UnityEngine;

public class Car:MonoBehaviour
{
    [Header("Transform Based on Relative Upward Direction"),SerializeField]
    private Transform relativeUp;

    [Space(8),SerializeField]
    private float speed=8f;

    /// <summary>
    /// r2D: Rigidbody2D
    /// </summary>
    private Rigidbody2D r2D;


    private void Awake()
    {
        r2D=GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        r2D.MovePosition(r2D.position+(Vector2)(speed*Time.deltaTime*relativeUp.up));
    }


    private void OnBecameInvisible()
    {
        // TODO: Restart.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Restart.
    }
}