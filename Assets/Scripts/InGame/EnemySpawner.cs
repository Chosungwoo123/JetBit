using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject[] enemies;

    private float timer;

    private void Update()
    {
        if (timer >= spawnTime)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

            timer = 0;
        }

        timer += Time.deltaTime;
    }
}