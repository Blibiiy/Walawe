using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int round = 1;
    public TextMeshProUGUI roundText;
    public EnemySpawner EnemySpawner;
    public GameObject victoryPanel;
    public TextMeshProUGUI totalScoreText;

    private void Start()
    {
        if (roundText == null)
            return;
    }
    private void Update()
    {
        if(EnemySpawner != null)
        {
            if (EnemySpawner.spawnedEnemy < EnemySpawner.maxEnemies && !EnemySpawner.isSpawning)
                StartCoroutine(EnemySpawner.SpawnRoutine());

            if (PointManager.instance.enemyKilled == EnemySpawner.maxEnemies)
            {
                if (round < 5)
                    AddRound();
                else
                    youWin();

            }
        }
    }

    public void AddRound()
    {
        round++;
        if (round < 5)
            roundText.text = "Round " + round;
        else
            roundText.text = "Final Round";
        EnemySpawner.maxEnemies += 2;
        EnemySpawner.spawnedEnemy = 0;
        PointManager.instance.enemyKilled = 0;
    }

    public void youWin()
    {
        victoryPanel.SetActive(true);
        totalScoreText.text = "Total Score :  " + PointManager.instance.currentPoints;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
