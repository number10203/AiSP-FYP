using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner instance;
    public GameObject characterPrefab;
    public GameObject player;
    public List<Sprite> lowercaseSprites = new List<Sprite>();
    public List<Sprite> uppercaseSprites = new List<Sprite>();
    public List<Sprite> numeralSprites = new List<Sprite>();
    public List<Sprite> symbolSprites = new List<Sprite>();
    public Tilemap floor;
    public List<GameObject> currentWaveCharacters = new List<GameObject>();
    public bool numeralSymbolEnabled = false;
    private Vector3 topLeftCell;
    private float rows, columns;
    private void Start()
    {
        instance = this;
        floor = GetComponent<Tilemap>();
        floor.CompressBounds();
        Minigame1EventHandler.instance.onEatCharacter += CharacterCollectTriggered;

        //GetRowsColumns();
        //topLeftCell = new Vector3((int)(transform.position.x - ((columns - 1) * 0.5)), (int)(transform.position.y + ((rows - 1) * 0.5))); //offset by one due to grid
        SpawnCharacterWave();
    }

    private void GetRowsColumns()
    {
        floor = GetComponent<Tilemap>();
        floor.CompressBounds();

        BoundsInt bounds = floor.cellBounds;
        columns = bounds.size.x;
        rows = bounds.size.y;
    }

    private void SpawnCharacterWave()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject character = Instantiate(characterPrefab, transform.position, Quaternion.identity);
            character.GetComponent<CollectibleHandler>().spawnerReference = instance;
            currentWaveCharacters.Add(character);
        }
        //for (int i = 0; i < 3; i++)
        //{
        //    Vector3 randomPosition = GeneratePosition();
        //    if (randomPosition.x == 0)
        //    {
        //        Debug.Log("Position Generation failed!");
        //        break;
        //    }
        //
        //    Vector3Int cellPosition = floor.WorldToCell(randomPosition);
        //    Vector3 cellCenterPosition = floor.GetCellCenterWorld(cellPosition);
        //    GameObject character = Instantiate(characterPrefab, cellCenterPosition, Quaternion.identity);
        //    currentWaveCharacters.Add(character);
        //    int random = Random.Range(0, 3);
        //    switch (random)
        //    {
        //        case 0:
        //            character.GetComponent<SpriteRenderer>().sprite = lowercaseSprites[Random.Range(0, lowercaseSprites.Count - 1)];
        //            character.GetComponent<CollectibleHandler>().type = CollectibleHandler.CharacterType.LOWERCASE;
        //            break;
        //        case 1:
        //            character.GetComponent<SpriteRenderer>().sprite = uppercaseSprites[Random.Range(0, uppercaseSprites.Count - 1)];
        //            character.GetComponent<CollectibleHandler>().type = CollectibleHandler.CharacterType.UPPERCASE;
        //            break;
        //        case 2:
        //            character.GetComponent<SpriteRenderer>().sprite = symbolSprites[Random.Range(0, symbolSprites.Count - 1)];
        //            character.GetComponent<CollectibleHandler>().type = CollectibleHandler.CharacterType.SYMBOL;
        //            break;
        //    }
        //}
    }

    private Vector3 GeneratePosition()
    {
        // Code related on what rules must be obeyed when generating positions for characters
        bool validPositionFound = false;
        for (int i = 0; i < 90; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(topLeftCell.x, topLeftCell.x + columns - 1), Random.Range(topLeftCell.y, topLeftCell.y - rows + 1));
            foreach(GameObject character in currentWaveCharacters)
            {
                //Check if they meet minimum distance requirements
                if (Mathf.Abs((character.transform.position - randomPosition).magnitude) < 4f)
                {
                    validPositionFound = false;
                    break;
                }

                if (Mathf.Abs((player.transform.position - randomPosition).magnitude) < 4f)
                {
                    validPositionFound = false;
                    break;
                }

                validPositionFound = true;
            }

            // Return position
            if (validPositionFound || currentWaveCharacters.Count == 0)
                return randomPosition;
        }

        Debug.Log("Exceeded maximum tries");
        return new Vector3 (0, 0);
    }

    public void CharacterCollectTriggered()
    {
        Debug.Log("Clearing wave");
        if (player.GetComponent<IdentityPlayerController>().characterList.Count >= 10)
            numeralSymbolEnabled = true;

        for (int i = 0; i < currentWaveCharacters.Count; i++)
        {
            if (currentWaveCharacters[i].GetComponent<CollectibleHandler>().playerController == null)
            {
                Destroy(currentWaveCharacters[i]);
                Debug.Log("Deleted untaken character");
            }
        }
        currentWaveCharacters.Clear();
        SpawnCharacterWave();
    }
}
