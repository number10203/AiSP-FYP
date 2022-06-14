using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScamEntity : MonoBehaviour
{
    public int scoreWhenWhacked;

    public void OnClickkk()
    {
        Debug.Log("wham");
        Debug.Log(scoreWhenWhacked);
        ScamManager_1.Instance.score += scoreWhenWhacked;
        this.gameObject.SetActive(false);
    }
    //
}
