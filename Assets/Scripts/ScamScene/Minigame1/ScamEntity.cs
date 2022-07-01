using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScamEntity : MonoBehaviour, IPointerDownHandler
{
    public ScamSpawner1.SpawnPoint spawnPoint;
    private Canvas rend;
    private BoxCollider2D inputCollider;
    private Image spriteRenderer;
    private Animator entityAnimator;
    private AudioManager audioManager;
    public AudioClip Bonk;
    public AudioClip Heartbreak;
    public AudioClip Combo_Hit;

    private bool wasHit = false;

    [SerializeField]
    private Sprite plusText, minusText;

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
        public string name;
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

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        currentEntity = imagesCollection[Random.Range(0, imagesCollection.Count)];
        spriteRenderer.sprite = currentEntity.entitySprite;
        RectTransform thisTransform = this.gameObject.GetComponent<RectTransform>();
        thisTransform.sizeDelta = new Vector2(currentEntity.entitySprite.rect.width * 0.02f, currentEntity.entitySprite.rect.height * 0.02f);
        inputCollider.size = new Vector2(thisTransform.rect.width, thisTransform.rect.height);
        rend = this.transform.parent.gameObject.GetComponent<Canvas>();

        int spawnPointIndex = spawnPoint.point.GetSiblingIndex();
        if (spawnPointIndex == 0)
            spawnPointIndex++;
        float rowOfSpawnPoint = Mathf.Ceil((float) spawnPointIndex / (float) ScamSpawner1.Instance.columns);
        if (spawnPointIndex % ScamSpawner1.Instance.columns == 0)
            rowOfSpawnPoint++;
        rend.overrideSorting = true;
        rend.sortingOrder = (int) rowOfSpawnPoint;

        this.name = currentEntity.entitySprite.name;
        entityAnimator.Play("Entity_Spawn");
    }

    private void Update()
    {
        if (wasHit)
            return;
        
        currentEntity.timealive += Time.deltaTime;

        if (currentEntity.timealive >= currentEntity.timedie)
        {
            if (currentEntity.score >= 0)
            {
                this.GetComponentsInChildren<Image>(true)[4].sprite = minusText;
                this.GetComponentsInChildren<Animator>(true)[1].SetTrigger("scoreTrigger");
                if (ScamManager_1.Instance.score != 0)
                {
                    ScamManager_1.Instance.score -= currentEntity.score;
                }
            }
            else
            {
                this.GetComponentsInChildren<Image>(true)[4].sprite = plusText;
                this.GetComponentsInChildren<Animator>(true)[1].SetTrigger("scoreTrigger");
                ScamManager_1.Instance.score -= currentEntity.score;
            }

            this.GetComponentsInChildren<Image>(true)[3].sprite = currentEntity.entitySprite;
            entityAnimator.SetTrigger("DespawnEntity");
            wasHit = true;
        }

    }

    public void OnPointerDown(PointerEventData p)
    {
        if (wasHit)
            return;

        wasHit = true;
        audioManager.Play(Bonk);
        if (currentEntity.score <= 0)
        {
            this.GetComponentsInChildren<Image>(true)[4].sprite = minusText;
            this.GetComponentsInChildren<Animator>(true)[1].SetTrigger("scoreTrigger");
            if (ScamManager_1.Instance.score != 0)
            {
                ScamManager_1.Instance.score += currentEntity.score;

            }

            StartCoroutine(AnimateBonkWrong());
        }
        else
        {
            this.GetComponentsInChildren<Image>(true)[4].sprite = plusText;
            this.GetComponentsInChildren<Animator>(true)[1].SetTrigger("scoreTrigger");
            ScamManager_1.Instance.score += currentEntity.score;
            StartCoroutine(AnimateBonkRight());
            audioManager.Play(Combo_Hit);
        }

        if (entityAnimator != null)
        {
            StartCoroutine(AnimateOnHit());
        }
        else
        {
            DespawnEntity();
        }
    }

    IEnumerator AnimateBonkRight()
    {
        RectTransform thisTransformBonk = this.transform.GetChild(4).GetComponent<RectTransform>();
        thisTransformBonk.sizeDelta = new Vector2(currentEntity.entitySprite.rect.width * 0.02f, currentEntity.entitySprite.rect.height * 0.02f);
        this.transform.GetChild(4).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        this.transform.GetChild(4).gameObject.SetActive(false);
    }

    IEnumerator AnimateBonkWrong()
    {
        //RectTransform thisTransformBonk = this.transform.GetChild(5).GetComponent<RectTransform>();
        //thisTransformBonk.sizeDelta = new Vector2(currentEntity.entitySprite.rect.width * 0.02f, currentEntity.entitySprite.rect.height * 0.02f);
        audioManager.Play(Heartbreak);
        this.transform.GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        this.transform.GetChild(5).gameObject.SetActive(false);
    }

    IEnumerator AnimateOnHit()
    {
        AssignSpritesForOnHit();

        yield return null;
        entityAnimator.SetTrigger("HitEntity");

        yield return new WaitForSeconds(1f);
        entityAnimator.SetTrigger("DespawnEntity");
    }

    public void AssignSpritesForOnHit()
    {
        Image[] images = this.GetComponentsInChildren<Image>(true);

        foreach (OnHitSpriteTypes onHitSprites in onHitSpritesCollection)
        {
            if (onHitSprites.type == currentEntity.type)
            {
                if (onHitSprites.name != this.name && onHitSprites.type == ENTITY_TYPE.FRIENDLY)
                    continue;

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
        spawnPoint.occupied = false;
        ScamSpawner1.Instance.DeleteEntity(this.transform.parent.gameObject);
    }
}
 