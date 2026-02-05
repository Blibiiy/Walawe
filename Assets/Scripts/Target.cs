using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    private float health = 50f;
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("health : " + health);
        if (health <= 0)
            Destroy(gameObject);
    }
}
