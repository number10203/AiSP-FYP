using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScamSpawner1 : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject[] scammerPrefab;
    public RectTransform spawnPointOrigin;
    public int NumberOfBushesToSpawn;
    public int timeinterval;

    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        SetSpawnPoints(3, 3, 200, 100, 75, -75);
        StartCoroutine(SpawnScammer());
    }

    private void SetSpawnPoints(int rows, int columns, int xInterval, int yInterval, int xOffSet = 0, int yOffSet = 0)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 newPosition = spawnPointOrigin.position + new Vector3(j * xInterval + xOffSet, i * -yInterval + yOffSet, 0);
                Transform spawnPoint = Instantiate(spawnPointOrigin, newPosition, Quaternion.identity);
                spawnPoints.Add(spawnPoint);
            }
        }
    }

    private IEnumerator SpawnScammer()
    {
        if (spawnPoints.Count == 0)
            yield break;

        if (NumberOfBushesToSpawn >= spawnPoints.Count)
        {
            foreach (Transform point in spawnPoints)
            {
                GameObject scammerSpawned = Instantiate(scammerPrefab[0], point.position, Quaternion.identity);
                scammerSpawned.transform.parent = mainCanvas.transform;
                point.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < NumberOfBushesToSpawn; i++)
        {
            //Find a valid spawnpoint
            Transform pointGiven = FindValidPoint();

            if (pointGiven == null)
                continue;

            GameObject scammerSpawned = Instantiate(scammerPrefab[0], pointGiven.position, Quaternion.identity);
            scammerSpawned.transform.parent = mainCanvas.transform;

            pointGiven.gameObject.SetActive(false);
        }

        yield return null;
    }

    #region Helper Functions

    private Transform FindValidPoint()
    {
        Transform validPoint = null;
        bool vacantSpawnPoints = true;

        while (validPoint == null && vacantSpawnPoints)
        {
            foreach (Transform point in spawnPoints)
            {
                if (point.gameObject.activeSelf)
                {
                    vacantSpawnPoints = true;
                    break;
                }

                vacantSpawnPoints = false;
            }
            int indexOfSpawnPointToCheck = Random.Range(0, spawnPoints.Count);
            if (spawnPoints[indexOfSpawnPointToCheck].gameObject.activeSelf)
                validPoint = spawnPoints[indexOfSpawnPointToCheck].transform;
        }
        return validPoint;
    }

    private void CreateObject(GameObject prefabObject, Vector3 position)
    {
        GameObject scammer = Instantiate(scammerPrefab[0], position, Quaternion.identity);
    }
    #endregion
}
