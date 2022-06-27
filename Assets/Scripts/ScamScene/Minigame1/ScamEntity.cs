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
    private Animator entityAnimator;

    private bool wasHit = false;

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

    [System.Serializable]
    public struct OnHitSpriteTypes
    {
        public ENTITY_TYPE type;
        public List<Sprite> onHitSprites;
    }

    [SerializeField]
    private List<EntityInformation> imagesCollection;
    [SerializeField]
    private List<OnHitSpriteTypes> onHitSpritesCollection;
    private EntityInformation currentEntity;

    public static ScamEntity Instance
    {
        get; private set;
    }

    private void Start()
    {
        inputCollider = this.GetComponent<BoxCollider2D>();
        spriteRenderer = this.GetComponent<Image>();
        entityAnimator = this.GetComponent<Animator>();

        currentEntity = imagesCollection[Random.Range(0, imagesCollection.Count)];
        spriteRenderer.sprite = currentEntity.entitySprite;
        RectTransform thisTransform = this.gameObject.GetComponent<RectTransform>();
        thisTransform.sizeDelta = new Vector2(currentEntity.entitySprite.rect.width * 0.02f, currentEntity.entitySprite.rect.height * 0.02f);
        inputCollider.size = new Vector2(thisTransform.rect.width, thisTransform.rect.height);
        rend = this.transform.parent.gameObject.GetComponent<Canvas>();

        int spawnPointIndex = spawnPoint.GetSiblingIndex();
        if (spawnPointIndex == 0)
            spawnPointIndex++;
        float rowOfSpawnPoint = Mathf.Ceil((float) spawnPointIndex / (float) ScamSpawner1.Instance.columns);
        if (spawnPointIndex % ScamSpawner1.Instance.columns == 0)
            rowOfSpawnPoint++;
        rend.overrideSorting = true;
        rend.sortingOrder = (int) rowOfSpawnPoint;

    }

    private void Update()
    {
        if (!wasHit)
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
        wasHit = true;
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

        if (entityAnimator != null)
        {
            AssignSpritesForOnHit();
            entityAnimator.Play("Entity_OnHit");
        }
        else
        {
            DespawnEntity();
        }
    }

    public void AssignSpritesForOnHit()
    {
        Image[] images = this.GetComponentsInChildren<Image>(true);

        foreach (OnHitSpriteTypes onHitSprites in onHitSpritesCollection)
        {
            if (onHitSprites.type == currentEntity.type)
            {
                for (int i = 1; i - 1 < onHitSprites.onHitSprites.Count; i++)
                {
                    images[i].sprite = onHitSprites.onHitSprites[i - 1];
                }
                break;
            }
        }
    }

    public void DespawnEntity()
    {
        this.gameObject.SetActive(false);
        spawnPoint.gameObject.SetActive(true);
        ScamSpawner1.Instance.DeleteEntity(this.transform.parent.gameObject);
    }
}
 