using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] internal Transform throwPoint;
    [SerializeField] private GameObject holdWeapon;
    [SerializeField] private LayerMask layerMask;

    protected Vector3 moveVector;
    protected Collider my_collider;
    protected Vector3 direction;
    internal float timer = 0f;
    internal bool canAttack = false;
    internal float attackRange = 5f;
    internal Collider[] enemiesInRange;
    internal string currentAnimation;
    internal GameObject target;

    public GameObject Target => target;


    private void Awake()
    {
        
    }

    internal void Moving()
    {
        direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.fixedDeltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(direction);

        timer = 0f;
        canAttack = true;
        ShowWeapon();
        ChangeAnimation("Run");
    }

    internal void PrepareAttack()
    {
        timer += Time.fixedDeltaTime;
        ChangeAnimation("Idle");
        if (timer <= 0.5f)
        {
            return;
        }

        if (canAttack && target != null)
        {
            ChangeAnimation("Attack");
            transform.LookAt(target.transform.position);
            ExecuteAttack(throwPoint, (target.transform.position - transform.position).normalized);
            timer = 0f;
            canAttack = false;
        }
    }

    protected void FindTarget()
    {
        int maxTargets = 10;
        enemiesInRange = new Collider[maxTargets];

        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, attackRange, enemiesInRange, layerMask);
        if (numEnemies <= 1)
        {
            target = null;
            return;  
        }

        for (int i = 0; i < numEnemies; i++)
        {
            if (enemiesInRange[i] != my_collider)
            {
                target = enemiesInRange[i].gameObject;
            }
        }
    }

    internal void ExecuteAttack(Transform throwPoint, Vector3 direction)
    {

        GameObject bulletObj = ObjectPool.instance.GetPooledObject();

        if (bulletObj != null)
        {
            HideWeapon();
            
            Vector3 startPos = throwPoint.position;
            bulletObj.transform.position = startPos;

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.attacker = this;
            bullet.Activate(direction);
        }
    }

    protected void HideWeapon()
    {
        holdWeapon.SetActive(false);
    }

    protected void ShowWeapon()
    {
        holdWeapon.SetActive(true);
    }

    internal void ChangeAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            if (currentAnimation != null)
            {
                animator.ResetTrigger(currentAnimation);
            }
            
            currentAnimation = animation;

            animator.SetTrigger(currentAnimation);
        }
    }

}
