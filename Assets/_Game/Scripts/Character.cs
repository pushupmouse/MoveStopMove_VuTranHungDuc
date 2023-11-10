using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //private float timer = 0f;
    //private bool canAttack = true;
    private string currentAnimation;

    private void Awake()
    {

    }

    //public void PrepareAttack(Vector3 moveVector)
    //{
    //    if (moveVector.x != 0 || moveVector.z != 0)
    //    {
    //        timer = 0f;
    //        canAttack = true;
    //    }
    //    else
    //    {
    //        timer += Time.fixedDeltaTime;
    //        if (timer > 1f)
    //        {
    //            if (canAttack)
    //            {
    //                ExecuteAttack();
    //                timer = 0f;
    //                canAttack = false;
    //            }
                
    //        }
    //    }
    //}


    public virtual void ExecuteAttack(Transform throwPoint, Vector3 direction)
    {

        GameObject bulletObj = ObjectPool.instance.GetPooledObject();

        if (bulletObj != null)
        {
            Vector3 startPos = throwPoint.position;
            bulletObj.transform.position = startPos;
            bulletObj.transform.rotation = transform.rotation;
            bulletObj.SetActive(true);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.rb.velocity = direction * 5f;
        }
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
