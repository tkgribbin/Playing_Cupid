using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //TODO
    //Randomize - 10 Random enemies are created, but then looped through in the same order. Randomize order?

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Queue<GameObject> enemyPool = new Queue<GameObject>();
    [SerializeField] private GameObject[] powerupPrefabs;
    [SerializeField] private Queue<GameObject> powerupPool = new Queue<GameObject>();
    [SerializeField] private Transform spawnParent;
    [SerializeField] private int poolStartSize = 10;

    private float spawnLimitYTop = 9;
    private float spawnLimitYBottom = 0.5f;
    private float spawnPosX = 15;
    

    private float enemyStartDelay = 1.0f;
    private float enemySpawnInterval = 2.0f;
    private float powerupStartDelay = 5.0f;
    private float powerupSpawnInterval = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", enemyStartDelay, enemySpawnInterval);
        
        for (int i = 0; i < poolStartSize; i++)
        {
            int random = Random.Range(0,enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[random]);
            enemy.SetActive(false);
            enemy.transform.parent = spawnParent;
            enemyPool.Enqueue(enemy);
        }

        InvokeRepeating("SpawnRandomPowerup", powerupStartDelay, powerupSpawnInterval);

        for (int i = 0; i < poolStartSize; i++)
        {
            int random = Random.Range(0, powerupPrefabs.Length);
            GameObject powerup = Instantiate(powerupPrefabs[random]);
            powerup.SetActive(false);
            powerup.transform.parent = spawnParent;
            powerupPool.Enqueue(powerup);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.DEAD)
        {
            CancelInvoke();
        }
    }
    // Spawn random at random x position at top of play area
    void SpawnRandomEnemy()
    {
        // Generate random index and random spawn position
        Vector3 spawnPos = new Vector3(spawnPosX, Random.Range(spawnLimitYTop, spawnLimitYBottom), -5);
        //int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        GameObject spawnedEnemy = GetEnemy();
        spawnedEnemy.transform.position = spawnPos;
    }

    public GameObject GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefabs[0]);
            enemy.transform.parent = spawnParent;
            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemyPool.Enqueue(enemy);
        enemy.SetActive(false);
    }


    void SpawnRandomPowerup()
    {
        // Generate random index and random spawn position
        Vector3 spawnPos = new Vector3(spawnPosX, Random.Range(spawnLimitYTop, spawnLimitYBottom), -5);
        //int powerupIndex = Random.Range(0, powerupPrefabs.Length);

        GameObject spawnedPowerup = GetPowerup();
        spawnedPowerup.transform.position = spawnPos;
    }

    public GameObject GetPowerup()
    {
        if (powerupPool.Count > 0)
        {
            GameObject powerup = powerupPool.Dequeue();
            powerup.gameObject.SetActive(true);
            return powerup;
        }
        else
        {
            GameObject powerup = Instantiate(powerupPrefabs[0]);
            powerup.transform.parent = spawnParent;
            return powerup;
        }
    }

    public void ReturnPowerup(GameObject powerup)
    {
        powerupPool.Enqueue(powerup);
        powerup.SetActive(false);
    }
}
