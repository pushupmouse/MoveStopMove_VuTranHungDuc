using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    protected Vector3 moveVector;
    protected Collider my_collider;
    protected Vector3 direction;
    internal float timer = 0f;
    internal bool canAttack = true;
    internal float attackRange = 5f;
    internal Collider[] enemiesInRange;
    internal string currentAnimation;
    internal GameObject target;

    public GameObject Target => target;


    internal void Moving()
    {
        direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.fixedDeltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(direction);

        timer = 0f;
        canAttack = true;
        ShowWeapon();
        ChangeAnimation(MyConst.Animation.RUN);
    }

    internal void PrepareAttack()
    {
        timer += Time.fixedDeltaTime;
        ChangeAnimation(MyConst.Animation.IDLE);
        if (timer <= 0.5f)
        {
            return;
        }

        if (canAttack && target != null)
        {
            ChangeAnimation(MyConst.Animation.ATTACK);
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            ExecuteAttack(throwPoint, (target.transform.position - transform.position).normalized);
            timer = 0f;
            canAttack = false;
            Invoke(nameof(ResetAttack), 2f);
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
        if (enemiesInRange.Length > 0)
        {
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                //CHECK DISTANCE IF THE BOT TOO FAR AWAY BY RESPAWNING THEN TARGET NULL!!!!
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

    public virtual void OnHit()
    {
        Deactivate();
    }

    internal void OnKill()
    {
        target = null;
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
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
