using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    // TODO: Level Design , Debuging log deletion
    public GameObject zombiePrefab;
    public float spawnRadius = 7f;
    public int maxZombies = 5;
    public float spawnInterval = 3f;
    public float upInterval = 10f;

    private float spawnTimer = 0f;
    private float upTimer = 0f;

    private void Update()
    {
        upTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && maxZombies > 0)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }

        if(upTimer > upInterval)
        {
            upTimer = 0f;
            maxZombies++;
        }


    }

    private void SpawnZombie()
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

    public void ZombieDied()
    {
        maxZombies++;
    }
}