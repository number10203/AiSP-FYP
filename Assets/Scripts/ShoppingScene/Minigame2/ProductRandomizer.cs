using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductRandomizer : MonoBehaviour
{
    public void RandomizeProducts(GameObject[] productLists)
    {
        foreach (GameObject productList in productLists)
        {
            List<int> list = new List<int>(new int[3]);

            for (int i = 0; i < 3; ++i)
            {
                int randomInt = Random.Range(1, 4);
                while (list.Contains(randomInt))
                {
                    randomInt = Random.Range(1, 4);
                }

                list[i] = randomInt;
                productList.transform.GetChild(i).localPosition = new Vector3((list[i] - 2) * 500f, 120f);
            }
        }
    }
}
