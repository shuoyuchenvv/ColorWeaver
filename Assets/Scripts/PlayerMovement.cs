using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement: MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;         // Movement speed
    public float gravity = -9.81f;   // Gravity force
    public float jumpHeight = 1.5f;  // Jump height

    private Vector3 velocity;        // To store player's vertical velocity
    private bool isGrounded;         // Check if the player is grounded

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        // If grounded and moving downwards, reset the velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Slight downward force to keep grounded
        }

        // Get input from WASD keys
        float x = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float z = Input.GetAxis("Vertical");   // W/S or Up/Down

        // Move player in direction relative to the camera
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jumping logic
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // Calculate jump velocity based on height
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
