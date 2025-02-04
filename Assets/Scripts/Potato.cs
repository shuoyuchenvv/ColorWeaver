using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
    public Transform player;  // Player reference
    public float followSpeed = 3f;  // NPC follow speed
    public float pushForce = 5f;  // Push force when colliding with the player
    public LayerMask wallLayer;  // Layer to define the walls the NPC should avoid
    public float wallAvoidanceDistance = 2f;  // Distance to check for walls

    private Rigidbody npcRb;

    void Start()
    {
        npcRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Check if there's a wall in front of the NPC
        if (!Physics.Raycast(transform.position, direction, wallAvoidanceDistance, wallLayer))
        {
            // No wall detected, continue to move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
        else
        {
            // Wall detected, avoid by changing direction
            Vector3 avoidanceDirection = Vector3.Cross(direction, Vector3.up).normalized; // Change direction to avoid wall
            transform.position = Vector3.MoveTowards(transform.position, transform.position + avoidanceDirection, followSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply a push force to the player when NPC collides with them
            Vector3 pushDirection = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        }
    }
}
