using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScamEntity : MonoBehaviour
{
    public Image displayImage;
    public Transform spawnPoint;

    [HideInInspector]
    private float timeUntilSpawn;
    [SerializeField]
    [Range(0, 100)]
    private int maximumTimeUntilSpawn;

    public enum ENTITY_TYPE
    {
        FRIENDLY,
        SCAMMER,
        UNKNOWN,
        COUNT
    }

    [System.Serializable]
    public struct EntityInformation
    {
        public ENTITY_TYPE type;
        public Sprite entitySprite;
        public int score;
        public float timealive;
        public float timedie;
    }

    [SerializeField]
    private List<EntityInformation> imagesCollection;
    private EntityInformation currentEntity;

    public static ScamEntity Instance
    {
        get; private set;
    }

    private void Start()
    {
        currentEntity = imagesCollection[Random.Range(0, imagesCollection.Count)];
        displayImage.sprite = currentEntity.entitySprite;
        RectTransform thisTransform = this.gameObject.GetComponent<RectTransform>();
        thisTransform.sizeDelta = new Vector2(currentEntity.entitySprite.rect.width * 0.01f, currentEntity.entitySprite.rect.height * 0.01f);
    }

    private void Update()
    {
        currentEntity.timealive += Time.deltaTime;
        if (currentEntity.timealive >= currentEntity.timedie)
        {
            if (currentEntity.score >= 0)
            {
                if (ScamManager_1.Instance.score != 0)
                {
                    ScamManager_1.Instance.score -= currentEntity.score / 2;
                }
            }
            else
            {
                ScamManager_1.Instance.score -= currentEntity.score;
            }
            this.gameObject.SetActive(false);
            spawnPoint.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }

    }

    public void OnClickkk()
    {


        //Debug.Log(scoreWhenWhacked);
        //entityCanvasGroup.alpha = 0;
        if (currentEntity.score <= 0)
        {
            if(ScamManager_1.Instance.score != 0)
            {
                ScamManager_1.Instance.score += currentEntity.score;
            }
        }
        else
        {
            ScamManager_1.Instance.score += currentEntity.score;
        }
        this.gameObject.SetActive(false);
        spawnPoint.gameObject.SetActive(true);
        Destroy(this.gameObject);

        //currpos = this.gameObject.transform.position;
        //spawnPoint = Instantiate(ScamSpawner1.Instance.spawnPointOrigin, currpos, Quaternion.identity);
        //ScamSpawner1.Instance.spawnPoints.Add(spawnPoint);
    }

}
 