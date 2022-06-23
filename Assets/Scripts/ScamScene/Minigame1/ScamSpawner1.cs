using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScamSpawner1 : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject[] scammerPrefab;
    public RectTransform spawnPointOrigin;
    //private int NumberOfBushesToSpawn;
    private int Numberspawned;
    private int SelectSprite;
    public float starttime;
    private float timeinterval;
    private int spawnspeed;
    private float nexttime;
    private int wavecheck;

    public List<Transform> spawnPoints = new List<Transform>();

    public static ScamSpawner1 Instance
    {
        get; private set;
    }

    public void Start()
    {
        Instance = this;
        //starttime += Time.time;

        spawnspeed = 1;


        SetSpawnPoints(3, 4, 200, 100, 75, -75);

        StartCoroutine(SpawnWave(2));
        //StartCoroutine(SpawnScammer(6));

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

    //private void ResetSpawnPoint(Transform currentpos)
    //{
    //    spawnPoints.Add(currentpos);

    //}

    private IEnumerator SpawnWave(int numberOfWaves)
    {
        //TO DO:
        //move spawning of entity code here
        //make it based on numberofentities

        for (int i = 0; i < numberOfWaves; i++)
        {
            StartCoroutine(SpawnScammer(20 * spawnspeed));
            //waiting time for one wave
            yield return new WaitForSeconds(65);
            //checks what wave it is
            wavecheck += 1;
            //increase speed
            spawnspeed += 2;
        }

    }

    private IEnumerator SpawnScammer(int numberOfEntities)
    {

        if (spawnPoints.Count == 0)
            yield break;

        //if (NumberOfBushesToSpawn >= spawnPoints.Count)
        //{
        //    foreach (Transform point in spawnPoints)
        //    {
        //        GameObject scammerSpawned = Instantiate(scammerPrefab[0], point.position, Quaternion.identity);
        //        scammerSpawned.transform.parent = mainCanvas.transform;
        //        point.gameObject.SetActive(false);
        //        yield return new WaitForSeconds(timeinterval);

        //    }
        //}

        //NumberOfBushesToSpawn = 1;

        for (Numberspawned = 0; Numberspawned < numberOfEntities; Numberspawned++)
        {
            //Randomize sprite to spawn
            //SelectSprite = Random.Range(0, scammerPrefab.Length);
            timeinterval = Random.Range(1, 5);
            yield return new WaitForSeconds(timeinterval / spawnspeed);

            SelectSprite = 0;

            //Find a valid spawnpoint
            Transform pointGiven = FindValidPoint();

            if (pointGiven == null)
                continue;

            GameObject scammerSpawned = Instantiate(scammerPrefab[SelectSprite], pointGiven.position, Quaternion.identity);
            scammerSpawned.transform.parent = mainCanvas.transform;
            scammerSpawned.GetComponentInChildren<ScamEntity>().spawnPoint = pointGiven;
            scammerSpawned.transform.GetChild(0).GetComponent<RectTransform>().position = pointGiven.position;


            pointGiven.gameObject.SetActive(false);

        }



        yield return null;
    }

    public void DeleteEntity(GameObject canvasOfEntity)
    {
        StartCoroutine(DestroyScamEntity(canvasOfEntity));
    }

    private IEnumerator DestroyScamEntity(GameObject entity)
    {
        yield return null;

        Destroy(entity);
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

    #endregion
}
