using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    private float health = 50f;
    private int headShotPoints = 100;
    private int KillPoints = 50;
    public void TakeDamage(float amount, bool isHeadShot)
    {

        health -= amount;
        
        Debug.Log("health : " + health);
        if (health <= 0)
        {
            int point = isHeadShot ? headShotPoints : KillPoints;
            PointManager.instance.AddPoints(point);
            Destroy(gameObject);
        }
    }
}
