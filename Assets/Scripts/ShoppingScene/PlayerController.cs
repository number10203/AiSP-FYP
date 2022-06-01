using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    public GameObject niceParticleEffect;

    public AudioClip objectiveSFX, correctSFX, wrongSFX;

    public GameObject phoneTick, laptopTick, chargerTick;

    [HideInInspector] public int phonesCaught = 0, laptopsCaught = 0, chargersCaught = 0;

    // Private variables
    //[SerializeField] private float movementSpeed = 5f;
    [SerializeField] private TextMeshProUGUI phoneObjective, laptopObjective, chargerObjective;
    [SerializeField] private Sprite neutral, happy, sad;
    [SerializeField] private SpriteRenderer currentSprite;

    private ShoppingSceneManager sceneManager;
    private AudioManager audioManager;
    private Rigidbody2D rb2d;
    private ScoreSpawner scoreSpawner;

    private bool facingRight = true;

    private Vector2 lastPos;
    private Vector2 trackVelocity;

    private void Start()
    {
        sceneManager = GameObject.Find("CurrentSceneManager").GetComponent<ShoppingSceneManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        rb2d = GetComponent<Rigidbody2D>();
        currentSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        scoreSpawner = GameObject.Find("CurrentSceneManager").GetComponent<ScoreSpawner>();

        phoneObjective.text = "0/3";
        laptopObjective.text = "0/3";
        chargerObjective.text = "0/3";
    }

    private void Update()
    {
        if (!sceneManager.gameStarted || sceneManager.gameEnded)
        {
            return;
        }

        trackVelocity = (rb2d.position - lastPos) * 50;
        lastPos = rb2d.position;

        //Debug.Log(Mathf.Abs(trackVelocity.x));

        /*
        if (manager.gameEnded == false)
        {
            //Time.timeScale = Mathf.Clamp(Mathf.Abs(trackVelocity.x), 0.25f, 1);
            Time.timeScale = Mathf.Clamp(Mathf.Abs(trackVelocity.x / 10), 0.5f, 1);
        }
        else
        {
            Time.timeScale = 1;
        }
        */

        PlayerMovement();
        CheckWin();
    }

    private void PlayerMovement()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = Camera.main.nearClipPlane;

            Vector3 mouseToPlayerDir = point - transform.position;

            // Flip player if moving other direction
            if (mouseToPlayerDir.x < -0.01f && !facingRight)
            {
                facingRight = true;

                Vector3 playerScale = transform.localScale;
                playerScale.x = -1;
                transform.localScale = playerScale;
            }
            else if (mouseToPlayerDir.x > 0.01f && facingRight)
            {
                facingRight = false;

                Vector3 playerScale = transform.localScale;
                playerScale.x = 1;
                transform.localScale = playerScale;
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(point.x, transform.position.y, 10), 0.2f);
            // Move player
            //transform.position = Vector3.Lerp(transform.position, new Vector3(point.x, transform.position.y, 10), movementSpeed * Time.deltaTime);
            // Clamp player position
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7f, 7f), transform.position.y, 10);
        }
    }

    public void CollectObject(ProductStats stats)
    {
        switch (stats.productID)
        {
            case 0: // Phone
                ++phonesCaught;
                phoneObjective.text = phonesCaught + "/3";
                StartCoroutine(BlinkColorGUI(phoneObjective, Color.green));
                sceneManager.score += 50;
                CreateParticleEffect();

                if (phonesCaught == 3)
                {
                    phoneTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                StartCoroutine(ChangeExpressions(1));
                scoreSpawner.SpawnScore(Color.green, "+50");

                break;

            case 1: // Broken Phone
                if (sceneManager.score != 0)
                {
                    sceneManager.score -= 50;
                }
                StartCoroutine(BlinkColorGUI(sceneManager.scoreText, Color.red));
                audioManager.Play(wrongSFX);

                StartCoroutine(ChangeExpressions(2));
                scoreSpawner.SpawnScore(Color.red, "-50");

                break;

            case 2: // Warranty Phone
                ++phonesCaught;
                phoneObjective.text = phonesCaught + "/3";
                StartCoroutine(BlinkColorGUI(phoneObjective, Color.green));
                sceneManager.score += 100;
                CreateParticleEffect();

                if (phonesCaught == 3)
                {
                    phoneTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                StartCoroutine(ChangeExpressions(1));
                scoreSpawner.SpawnScore(Color.green, "+100");

                break;

            case 3: // Laptop
                ++laptopsCaught;
                laptopObjective.text = laptopsCaught + "/3";
                StartCoroutine(BlinkColorGUI(laptopObjective, Color.green));
                sceneManager.score += 50;
                CreateParticleEffect();

                if (laptopsCaught == 3)
                {
                    laptopTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                StartCoroutine(ChangeExpressions(1));
                scoreSpawner.SpawnScore(Color.green, "+50");

                break;

            case 4: //Broken Laptop
                if (sceneManager.score != 0)
                {
                    sceneManager.score -= 50;
                }
                StartCoroutine(BlinkColorGUI(sceneManager.scoreText, Color.red));
                audioManager.Play(wrongSFX);

                StartCoroutine(ChangeExpressions(2));
                scoreSpawner.SpawnScore(Color.red, "-50");

                break;

            case 5: // Warranty Laptop
                ++laptopsCaught;
                laptopObjective.text = laptopsCaught + "/3";
                StartCoroutine(BlinkColorGUI(laptopObjective, Color.green));
                sceneManager.score += 100;
                CreateParticleEffect();

                if (laptopsCaught == 3)
                {
                    laptopTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                StartCoroutine(ChangeExpressions(1));
                scoreSpawner.SpawnScore(Color.green, "+100");

                break;

            case 6: // Charger
                ++chargersCaught;
                chargerObjective.text = chargersCaught + "/3";
                StartCoroutine(BlinkColorGUI(chargerObjective, Color.green));
                sceneManager.score += 50;
                CreateParticleEffect();

                if (chargersCaught == 3)
                {
                    chargerTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                StartCoroutine(ChangeExpressions(1));
                scoreSpawner.SpawnScore(Color.green, "+50");

                break;

            case 7: // Broken Charger
                if (sceneManager.score != 0)
                {
                    sceneManager.score -= 50;
                }
                StartCoroutine(BlinkColorGUI(sceneManager.scoreText, Color.red));
                audioManager.Play(wrongSFX);

                StartCoroutine(ChangeExpressions(2));
                scoreSpawner.SpawnScore(Color.red, "-50");

                break;

            case 8: // Warranty Charger
                ++chargersCaught;
                chargerObjective.text = chargersCaught + "/3";
                StartCoroutine(BlinkColorGUI(chargerObjective, Color.green));
                sceneManager.score += 100;
                CreateParticleEffect();

                if (chargersCaught == 3)
                {
                    chargerTick.SetActive(true);
                    audioManager.Play(objectiveSFX);
                }
                else
                {
                    audioManager.Play(correctSFX);
                }

                StartCoroutine(ChangeExpressions(1));
                scoreSpawner.SpawnScore(Color.green, "+100");

                break;
        }
    }

    private IEnumerator ChangeExpressions(int i)
    {
        switch (i)
        {
            case 1: // Happy
                currentSprite.sprite = happy;
                break;
            case 2: // Sad
                currentSprite.sprite = sad;
                break;
        }

        yield return new WaitForSecondsRealtime(0.6f);

        currentSprite.sprite = neutral;
    }

    public void CreateParticleEffect()
    {
        GameObject effect = Instantiate(niceParticleEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }

    private IEnumerator BlinkColorGUI(TextMeshProUGUI text, Color color)
    {
        text.color = color;
        yield return new WaitForSecondsRealtime(0.6f);
        text.color = Color.white;
    }

    private void CheckWin()
    {
        if (phonesCaught >= 3 && laptopsCaught >= 3 && chargersCaught >= 3)
        {
            sceneManager.WinGame();
            Time.timeScale = 1;
        }
    }
}
