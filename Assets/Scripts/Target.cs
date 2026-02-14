using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 50f;
    public int headShotPoints = 100;
    public int KillPoints = 50;
    public void TakeDamage(DamageInfo info)
    {
        float finalDamage = info.BaseDamage;
        if(info.Hitzone == HitZone.Head)
        {
            finalDamage *= 2;
        }

        health -= finalDamage;

        Debug.Log("health : " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
