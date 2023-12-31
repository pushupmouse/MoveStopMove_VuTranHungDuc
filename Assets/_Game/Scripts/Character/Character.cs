using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] protected GameObject holdWeapon;
    [SerializeField] protected GameObject holdHat;
    [SerializeField] protected GameObject holdShield;
    [SerializeField] protected SkinnedMeshRenderer pantsRenderer;
    [SerializeField] protected int minLevel = 1;
    [SerializeField] protected int maxLevel = 15;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected SkinSO hatSO;
    [SerializeField] protected SkinSO pantsSO;
    [SerializeField] protected SkinSO shieldSO;
    [SerializeField] internal Transform throwPoint;
    [SerializeField] internal float attackRange = 1f;
    
    private int level;
    private float maxMagnitude = 0f;
    protected float cooldown = 0.5f;
    protected float idleCooldown = 1.5f;
    private float speedMult = 0.5f;
    private float rotateMult = 1f;
    private float scaleBonus = 0.05f;
    private float heightOffset = 0.025f;
    private float attackRangeBonus = 0.1f;
    private float ratio = 8f;
    protected Transform _transform;
    protected Vector3 moveVector;
    protected Vector3 direction;
    protected bool isDead;
    protected Weapon currentWeapon;
    protected GameObject currentHat;
    protected GameObject currentPants;
    protected GameObject currentShield;
    internal float timer = 0f;
    internal float idleTimer = 0f;
    internal bool canAttack = true;
    internal Collider[] enemiesInRange;
    internal string currentAnimation;
    internal GameObject target;

    public GameObject Target => target;


    protected int Level
    {
        get => level; set
        {
            level = Math.Clamp(value, minLevel, maxLevel);
        }
    }
    private void Awake()
    {
        _transform = transform;
        OnInit();
    }

    private void Start()
    {

    }

    protected virtual void OnInit()
    {
        level = minLevel;
        _transform.localScale = Vector3.one;
        cooldown = 0.5f;
        idleCooldown = 1.5f;
        attackRange = 5f;
    }

    internal void Moving()
    {
        direction = Vector3.RotateTowards(_transform.forward, moveVector, rotateSpeed * Time.fixedDeltaTime, maxMagnitude);
        _transform.rotation = Quaternion.LookRotation(direction);

        timer = 0f;
        canAttack = true;
        target = null;
        ShowWeapon();
        ChangeAnimation(MyConst.Animation.RUN);
    }

    internal void PrepareAttack()
    {
        timer += Time.fixedDeltaTime;
        idleTimer += Time.fixedDeltaTime;

        if (timer <= cooldown)
        {
            return;
        }

        if (canAttack && target != null)
        {
            ExecuteAttack(throwPoint, (target.transform.position - _transform.position).normalized);
            ResetAttack();
        }

        if (idleTimer >= idleCooldown && target != null)
        {
            ExecuteAttack(throwPoint, (target.transform.position - _transform.position).normalized);
            ResetAttack();
        }
    }

    private void ResetAttack()
    {
        timer = 0f;
        canAttack = false;
        idleTimer = 0f;
    }

    protected void FindTarget()
    {
        enemiesInRange = Physics.OverlapSphere(_transform.position, attackRange, characterLayer);

        float nearestDistance = float.MaxValue;
        target = null;

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            Collider enemyCollider = enemiesInRange[i];

            if (enemyCollider != _collider)
            {
                float distance = Vector3.Distance(_transform.position, enemyCollider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    target = enemyCollider.gameObject;
                }
            }
        }
    }

    internal void ExecuteAttack(Transform throwPoint, Vector3 direction)
    {
        ChangeAnimation(MyConst.Animation.ATTACK);
        _transform.LookAt(new Vector3(target.transform.position.x, _transform.position.y, target.transform.position.z));

        GameObject bulletObj = BulletPool.Instance.GetPooledObject(currentWeapon.weaponType);

        if (bulletObj != null)
        {
            HideWeapon();

            Vector3 startPos = throwPoint.position;
            bulletObj.transform.position = startPos;

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.attacker = this;
            if (bullet.attacker.Level <= maxLevel)
            {
                float scaleMult = bullet.attacker.Level;
                bullet.transform.localScale = Vector3.one + new Vector3(bullet.bulletScaleBonus, bullet.bulletScaleBonus, bullet.bulletScaleBonus) * scaleMult;
                bullet.speed += speedMult * scaleMult;
                bullet.rotateSpeed += rotateMult * scaleMult;
            }
            else
            {
                bullet.transform.localScale = Vector3.one + new Vector3(bullet.bulletScaleBonus, bullet.bulletScaleBonus, bullet.bulletScaleBonus) * maxLevel;
                bullet.speed += speedMult * maxLevel;
                bullet.rotateSpeed += rotateMult * maxLevel;

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
        Level++;
        if (Level < maxLevel)
        {
            Vector3 scaleChange = new Vector3(scaleBonus, scaleBonus, scaleBonus);
            Vector3 positionChange = new Vector3(0f, heightOffset, 0f);
            _transform.localScale += scaleChange;
            _transform.position += positionChange;
            attackRange += attackRangeBonus + attackRangeBonus * Level/ratio;
        }
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

    protected void SetAttributes(WeaponType type)
    {
        attackRange += weaponSO.GetRange(type);
        cooldown -= weaponSO.GetSpeed(type);
        idleCooldown -= weaponSO.GetSpeed(type);
    }
}
