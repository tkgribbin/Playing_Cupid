using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 30;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.CurrentGameState == GameManager.GameState.RUNNING)
        {
            transform.Translate(Vector3.left * Time.deltaTime * (speed + gameManager.GetAdditionalSpeed()));
        }
    }
}
