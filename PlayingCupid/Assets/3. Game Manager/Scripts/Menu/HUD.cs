using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private GameManager gameManager;
    private int lives;
    [SerializeField] GameObject heart1, heart2, heart3;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLives(gameManager.GetLives());
    }

    void ActvateHeart(GameObject heart)
    {
        heart.GetComponent<Image>().color = Color.white;
    }
    void DeactivateHeart(GameObject heart)
    {
        heart.GetComponent<Image>().color = Color.gray;
    }
    void UpdateLives(int lives)
    {
        switch (lives)
        {
            case 0:
                DeactivateHeart(heart1);
                DeactivateHeart(heart2);
                DeactivateHeart(heart3);
                break;
            case 1:
                //1 heart visible
                ActvateHeart(heart1);
                DeactivateHeart(heart2);
                DeactivateHeart(heart3);
                break;
            case 2:
                //2 hearts visible
                ActvateHeart(heart1);
                ActvateHeart(heart2);
                DeactivateHeart(heart3);
                break;
            case 3:
                //3 hearts visible
                ActvateHeart(heart1);
                ActvateHeart(heart2);
                ActvateHeart(heart3);
                break;
            default:
                //0 hearts visible
                DeactivateHeart(heart1);
                DeactivateHeart(heart2);
                DeactivateHeart(heart3);

                break;
        }
    }
}
