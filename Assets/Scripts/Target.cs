using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 50f;
    public int normalKillPoints = 50;
    public int perHitPoints = 10;
    public int headshotkillPoints = 100;
    public bool isHeadshot;

    public void TakeDamage(DamageInfo info)
    {
        float finalDamage = info.BaseDamage;
        isHeadshot = false;
        if (info.Hitzone == HitZone.Head)
        {
            finalDamage *= 2;
            isHeadshot = true;
        }

        int finalPoints = isHeadshot ? perHitPoints * 2 : perHitPoints;

        health -= finalDamage;
        PointManager.instance.AddPoints(finalPoints);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsDeath()
    {
        if (health <= 0)
        {
            int finalPoints = isHeadshot ? headshotkillPoints : normalKillPoints;
            PointManager.instance.AddPoints(finalPoints);
            return true;
        }

        return false;
    }
}
