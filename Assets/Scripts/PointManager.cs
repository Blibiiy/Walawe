using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance { get; private set; }

    public int currentPoints = 500;

    public TextMeshProUGUI pointText;

    private void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;
        UpdatePointScore();
    }

    public void ReducePoints (int amount)
    {
        currentPoints -= amount; UpdatePointScore();
    }

    public void UpdatePointScore()
    {
        pointText.text = currentPoints.ToString();
    }
}
