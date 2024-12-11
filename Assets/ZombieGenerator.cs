using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnRadius = 7f;
    public int baseMaxZombies = 3; // Init
    public float spawnInterval = 1f;
    public float waveInterval = 30f; // 30ÃÊ

    private int maxZombies;
    private float spawnTimer = 0f;
    private float waveTimer = 0f;

    private void Start()
    {
        maxZombies = baseMaxZombies;
    }

    private void Update()
    {

        spawnTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;


        if (spawnTimer >= spawnInterval && maxZombies > 0)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }


        if (waveTimer >= waveInterval)
        {
            waveTimer = 0f;
            IncreaseWave();
        }
    }

    private void SpawnZombie()
    {
        if (maxZombies > 0)
        {
            Vector3 spawnPosition = new Vector3(
                transform.position.x + Random.Range(-spawnRadius, spawnRadius),
                transform.position.y,
                transform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            GameObject newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            ZombieController zombieController = newZombie.GetComponent<ZombieController>();
            if (zombieController != null)
            {
                zombieController.SetZombieGenerator(this);
            }

            maxZombies--;
        }
    }

    private void IncreaseWave()
    {
        GameManager.wave++;
        maxZombies = baseMaxZombies + (GameManager.wave - 1) * 1;
    }
}
