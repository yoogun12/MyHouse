using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, RunAway }

    public EnemyState state = EnemyState.Idle;

    public int enemyHp = 5;          //적의 체력 
    private int currentEhp;
    public Slider hpSlider;

    public float moveSpeed = 2f;        //이동 속도
    public float traceRange = 15f;      //추적 시작 거리
    public float attackRange = 6f;      //공격 시작거리
    public float runawayRange = 20f;
    public float attackCooldown = 1.5f;

    public GameObject projectilePrefab;  //투사체 프리팹

    public Transform firePoint;         //발사 위치

    private Transform player;           //플레이어 추적용

    private float lastAttackTime;

  

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
        currentEhp = enemyHp;
        hpSlider.value = currentEhp;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // FSM 상태 전환
        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                break;

            case EnemyState.Trace:
                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                break;

            case EnemyState.Attack:
                if (dist > attackRange)
                    state = EnemyState.Trace;
                else
                    AttackPlayer();
                break;

            case EnemyState.RunAway:
                Run(); 
                if (dist > runawayRange)
                    state = EnemyState.Idle;
                break;




        }
    }

    public void TakeDamage(int damage)
    {
        currentEhp -= damage;
        hpSlider.value = (float)currentEhp / enemyHp;

        if (currentEhp <= 2)
            state = EnemyState.RunAway;  

        if (currentEhp <= 0)
            Die();
    }


    void Die()
    {
        Destroy(gameObject);
    }

    void TracePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    void Run()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * -1 *  Time.deltaTime ;
        transform.LookAt(player.position * -1);

    }

    void AttackPlayer()
    {
        // 일정 쿨다운마다 발사
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            transform.LookAt(player.position);
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            if (ep != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

}
