using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnTimeMin;
    [SerializeField] private float spawnTimeMax;
    [SerializeField] private List<EnemyData> enemyDatas;

    private float spawnTime;

    [System.Serializable]
    public struct EnemyData
    {
        public GameObject enemyPrefab;
        public int weight;
    }

    private float timer;

    private int total;

    private List<int> enemyNumList = new List<int>();

    private void Start()
    {
        for (int i = 0; i < enemyDatas.Count; i++)
        {
            for (int j = 0; j < enemyDatas[i].weight; j++)
            {
                enemyNumList.Add(i);
            }
        }

        // 리스트 셔플
        int n = enemyNumList.Count;
        while (n > 1)
        {
            n--;
            int k = new System.Random().Next(n);
            var value = enemyNumList[k];
            enemyNumList[k] = enemyNumList[n];
            enemyNumList[n] = value;
        }

        spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
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
        int selectNum;

        selectNum = Random.Range(0, enemyNumList.Count);

        Instantiate(enemyDatas[enemyNumList[selectNum]].enemyPrefab, 
                    spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
    }
}