using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentityPlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public float speed;
    private bool changedDirection;
    private float timeUntilMovement;
    private float timePassed = 0f;
    public Vector3 direction
    {
        private set;
        get;
    }

    private void Start()
    {
        timeUntilMovement = 1 / speed;
        direction = new Vector3(1, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && direction.x == 0f && !changedDirection)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            changedDirection = true;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && direction.y == 0f && !changedDirection)
        {
            direction = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
            changedDirection = true;
        }

        if (timePassed <= timeUntilMovement)
        {
            timePassed += Time.deltaTime;
        }
        else
        {
            playerBody.MovePosition(new Vector2(Mathf.Round(playerBody.position.x), Mathf.Round(playerBody.position.y)));
            playerBody.velocity = direction * speed;
            //playerBody.MovePosition(transform.position += direction);
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.fixedDeltaTime);
            timePassed = 0f;
            changedDirection = false;
        }
    }

    public void SetPosition(Vector3 newPosition)
    {
        playerBody.MovePosition(newPosition);
        playerBody.SetRotation(0f);
        playerBody.velocity = direction * speed;
        timePassed = 0f;
    }
}
