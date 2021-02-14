using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D col;

    private BowManager bowManager;
    private GameManager gameManager;
    bool isFlying = false;
    float angle;

    public Vector3 pos { get { return transform.position; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        bowManager = FindObjectOfType<BowManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
        isFlying = true;
    }

    public void DeactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        isFlying = false;
    }

    public void SetFlyAngle()
    {
        angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            bowManager.ReturnArrow(this);
        }
        if (isFlying)
        {
            SetFlyAngle();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Play Partical effect/sound
            bowManager.PlayParticle(this.transform);
            bowManager.PlaySound("Pop");
            gameManager.AddScore(100);
            //Remove Enemy and Arrow
            bowManager.ReturnArrow(this);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            //Play Partical effect/sound
            //TODO Change effect
            bowManager.PlayParticle(this.transform);
            bowManager.PlaySound("Ping");
            gameManager.GainLife();

            //Remove powerup and Arrow
            bowManager.ReturnArrow(this);
            collision.gameObject.SetActive(false);
        }
    }
}
