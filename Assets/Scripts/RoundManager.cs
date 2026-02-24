using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int round = 1;
    public TextMeshProUGUI roundText;
    public EnemySpawner EnemySpawner;

    private void Start()
    {
        if (roundText == null)
            return;
    }
    private void Update()
    {
        if(EnemySpawner != null)
        {
            if (EnemySpawner.spawnedEnemy <= EnemySpawner.maxEnemies && !EnemySpawner.isSpawning)
                StartCoroutine(EnemySpawner.SpawnRoutine());

            if (EnemySpawner.spawnedEnemy == 0)
            {
                AddRound();
            }
        }
    }

    public void AddRound()
    {
        roundText.text = "Round " + round;
        EnemySpawner.maxEnemies++;
        EnemySpawner.spawnedEnemy = 0;
    }
}
