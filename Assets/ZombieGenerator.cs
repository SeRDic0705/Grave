using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject zombiePrefab; 
    public float spawnRadius = 10f; // spawn ��ġ
    public int maxZombies = 5; // max 
    public float spawnInterval = 3f; // spawn ����(second)

    private float spawnTimer = 0f; // timer

    private void Update()
    {
        // ���� �ð� �������� ���͸� ����
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && maxZombies > 0)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        // spawn loc
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnRadius, spawnRadius),
            transform.position.y,
            transform.position.z + Random.Range(-spawnRadius, spawnRadius)
        );

        // prefab ����
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // max test
        maxZombies--;
    }

    public void ZombieDied()
    {
        maxZombies++;
    }
}
