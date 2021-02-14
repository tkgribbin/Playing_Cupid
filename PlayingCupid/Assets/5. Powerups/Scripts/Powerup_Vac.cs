using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Vac : MonoBehaviour
{
    public float speed = 30;

    GameManager gameManager;
    private bool spawned = false;
    private Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnDisable()
    {
        if (!spawned)
        {
            spawned = true;
            return;
        }

        //Powerup Effect
        //gameManager.gainLife();


        spawner.ReturnPowerup(this.gameObject);
    }
    private void Update()
    {
        if (gameManager.CurrentGameState == GameManager.GameState.RUNNING)
        {
            //Move Left
            transform.Translate(Vector3.left * Time.deltaTime * (speed + gameManager.GetAdditionalSpeed()));
        }

        //Spawner bug fix (Spawning 10 Enemies, and setting them disabled in spawner was buggy)
        //if (!spawned)
        //{
        //    spawned = true;
        //    this.gameObject.SetActive(false);
        //}


        //Remove when off screen
        if (gameObject.transform.position.x < -15)
        {
            this.gameObject.SetActive(false);
        }


    }
}
