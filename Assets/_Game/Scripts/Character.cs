using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject holdWeapon;
    [SerializeField] private LayerMask layerMask;

    protected Vector3 moveVector;
    protected Collider my_collider;
    protected Vector3 direction;
    protected float timer = 0f;
    protected bool canAttack = false;
    protected float attackRange = 5f;
    protected Collider[] enemiesInRange;
    protected GameObject target;
    protected string currentAnimation;

    private void Awake()
    {
        my_collider = GetComponent<Collider>();
    }

    protected void Moving()
    {
        direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.fixedDeltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(direction);

        timer = 0f;
        canAttack = true;
        ShowWeapon();
        ChangeAnimation("Run");
    }

    protected void Stopping()
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
        if (numEnemies <= 0)
        {
            return;
        }

        for (int i = 0; i < numEnemies; i++)
        {
            if (enemiesInRange[i] != my_collider)
            {
                target = enemiesInRange[i].gameObject;
            }
            else
            {
                target = null;
            }
        }
    }

    protected void ExecuteAttack(Transform throwPoint, Vector3 direction)
    {

        GameObject bulletObj = ObjectPool.instance.GetPooledObject();

        if (bulletObj != null)
        {
            HideWeapon();
            
            Vector3 startPos = throwPoint.position;
            bulletObj.transform.position = startPos;

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.attacker = this.gameObject;
            bullet.Activate(direction);
            //bullet.rb.velocity = direction * 5f
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

    protected void ChangeAnimation(string animation)
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
