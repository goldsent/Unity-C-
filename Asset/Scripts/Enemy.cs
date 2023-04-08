using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int maxHP = 30;
    [SerializeField] private int currentHP;
    [SerializeField] private Player player;
    private int atk = 10;
    private float rotateSpeed = 7f;

    // Component
    private Animator m_Anim;
    private NavMeshAgent m_Agent;

    private Vector3 originalPos;
    private Quaternion originalRot;

    // Enemy
    private Transform m_Target;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
        originalPos = transform.position + new Vector3(0, 0.08f, 0);
        originalRot = Quaternion.Euler(transform.rotation.eulerAngles);
    }
    private void Update()
    {

        if (currentHP != maxHP)
        {

            Vector3 direction = (player.transform.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, direction, 2f))
            {
                transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotateSpeed);
                m_Anim.SetBool("IsAttack", true);
            }
            else if (Physics.Raycast(transform.position, direction, 5f) && Vector3.Distance(transform.position, originalPos) < 10f)
            {
                m_Agent.isStopped = false;
                m_Anim.SetBool("IsAttack", false);
                m_Anim.SetBool("IsWalk", true);
                m_Agent.SetDestination(player.transform.position);

            }
            else
            {
                if (Vector3.Distance(transform.position, originalPos) > 0.1f)
                {




                    // Set the agent's destination to the original position
                    m_Agent.SetDestination(originalPos);

                    // Play the walk animation
                    m_Anim.SetBool("IsWalk", true);
                }
                else
                {
                    // Stop the agent and play the idle animation
                    m_Agent.isStopped = true;
                    m_Anim.SetBool("IsWalk", false);
                    m_Anim.SetBool("IsAttack", false);
                    Debug.Log(transform.position);
                    Debug.Log(originalPos);
                }


            }
        }

        if (currentHP <= 1)
        {
            EnemyDie();
        }


    }

    public void HitedDamage(int amountDamage)
    {
        
        currentHP -= amountDamage;
        GameController gameController = FindObjectOfType<GameController>();
        gameController.enemyHealth -= atk;
        Debug.Log("Hp enemy : " + currentHP);
    }
    private void EnemyDie()
    {


        StartCoroutine(DeathAnim());

    }

    public void EnemyAttack()
    {
        player.HitedDamage(atk);
    }
    IEnumerator DeathAnim()
    {
        m_Anim.SetBool("IsAttack", false);
        m_Anim.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }
}