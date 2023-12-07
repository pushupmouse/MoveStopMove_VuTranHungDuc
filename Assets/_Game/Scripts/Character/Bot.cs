using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Bot : Character
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float walkPointRange;
    [SerializeField] private NavMeshAgent agent;

    private Vector3 walkPoint;
    private IState currentState;
    private bool walkPointSet = false;
    private float maxDistance = 2f;
    private float minRange = -15f, maxRange = 15f;
    private float spawnDelay = 2f;
    private float distanceToPoint = 1f;
    public GameObject targetIndicator;


    private void Awake()
    {
        _transform = transform;
        Level = 0;
    }

    private void Start()
    {
        Unsubscribe();
        Subscribe();
        GetRandomWeapon();    
    }

    protected override void OnInit()
    {
        base.OnInit();
        SetAttributes((WeaponType)GameManager.Instance.UserData.equippedWeapon);
    }

    private void GetRandomWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        int randomType = Random.Range(0, 5);

        currentWeapon = Instantiate(weaponSO.GetWeapon((WeaponType)randomType), holdWeapon.transform).GetComponent<Weapon>();
        SetAttributes((WeaponType)randomType);
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        if (target != null)
        {
            if (!target.activeSelf || Vector3.Distance(_transform.position, target.transform.position) > attackRange)
            {
                target = null;
            }
        }

        if (target == null)
        {
            FindTarget();
        }
        
        if (target == null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new AttackState());
        }
    }

    internal void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            if (gameObject.activeSelf)
            {
                agent.SetDestination(walkPoint);
            }
            Moving();
        }

        if((_transform.position - walkPoint).magnitude < distanceToPoint)
        {
            walkPointSet = false;
        }
    }


    internal void Attacking()
    {
        agent.SetDestination(_transform.position);
        ChangeAnimation(MyConst.Animation.IDLE);
        PrepareAttack();

        if(target != null)
        {
            _transform.LookAt(target.transform);
        }
    }

    public override void OnHit()
    {
        Invoke(nameof(Respawn), spawnDelay);
        base.OnHit();
    }

    private void Respawn()
    {
        LevelManager.Instance.Respawn(this);
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(_transform.position.x + randomX, _transform.position.y, _transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -_transform.up, maxDistance, groundLayer))
        {
            walkPointSet = true;
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public override void Activate()
    {
        base.Activate();

        float randomX = Random.Range(minRange, maxRange);
        float randomZ = Random.Range(minRange, maxRange);
        _transform.position = new Vector3(randomX, _transform.position.y, randomZ);
        isDead = false;
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private void OnGameOver()
    {
        Deactivate();
    }

    private void Subscribe()
    {
        LevelManager.Instance.OnGameVictory += OnGameOver;
        LevelManager.Instance.OnGameOver += OnGameOver;
        LevelManager.Instance.OnGameStart += OnInit;
        LevelManager.Instance.OnEnterMenu += OnGameOver;
    }

    private void Unsubscribe()
    {
        LevelManager.Instance.OnGameVictory -= OnGameOver;
        LevelManager.Instance.OnGameOver -= OnGameOver;
        LevelManager.Instance.OnGameStart -= OnInit;
        LevelManager.Instance.OnEnterMenu -= OnGameOver;
    }
}
