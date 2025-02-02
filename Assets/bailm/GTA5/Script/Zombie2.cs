using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie2 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    public float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Zombie Standing Var")]
    public float zombieSpeed;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    bool previouslyAttack;
    public Player2 player2;

    [Header("Zombie Animation")]
    public Animator anim;

    [Header("Zombie mood/states")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInvisionRadius;
    public bool playerInattackingRadius;

    private void Awake()
    {
        
        presentHealth = zombieHealth;
        zombieAgent = GetComponent<NavMeshAgent>();
        healthBar.GiveFullHealth(zombieHealth); 
        
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if (!playerInvisionRadius && !playerInattackingRadius) Idle();
        if (playerInvisionRadius && !playerInattackingRadius) Pursueplayer();
        if (playerInvisionRadius && playerInattackingRadius) AttackPlayer();
    } 

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
        anim.SetBool("Idle", true);
        anim.SetBool("Running", false);
    }
    private void Pursueplayer()
    {
        if(zombieAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
        }
    }
    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                PlayerScript2 playerBody = hitInfo.transform.GetComponent<PlayerScript2>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
                anim.SetBool("Attacking", true);
                anim.SetBool("Running", false);
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }
    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if (presentHealth <= 0)
        {
            anim.SetBool("Died", true);
            zombieDie();
        }
    }

    private void zombieDie()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;
        playerInattackingRadius = false;
        playerInvisionRadius = false;
        ObjectToHit.Destroy(gameObject, 5.0f);
        player2.playerMoney += 10;
    }
}
