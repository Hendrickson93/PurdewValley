using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public VectorValue startingPosition;
    Vector2 movement;

    // Audio for grass-movement
    public AudioSource grassWalk;

    void Start() 
    {
        // If you exit a building, you will appear wherever the player's starting position is set to 
        transform.position = startingPosition.initialValue;
    }
    
    // Update is called once per frame
    void Update()
    {
        


        // Movement Audio
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // If character is walking on grass, output grass-walking sound
            grassWalk.enabled = true;
        }
        else
        {
            grassWalk.enabled = false;
        }
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        /*  The length of a movement vector which means our speed
            Square magnitude is the squared length of the vector */
        animator.SetFloat("Speed", movement.sqrMagnitude);

        
    }

    // For movement
    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
