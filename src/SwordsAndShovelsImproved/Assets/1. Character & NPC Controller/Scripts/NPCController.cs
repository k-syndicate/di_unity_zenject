using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class NPCController : MonoBehaviour
{
    public float patrolTime = 10;
    public float aggroRange = 10;
    public Transform[] waypoints;
    public AttackDefinition attack;

    public Transform SpellHotSpot;

    int index;
    float speed, agentSpeed;
    Transform player;

    Animator animator;
    NavMeshAgent agent;

    private float timeOfLastAttack;

    private bool playerIsAlive;
    private MeleeEnemyFactory _meleeEnemyFactory;

    [Inject]
    private void Construct(HeroController heroController)
    {
        player = heroController.transform;
        player.gameObject.GetComponent<DestructedEvent>().IDied += PlayerDied;
    }
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed;
        index = Random.Range(0, waypoints.Length);

        InvokeRepeating("Tick", 0, 0.5f);

        if (waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
        }

        timeOfLastAttack = float.MinValue;
        playerIsAlive = true;
    }

    private void PlayerDied()
    {
        playerIsAlive = false;
    }

    void Update()
    {
        speed = Mathf.Lerp(speed, agent.velocity.magnitude, Time.deltaTime * 10);
        animator.SetFloat("Speed", speed);

        float timeSinceLastAttack = Time.time - timeOfLastAttack;
        bool attackOnCooldown = timeSinceLastAttack < attack.Cooldown;

        agent.isStopped = attackOnCooldown;

        if (playerIsAlive)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            bool attackInRange = distanceFromPlayer < attack.Range;

            if (!attackOnCooldown && attackInRange)
            {
                transform.LookAt(player.transform);
                timeOfLastAttack = Time.time;
                animator.SetTrigger("Attack");
            }
        }
    }

    public void Hit()
    {
        if (!playerIsAlive)
            return;

        if(attack is Weapon)
        {
            ((Weapon)attack).ExecuteAttack(gameObject, player.gameObject);
        }
        else if(attack is Spell)
        {
            ((Spell)attack).Cast(gameObject, SpellHotSpot.position, player.transform.position, LayerMask.NameToLayer("EnemySpells"));
        }
    }

    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1;
    }

    void Tick()
    {
        if (waypoints.Length > 0)
            agent.destination = waypoints[index].position;
        
        agent.speed = agentSpeed / 2;

        if (player != null && Vector3.Distance(transform.position, player.transform.position) < aggroRange)
        {
            agent.speed = agentSpeed;
            agent.destination = player.position;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
    
}