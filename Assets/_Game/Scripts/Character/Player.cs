using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private FloatingJoystick joystick;
    
    private int levelToAdjust = 0;
    private int levelSegment = 5;
    private GameObject previousTarget;
    private DataManager dataManager;
    public WeaponSO weaponSO;


    private void Awake()
    {
        _transform = transform;
        _camera.SetPlayerReference(_transform);
        Level = 0;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsState(GameManager.GameState.Gameplay))
        {
            return;
        }

        MoveWithJoystick();

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

        SetTargetIndicator();
    }

    private void MoveWithJoystick()
    {
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.fixedDeltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.fixedDeltaTime;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Moving();
        }
        else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            ChangeAnimation(MyConst.Animation.IDLE);
            PrepareAttack();
        }

        rb.MovePosition(rb.position + moveVector);
    }

    private void SetTargetIndicator()
    {
        if (target == previousTarget)
        {
            return;
        }

        if (previousTarget != null)
        {
            Bot bot = previousTarget.GetComponent<Bot>();

            if (bot != null)
            {
                bot.targetIndicator.SetActive(false);
            }
        }

        if (target != null)
        {
            Bot bot = target.GetComponent<Bot>();

            if (bot != null)
            {
                bot.targetIndicator.SetActive(true);
            }
        }
        previousTarget = target;
    }



    public override void Deactivate()
    {
        
    }

    public override void OnKill()
    {
        base.OnKill();

        levelToAdjust++;

        if(levelToAdjust >= levelSegment && Level <= maxLevel)
        {
            _camera.AdjustCamera();
            levelToAdjust = 0;
        }
    }
    
    //private void SaveCurrentWeapon()
    //{
    //    WeaponData currentWeaponData = weaponSO.weapons[0];
    //    dataManager.SaveToPlayerPref(currentWeaponData);
    //    Debug.Log(currentWeaponData.weaponType);
    //}
}
