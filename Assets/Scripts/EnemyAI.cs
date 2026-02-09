using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerPosition;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(playerPosition.position);
    }
}
