using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerPosition;

    [Header("Enemy Attack Variables")]
    public float attackRange;
    public float damage;
    public float timeBetweenAttacks;

    private float attackTimer = 0f;
    [SerializeField] private IDamageable playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = playerPosition.GetComponent<IDamageable>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
        if (attackTimer > -1f )
        {
            attackTimer -= Time.deltaTime;
        }
        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;


            if(attackTimer < 0f)
            {
                AttackPlayer();
                attackTimer = timeBetweenAttacks;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(playerPosition.position);
        }
    }

    void AttackPlayer()
    {
        if(playerHealth != null)
        {
            DamageInfo info = new DamageInfo(damage);
            playerHealth.TakeDamage(info);
        }
    }
}
