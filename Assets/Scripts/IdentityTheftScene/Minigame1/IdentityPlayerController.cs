using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IdentityPlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public float speed;
    public Tilemap floorMap;
    public List<Vector4> turningHistory = new List<Vector4>();
    public List<GameObject> characterList = new List<GameObject>();
    public int DistToDetect = 20;
    private float timeUntilMovement;
    private float timePassed = 0f;
    private Vector3 startPos, endPos; 
    private Vector2 startTapPos;
    private bool isScreenPressed = false;
    private Vector3 prevDir;
    private Animator animator;
    public Vector3 direction
    {
        private set;
        get;
    }

    private void Start()
    {
        timeUntilMovement = 1 / speed;
        direction = new Vector3(1, 0);
        prevDir = new Vector3(1, 0);
        startPos = transform.position;
        endPos = startPos + direction;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckForInput();
        timePassed += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, endPos, timePassed / timeUntilMovement);

        if (timePassed >= timeUntilMovement)
        {
            timePassed = 0f;
            Vector4 newTurn = new Vector4(transform.position.x, transform.position.y);
            newTurn.w = Time.time;
            turningHistory.Add(newTurn);
            startPos = transform.position;
            endPos = startPos + direction;
            if (prevDir != direction)
            {
                UpdateAnimation();
            }
            prevDir = direction;
        }
    }

    void UpdateAnimation()
    {
        if (direction.x > 0)
            animator.SetTrigger("TurnRight");
        else if (direction.x < 0)
            animator.SetTrigger("TurnLeft");
        else if (direction.y > 0)
            animator.SetTrigger("TurnUp");
        else
            animator.SetTrigger("TurnDown");
    }

    void CheckForInput()
    {
        if (isScreenPressed == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startTapPos = Input.touches[0].position;
            isScreenPressed = true;
            Debug.Log("Swipe beginning!");
        }
        if (isScreenPressed && Input.GetMouseButtonUp(0))
        {
            isScreenPressed = false;
        }

        if (isScreenPressed)
        {
            if (Input.touches[0].position.y >= startTapPos.y + DistToDetect)
            {
                isScreenPressed = false;
                direction = new Vector3(0, 1, 0);
                Debug.Log("Attempted to swipe!");
            }
            else if (Input.touches[0].position.y <= startTapPos.y - DistToDetect)
            {
                isScreenPressed = false;
                direction = new Vector3(0, -1, 0);
                Debug.Log("Attempted to swipe!");
            }
            else if (Input.touches[0].position.x >= startTapPos.x + DistToDetect)
            {
                isScreenPressed = false;
                direction = new Vector3(1, 0, 0);
                Debug.Log("Attempted to swipe!");
            }
            else if (Input.touches[0].position.x <= startTapPos.x - DistToDetect)
            {
                isScreenPressed = false;
                direction = new Vector3(-1, 0, 0);
                Debug.Log("Attempted to swipe!");
            }
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && prevDir.x == 0f)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && prevDir.y == 0f)
        {
            direction = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        }
    }

    public void SetPosition(Vector3 newPosition)
    {
        Vector3Int cellPosition = floorMap.WorldToCell(newPosition);
        transform.position = floorMap.GetCellCenterWorld(cellPosition);
        playerBody.SetRotation(0f);
        playerBody.velocity = direction * speed;
        timePassed = 0f;
    }

    

}
