using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScamSpawner1 : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject[] scammerPrefab;
    public RectTransform spawnPointOrigin;
    public RectTransform spawnPointReference;
    //private int NumberOfBushesToSpawn;
    private int Numberspawned;
    private int SelectSprite;
    public float starttime;
    private float timeinterval;
    private int spawnspeed;
    private float nexttime;
    private int wavecheck;

    [Header("How many rows and columns to generate spawnpoints in")]
    public int rows;
    public int columns;
    
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public bool occupied;
    }

    public static ScamSpawner1 Instance
    {
        get; private set;
    }

    public void Start()
    {
        Instance = this;
        spawnPointOrigin.gameObject.SetActive(true);
        //starttime += Time.time;

        spawnspeed = 3;

        Debug.Log("Height: " + Screen.currentResolution.height + ", Width: " + Screen.currentResolution.width);

        StartCoroutine(SpawnWave(2));
        //StartCoroutine(SpawnScammer(6));

    }

    private void SetSpawnPoints(int rows, int columns, int xInterval, int yInterval, int xOffSet = 0, int yOffSet = 0)
    {
        spawnPointReference.gameObject.SetActive(true);
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= columns; j++)
            {
                Vector3 newPosition = spawnPointReference.position + new Vector3(xInterval * j + xOffSet, -yInterval * i + yOffSet);
                //Vector3 newPosition = spawnPointOrigin.position + new Vector3(j * xInterval + xOffSet, i * -yInterval + yOffSet, 0);
                Transform spawnPoint = Instantiate(spawnPointReference, newPosition, Quaternion.identity);
                //spawnPoints.Add(spawnPoint);
                spawnPoint.parent = spawnPointOrigin.transform;
            }
        }
        spawnPointReference.gameObject.SetActive(false);
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
            yield return new WaitForSeconds(50);
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
            SpawnPoint pointGiven = FindValidPoint();

            if (pointGiven.point == null)
                continue;

            GameObject scammerSpawned = Instantiate(scammerPrefab[SelectSprite], pointGiven.point.position, Quaternion.identity);
            scammerSpawned.transform.parent = this.transform.parent.transform;
            scammerSpawned.GetComponentInChildren<ScamEntity>().spawnPoint = pointGiven;
            scammerSpawned.transform.GetChild(0).GetComponent<RectTransform>().position = pointGiven.point.position;

        }



        yield return null;
    }

    public void OnDisable()
    {
        spawnPointOrigin.gameObject.SetActive(false);
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

    private SpawnPoint FindValidPoint()
    {
        SpawnPoint validPoint = new SpawnPoint();
        bool vacantSpawnPoints = true;

        while (vacantSpawnPoints)
        {
            foreach (SpawnPoint point in spawnPoints)
            {
                if (!point.occupied)
                {
                    vacantSpawnPoints = true;
                    break;
                }

                vacantSpawnPoints = false;
            }
            int indexOfSpawnPointToCheck = Random.Range(0, spawnPoints.Count);
            if (!spawnPoints[indexOfSpawnPointToCheck].occupied)
            {
                spawnPoints[indexOfSpawnPointToCheck].occupied = true;
                return spawnPoints[indexOfSpawnPointToCheck];
            }
        }
        return validPoint;
    }

    #endregion
}
