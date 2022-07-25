using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleHandler : MonoBehaviour
{
    public enum CharacterType
    {
        LOWERCASE,
        UPPERCASE,
        SYMBOL,
        COUNT
    }
    public CharacterType type;
    public CharacterSpawner spawnerReference;
    public IdentityPlayerController playerController;
    private bool collected = false;
    private float timeOffset = 0f;
    private int frameLengthCollided = 0;

    private void Start()
    {
        spawnerReference = CharacterSpawner.instance;
        SpawnThisCharacter();
    }

    private void Update()
    {
        if (playerController == null)
            return;

        Vector3 correctedPlayerPosition = new Vector3(playerController.transform.position.x, playerController.transform.position.y);
        transform.position = GetPosition(playerController.turningHistory, correctedPlayerPosition, timeOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected)
            return;

        playerController = collision.gameObject.GetComponent<IdentityPlayerController>();
        if (playerController == null)
        {
            SpawnThisCharacter();
            return;
        }

        if (playerController.characterList.Count > 0)
        {
            CollectibleHandler handler = playerController.characterList[playerController.characterList.Count - 1].GetComponent<CollectibleHandler>();
            transform.position = new Vector3(handler.transform.position.x, handler.transform.position.y);
            timeOffset = 1f / playerController.speed + 1f / playerController.speed * playerController.characterList.Count;

            Debug.Log("Not first character");
        }
        else
        {
            transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y);
            timeOffset = 1f / playerController.speed;
            Debug.Log("First character");
        }
        this.gameObject.name = "Assimilated";
        playerController.characterList.Add(this.gameObject);
        Minigame1EventHandler.instance.EatCharacterTrigger();
        collected = true;
        GameManager.INSTANCE.currentIdentityScore += 10;

        Debug.Log("Successfully added to snake " + playerController.characterList.Count);
    }
    Vector3 GetPosition(List<Vector4> allPoints, Vector3 currentHeadPos, float myTimeOffset)
    {
        if (allPoints == null || allPoints.Count == 0) return Vector3.zero;
        if (allPoints.Count == 1) return allPoints[0];

        Vector3 lastPoint;
        float timeLerpValue;
        float myTargetTime = Time.time - myTimeOffset;

        for (int p = 1; p < allPoints.Count; p++)
        {
            if (allPoints[p].w >= myTargetTime)
            {
                lastPoint = allPoints[p - 1];
                Vector3 nextPoint = allPoints[p];
                timeLerpValue = (myTargetTime - allPoints[p - 1].w) / (allPoints[p].w - allPoints[p - 1].w);
                return Vector3.Lerp(lastPoint, nextPoint, timeLerpValue);
            }
        }

        lastPoint = allPoints[allPoints.Count - 1];
        float lastTime = allPoints[allPoints.Count - 1].w;
        timeLerpValue = (myTargetTime - lastTime) / (Time.time - lastTime);
        return Vector3.Lerp(lastPoint, currentHeadPos, timeLerpValue);

    }
    private void SpawnThisCharacter()
    {
        Vector3 randomPosition = GeneratePosition();
        if (randomPosition.x == 0)
        {
            Debug.Log("Position Generation failed!");
            return;
        }

        Vector3Int cellPosition = spawnerReference.floor.WorldToCell(randomPosition);
        Vector3 cellCenterPosition = spawnerReference.floor.GetCellCenterWorld(cellPosition);
        transform.position = cellCenterPosition;
        StartCoroutine(SpawnAnimation());
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = spawnerReference.lowercaseSprites[Random.Range(0, spawnerReference.lowercaseSprites.Count - 1)];
                type = CharacterType.LOWERCASE;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = spawnerReference.uppercaseSprites[Random.Range(0, spawnerReference.uppercaseSprites.Count - 1)];
                type = CharacterType.UPPERCASE;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = spawnerReference.symbolSprites[Random.Range(0, spawnerReference.symbolSprites.Count - 1)];
                type = CharacterType.SYMBOL;
                break;
        }
    }
    private Vector3 GeneratePosition()
    {
        // Code related on what rules must be obeyed when generating positions for characters
        bool validPositionFound = false;
        BoundsInt bounds = spawnerReference.floor.cellBounds;
        int columns = bounds.size.x;
        int rows = bounds.size.y;
        Vector3Int topLeftCell = new Vector3Int((int)(spawnerReference.transform.position.x - ((columns - 1) * 0.5)), 
                                                (int)(spawnerReference.transform.position.y + ((rows - 1) * 0.5)), 0); //offset by one due to grid
        for (int i = 0; i < 90; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(topLeftCell.x, topLeftCell.x + columns), Random.Range(topLeftCell.y - rows + 1, topLeftCell.y + 1));
            foreach (GameObject character in spawnerReference.currentWaveCharacters)
            {
                //Check if they meet minimum distance requirements
                if (Mathf.Abs((character.transform.position - randomPosition).magnitude) < 4f)
                {
                    validPositionFound = false;
                    break;
                }

                if (Mathf.Abs((spawnerReference.player.transform.position - randomPosition).magnitude) < 4f)
                {
                    validPositionFound = false;
                    break;
                }

                validPositionFound = true;
            }

            // Return position
            if (validPositionFound || spawnerReference.currentWaveCharacters.Count == 0)
                return randomPosition;
        }

        Debug.Log("Exceeded maximum tries");
        return new Vector3(0, 0);
    }
    private IEnumerator SpawnAnimation()
    {
        float timePassed = 0.0f;

        while (timePassed < 0.5f)
        {
            timePassed += Time.deltaTime;
            float timeInScaling = timePassed / 0.5f;
            transform.localScale = new Vector3(Mathf.Lerp(0, 1, timeInScaling), Mathf.Lerp(0, 1, timeInScaling));
            transform.Rotate(new Vector3(0, 0, 1), 30f);
            yield return null;
        }
        transform.localScale = new Vector3(1, 1);
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collected)
            return;

        IdentityPlayerController playerController = collision.gameObject.GetComponent<IdentityPlayerController>();
        if (playerController == null)
            return;

        frameLengthCollided++;
        if (frameLengthCollided >= 10)
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
