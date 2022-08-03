using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoundaryCollisionManager : MonoBehaviour
{
    public IdentityPlayerController playerController;

    private void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: end game
        Debug.Log("Collision with border!");
        Minigame1EventHandler.instance.GameEndTrigger();
    }
}
