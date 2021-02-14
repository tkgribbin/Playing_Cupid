using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 30;
    public int score = 100;

    private Vector2 moveTarget;

    AudioManager audioManager;
    GameManager gameManager;
    private bool spawned = false;
    private Spawner spawner;
    private Runner runner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
        runner = FindObjectOfType<Runner>();
        moveTarget = runner.target.gameObject.transform.position;
    }
    private void OnDisable()
    {
        if (!spawned)
        {
            spawned = true;
            return;
        }
        spawner.ReturnEnemy(this.gameObject);
    }
    private void Update()
    {
        if(gameManager.CurrentGameState == GameManager.GameState.RUNNING)
        {
            //Move Left
            //transform.Translate(Vector3.left * Time.deltaTime * speed);

            //Move towards runner
            float step = (speed + gameManager.GetAdditionalSpeed()) * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Lose Life
            audioManager.Play("Hit");
            gameManager.LossLife();
            gameObject.SetActive(false);
        }
    }
}