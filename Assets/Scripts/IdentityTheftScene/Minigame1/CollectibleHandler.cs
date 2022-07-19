using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleHandler : MonoBehaviour
{
    public IdentityPlayerController playerController;
    private bool collected = false;
    private float timeOffset = 0f;
    private int frameLengthCollided = 0;

    private void Update()
    {
        if (playerController == null)
            return;

        Vector3 correctedPlayerPosition = new Vector3(playerController.transform.position.x, playerController.transform.position.y - 1);
        transform.position = GetPosition(playerController.turningHistory, correctedPlayerPosition, timeOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected)
        {
            return;
        }

        playerController = collision.gameObject.GetComponent<IdentityPlayerController>();
        Debug.Log(collision.gameObject.name);

        if (playerController.characterList.Count > 0)
        {
            CollectibleHandler handler = playerController.characterList[playerController.characterList.Count - 1].GetComponent<CollectibleHandler>();
            transform.position = new Vector3(handler.transform.position.x, handler.transform.position.y);
            timeOffset = 1f / playerController.speed + 1f / playerController.speed * playerController.characterList.Count;

            Debug.Log("Not first character");
        }
        else
        {
            transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y - 1);
            timeOffset = 1f / playerController.speed;
            Debug.Log("First character");
        }
        this.gameObject.name = "Assimilated";
        playerController.characterList.Add(this.gameObject);
        Minigame1EventHandler.instance.EatCharacterTrigger();
        collected = true;
        GameManager.INSTANCE.currentIdentityScore += 50;

        Debug.Log("Successfully added to snake " + playerController.characterList.Count);
    }
    Vector3 GetPosition(List<Vector4> allPoints, Vector3 currentHeadPos, float myTimeOffset)
    {
        if (allPoints == null || allPoints.Count == 0) return Vector3.zero;
        if (allPoints.Count == 1) return (Vector3)allPoints[0];

        Vector3 lastPoint;
        float timeLerpValue;
        float myTargetTime = Time.time - myTimeOffset;

        for (int p = 1; p < allPoints.Count; p++)
        {
            if (allPoints[p].w >= myTargetTime)
            {
                lastPoint = (Vector3)allPoints[p - 1];
                Vector3 nextPoint = (Vector3)allPoints[p];
                timeLerpValue = (myTargetTime - allPoints[p - 1].w) / (allPoints[p].w - allPoints[p - 1].w);
                return Vector3.Lerp(lastPoint, nextPoint, timeLerpValue);
            }
        }

        lastPoint = (Vector3)allPoints[allPoints.Count - 1];
        float lastTime = allPoints[allPoints.Count - 1].w;
        timeLerpValue = (myTargetTime - lastTime) / (Time.time - lastTime);
        return Vector3.Lerp(lastPoint, currentHeadPos, timeLerpValue);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collected)
            return;

        IdentityPlayerController playerController = collision.gameObject.GetComponent<IdentityPlayerController>();
        if (playerController == null)
            return;

        frameLengthCollided++;
        if (frameLengthCollided >= 15)
        {
            Minigame1EventHandler.instance.GameEndTrigger();
            Debug.Log("Collided with tail");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        frameLengthCollided = 0;
    }
}
