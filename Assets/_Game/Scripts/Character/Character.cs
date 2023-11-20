using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] internal Transform throwPoint;
    [SerializeField] private GameObject holdWeapon;
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] internal float attackRange = 5f;

    protected Vector3 moveVector;
    protected Collider my_collider;
    protected Vector3 direction;
    internal float timer = 0f;
    internal bool canAttack = true;
    internal Collider[] enemiesInRange;
    internal string currentAnimation;
    internal GameObject target;
    protected bool isDead;
    protected int level;

    public GameObject Target => target;


    internal void Moving()
    {
        direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.fixedDeltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(direction);

        timer = 0f;
        canAttack = true;
        target = null;
        ShowWeapon();
        ChangeAnimation(MyConst.Animation.RUN);
    }

    internal void PrepareAttack()
    {
        timer += Time.fixedDeltaTime;
        
        if (timer <= 0.5f)
        {
            return;
        }

        if (canAttack && target != null)
        {
            ExecuteAttack(throwPoint, (target.transform.position - transform.position).normalized);
            timer = 0f;
            canAttack = false;
        }
    }

    private void ResetAttack()
    {
        timer = 0f;
        canAttack = true;
    }

    protected void FindTarget()
    {
        enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, characterLayer);

        if (enemiesInRange.Length > 1)
        {
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                if (enemiesInRange[i] != my_collider)
                {
                    target = enemiesInRange[i].gameObject;
                }
            }
        }
        else
        {
            target = null;
        }

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    internal void ExecuteAttack(Transform throwPoint, Vector3 direction)
    {
        ChangeAnimation(MyConst.Animation.ATTACK);
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        GameObject bulletObj = ObjectPool.instance.GetPooledObject();

        if (bulletObj != null)
        {
            HideWeapon();
            
            Vector3 startPos = throwPoint.position;
            bulletObj.transform.position = startPos;

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.attacker = this;
            if(bullet.attacker.level <= 15)
            {
                float scaleMult = bullet.attacker.level;
                bullet.transform.localScale = Vector3.one + new Vector3(0.1f, 0.1f, 0.1f) * scaleMult;
                bullet.speed += 1f * scaleMult/2;
                bullet.rotateSpeed += 1f * scaleMult;
            }
            else
            {
                bullet.transform.localScale = Vector3.one + new Vector3(0.1f, 0.1f, 0.1f) * 15;
                bullet.speed += 1f * 15/2;
                bullet.rotateSpeed += 1f * 15;

            }

            bullet.Activate(direction);
        }
    }

    public virtual void OnHit()
    {
        Deactivate();
    }

    public virtual void OnKill()
    {
        level++;

        Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 positionChange = new Vector3(0.0f, 0.05f, 0.0f);

        if (level <= 15)
        {
            transform.localScale += scaleChange;
            transform.position += positionChange;
        }


        attackRange += 0.25f;
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        isDead = false;
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        isDead = true;
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
