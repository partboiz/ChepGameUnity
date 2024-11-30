using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRange = 5f;
    private Vector3 startPosition;
    private List<GameObject> playersOnElevator = new List<GameObject>();

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = Mathf.PingPong(Time.time * moveSpeed, moveRange) - (moveRange / 2);
        Vector3 newPosition = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
        Vector3 movement = newPosition - transform.position;

        transform.position = newPosition;

        
        foreach (GameObject player in playersOnElevator)
        {
            if (player != null)
            {
                player.transform.position += movement;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playersOnElevator.Contains(collision.gameObject))
        {
            playersOnElevator.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playersOnElevator.Remove(collision.gameObject);
        }
    }
}