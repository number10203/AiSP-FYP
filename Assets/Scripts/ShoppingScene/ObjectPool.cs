using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Public variables
    public static ObjectPool SHARED_INSTANCE;

    // Private variables
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int amountToPool;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        SHARED_INSTANCE = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < amountToPool; ++i)
        {
            temp = Instantiate(prefab, parent.transform);
            temp.SetActive(false);

            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; ++i)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    public int GetObjectsOnScreen(int productID)
    {
        int noOfObjs = 0;

        for (int i = 0; i < pooledObjects.Count; ++i)
        {
            if (pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<ProductStats>().productID == productID)
            {
                ++noOfObjs;
            }
        }

        return noOfObjs;
    }

    public void SetAllToInactive()
    {
        foreach (GameObject gameObject in pooledObjects)
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
