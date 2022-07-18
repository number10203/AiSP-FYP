using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoundaryCollisionManager : MonoBehaviour
{
    public IdentityPlayerController playerController;
    private Tilemap boundary;

    private void Start()
    {
        boundary = GetComponent<Tilemap>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected");
        Debug.Log("Contact Point: " + collision.GetContact(0).point.normalized);

        if (playerController.direction.x != 0)
        {
            playerController.SetPosition(new Vector3(Mathf.Round(playerController.transform.position.x - playerController.direction.x * 9f), playerController.transform.position.y));
            Debug.Log("Horizontal Wrap Triggered | New Position: " + Mathf.Round(playerController.transform.position.x - playerController.direction.x * 9f));
        }
        else
        {
            playerController.SetPosition(new Vector3(playerController.transform.position.x, Mathf.Round(playerController.transform.position.y - playerController.direction.y * 8f)));
            Debug.Log("Vertical Wrap Triggered");
        }
    }
}
