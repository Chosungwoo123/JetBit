using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnTime;
    [SerializeField] private List<EnemyData> enemyDatas;

    [System.Serializable]
    public struct EnemyData
    {
        public GameObject enemyPrefab;
        public int weight;
    }

    private float timer;

    private int total;

    private void Start()
    {
        for (int i = 0; i < enemyDatas.Count; i++)
        {
            total += enemyDatas[i].weight;
        }
    }

    private void Update()
    {
        if (timer >= spawnTime)
        {
            SpawnEnemy();

            timer = 0;
        }

        timer += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        int weight = 0;
        int selectNum;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < enemyDatas.Count; i++)
        {
            weight += enemyDatas[i].weight;

            if (selectNum <= weight)
            {
                Instantiate(enemyDatas[i].enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            }
        }
    }
}