using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScamEntity : MonoBehaviour, IPointerDownHandler
{
    public Transform spawnPoint;
    private Canvas rend;
    private BoxCollider2D inputCollider;
    private Image spriteRenderer;

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
        inputCollider = this.GetComponent<BoxCollider2D>();
        spriteRenderer = this.GetComponent<Image>();

        currentEntity = imagesCollection[Random.Range(0, imagesCollection.Count)];
        spriteRenderer.sprite = currentEntity.entitySprite;
        RectTransform thisTransform = this.gameObject.GetComponent<RectTransform>();
        thisTransform.sizeDelta = new Vector2(currentEntity.entitySprite.rect.width * 0.02f, currentEntity.entitySprite.rect.height * 0.02f);
        inputCollider.size = new Vector2(thisTransform.rect.width, thisTransform.rect.height);
        rend = this.transform.parent.gameObject.GetComponent<Canvas>();
        //if (this.gameObject.transform.position.y >= 100)
        //{
        //    rend.overrideSorting = true;
        //    rend.sortingOrder -= 1;
        //    if (this.gameObject.transform.position.y >= 200)
        //    {
        //        rend.sortingOrder -= 1;
        //    }
        //}
        int spawnPointIndex = spawnPoint.GetSiblingIndex();
        if (spawnPointIndex == 0)
            spawnPointIndex = 1;
        float rowOfSpawnPoint = Mathf.Ceil((float) spawnPointIndex / (float) ScamSpawner1.Instance.columns);
        rend.overrideSorting = true;
        rend.sortingOrder = (int) rowOfSpawnPoint;

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
            ScamSpawner1.Instance.DeleteEntity(this.transform.parent.gameObject);
        }

    }

    public void OnPointerDown(PointerEventData p)
    {
        Debug.Log("I GOT WHACKED :(");
        //entityCanvasGroup.alpha = 0;
        if (currentEntity.score <= 0)
        {
            if (ScamManager_1.Instance.score != 0)
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
        ScamSpawner1.Instance.DeleteEntity(this.transform.parent.gameObject);
        //Destroy(this.gameObject);

        //currpos = this.gameObject.transform.position;
        //spawnPoint = Instantiate(ScamSpawner1.Instance.spawnPointOrigin, currpos, Quaternion.identity);
        //ScamSpawner1.Instance.spawnPoints.Add(spawnPoint);
    }

}
 