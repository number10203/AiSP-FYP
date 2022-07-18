using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterSpawner : MonoBehaviour
{
    Tilemap floor;
    Vector3 topLeftCell;
    float rows, columns;

    // Start is called before the first frame update
    void Start()
    {
        GetRowsColumns();

        topLeftCell = new Vector3((int)(transform.position.x - ((columns - 1) * 0.5)), (int)(transform.position.y + ((rows - 1) * 0.5))); //offset y by one down
        SpawnCharacterWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetRowsColumns()
    {
        floor = GetComponent<Tilemap>();
        floor.CompressBounds();

        BoundsInt bounds = floor.cellBounds;
        columns = bounds.size.x;
        rows = bounds.size.y;
    }

    void SpawnCharacterWave()
    {
        GameObject gameobj = new GameObject();
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(topLeftCell.x, topLeftCell.x + columns - 1), Random.Range(topLeftCell.y, topLeftCell.y - rows + 1));
            Vector3Int cellPosition = floor.WorldToCell(randomPosition);
            Vector3 cellCenterPosition = floor.GetCellCenterWorld(cellPosition); 
            Instantiate(gameobj, cellCenterPosition, Quaternion.identity);
        }
    }
}
