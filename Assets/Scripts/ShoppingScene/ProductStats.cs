using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductStats : MonoBehaviour
{
    // Public variables
    public int productID = -1;  // 0 - Phone, 1 - Scam Phone, 2 - Warranty Phone, 3 - Laptop, 4 - Scam Laptop, 5 - Warranty Laptop, 6 - Charger, 7 - Scam Charger, 8 - Warranty Charger

    public float speed = 3f;

    // Private variables
    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        transform.position -= Vector3.up * Time.deltaTime * speed;
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= -6f)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetStats()
    {
        productID = -1;
    }

    public void SetStats(int productID)
    {
        ResetStats();

        this.productID = productID;
    }

    public void RandomSpeed()
    {
        float randSpeed = Random.Range(3, 6);
        this.speed = randSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            player.CollectObject(this);
            gameObject.SetActive(false);
        }
    }
}
