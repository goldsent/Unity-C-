using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{



    [SerializeField] public int maxHP = 20;

    public int currentHP;
    public int atk = 10;

    [SerializeField] private LayerMask m_Enemy;
    [SerializeField] private LayerMask m_Ground;

    // Component
    private Animator m_Anim;
    private NavMeshAgent m_Agent;
    private float speed;
    // Enemy
    private Transform m_Target;
    private Enemy enemyTarget;
    void Start()
    {

        currentHP = maxHP;
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {

        CheckStatePlayer();
        HeroAttack();
        HeroMove();
    }

    private void HeroMove()
    {   // Right Click on Ground
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_Ground))
            {
                m_Target = null;


                m_Agent.SetDestination(hit.point);
            }
        }
    }

    private void HeroAttack()
    {   // Left Click on Enemy
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_Enemy))
            {
                m_Target = hit.collider.transform;

                m_Agent.SetDestination(m_Target.position);

                m_Agent.stoppingDistance = 1f;
                enemyTarget = hit.collider.GetComponent<Enemy>();

            }

        }
    }

    private void CheckStatePlayer()
    {   //Check speed from Nav
        speed = m_Agent.velocity.magnitude / m_Agent.speed;

        if (speed > 0)
        {
            m_Anim.SetBool("IsWalk", true);
        }
        else if (m_Target != null && Vector3.Distance(transform.position, m_Target.position) <= 1.5f)
        {
            m_Anim.SetBool("IsWalk", false);
            if (Physics.Raycast(transform.position, m_Target.transform.position - transform.position, 2f, m_Enemy))
            {
                m_Anim.SetBool("IsAttack", true);


            }
            if (currentHP <= 1f)
            {
                StartCoroutine(DeathAnim());
            }
        }
        else
        {
            m_Anim.SetBool("IsAttack", false);
            m_Anim.SetBool("IsWalk", false);

        }
        if (currentHP <= 1f)
        {
            StartCoroutine(DeathAnim());
        }
    }
    public void HitedDamage(int amountDamage)
    {

        currentHP -= amountDamage;
        GameController gameController = FindObjectOfType<GameController>();
        gameController.playerHealth -= 10;
        Debug.Log("Hp Hero : " + currentHP);
    }


    IEnumerator DeathAnim()
    {
        m_Anim.SetBool("IsAttack", false);
        m_Anim.SetBool("IsWalk", false);
        m_Anim.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);


    }
    public void AutoAttack()
    {
        enemyTarget.HitedDamage(atk);
    }
}