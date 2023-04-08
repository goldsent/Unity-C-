using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");


    public int maxHP = 100;
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
    private Transform posTarget;
    void Start()
    {
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
                posTarget = m_Target;

            }

        }
    }

    private void CheckStatePlayer()
    {   //Check speed from Nav
        speed = m_Agent.velocity.magnitude / m_Agent.speed;
        
        if (speed > 0)
        {
            m_Anim.Play(Walk);
        }
        else if (m_Target != null && Vector3.Distance(transform.position, posTarget.position) <= 1.5f)
        {
            m_Anim.Play(Attack);
            StartCoroutine(PlayerAtk());
        }
        else
        {
            m_Anim.Play(Idle);
        }
    }

    IEnumerator PlayerAtk()
    {
        yield return new WaitForSeconds(1.0f);
    }
}