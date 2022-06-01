using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSpawner : MonoBehaviour
{
    // Private variables
    [SerializeField] private GameObject scorePrefab, parent;
    [SerializeField] private GameObject player;

    public void SpawnScore(Color color, string scoreStr)
    {
        GameObject score = Instantiate(scorePrefab, Camera.main.WorldToScreenPoint(player.transform.position), Quaternion.identity, parent.transform);
        score.GetComponent<TextMeshProUGUI>().color = color;
        score.GetComponent<TextMeshProUGUI>().text = scoreStr;
        LeanTween.moveLocalY(score, score.transform.localPosition.y + 20f, 2f).setEaseOutCubic();

        StartCoroutine(DestroyScore(score));
    }

    private IEnumerator DestroyScore(GameObject score)
    {
        yield return new WaitForSeconds(2f);

        Destroy(score);
    }
}
