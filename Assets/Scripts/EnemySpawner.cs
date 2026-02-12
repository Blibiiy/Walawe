using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setting")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float spawnInterval = 3f;
    public int maxEnemies = 2;
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); 
    }
}
