using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance { get; private set; }

    public int currentPoints = 500;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;
    }
}
