using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class Player : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Vector3 startPosition;
    private int levelToAdjust = 0;
    private int levelSegment = 3;
    private GameObject previousTarget;

    private void Awake()
    {
        _transform = transform;
        _camera.SetPlayerReference(_transform);
        Level = 0;
    }

    private void Start()
    {
        Unsubscribe();
        Subscribe();
        ChangeWeapon((WeaponType)GameManager.Instance.UserData.equippedWeapon);
        ChangeHat(GameManager.Instance.UserData.equippedHat);
        ChangePants(GameManager.Instance.UserData.equippedPants);
        ChangeShield(GameManager.Instance.UserData.equippedShield);
    }

    protected override void OnInit()
    {
        base.OnInit();
        _transform.position = startPosition;
        SetAttributes((WeaponType)GameManager.Instance.UserData.equippedWeapon);
        gameObject.layer = LayerMask.NameToLayer(MyConst.Layer.CHARACTER);
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
        isDead = true;
        ChangeAnimation(MyConst.Animation.DEAD);
        LevelManager.Instance.OnPlayerDeath();
    }

    public override void OnKill()
    {
        base.OnKill();

        levelToAdjust++;

        if(levelToAdjust >= levelSegment && Level < maxLevel)
        {
            _camera.AdjustCamera(Level);
            levelToAdjust = 0;
        }

        LevelManager.Instance.OnBotKill();
    }

    public void ChangeWeapon(WeaponType type)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(weaponSO.GetWeapon(type), holdWeapon.transform).GetComponent<Weapon>();
    }

    public void ChangeHat(int index)
    {
        if(index == -1)
        {
            return;
        }
        
        if(currentHat != null)
        {
            Destroy(currentHat.gameObject);
        }

        currentHat = Instantiate(hatSO.GetSkinByIndex(index).skin, holdHat.transform);
    }

    public void ChangePants(int index)
    {
        if (index == -1)
        {
            return;
        }

        pantsRenderer.material = pantsSO.GetSkinByIndex(index).skinMaterial;
    }

    public void ChangeShield(int index)
    {
        if (index == -1)
        {
            return;
        }

        if(currentShield != null)
        {
            Destroy(currentShield.gameObject);
        }

        currentShield = Instantiate(shieldSO.GetSkinByIndex(index).skin, holdShield.transform);
    }

    private void OnWeaponChanged()
    {
        WeaponType type = (WeaponType)GameManager.Instance.UserData.equippedWeapon;
        ChangeWeapon(type);
    }
    
    private void OnHatChanged()
    {
        int index = GameManager.Instance.UserData.equippedHat;
        ChangeHat(index);
    }

    private void OnPantsChanged()
    {
        int index = GameManager.Instance.UserData.equippedPants;
        ChangePants(index);
    }

    private void OnShieldChanged()
    {
        int index = GameManager.Instance.UserData.equippedShield;
        ChangeShield(index);
    }

    private void OnVictory()
    {
        ChangeAnimation(MyConst.Animation.WIN);
        GoImmune();
    }

    private void ResetAnimation()
    {
        ChangeAnimation(MyConst.Animation.IDLE);
    }

    private void GoImmune()
    {
        gameObject.layer = LayerMask.NameToLayer(MyConst.Layer.INVINCIBLE);
    }

    private void Subscribe()
    {
        EquipmentManager.Instance.OnWeaponChanged += OnWeaponChanged;
        EquipmentManager.Instance.OnHatChanged += OnHatChanged;
        EquipmentManager.Instance.OnPantsChanged += OnPantsChanged;
        EquipmentManager.Instance.OnShieldChanged += OnShieldChanged;
        LevelManager.Instance.OnGameVictory += OnVictory;
        LevelManager.Instance.OnGameStart += OnInit;
        LevelManager.Instance.OnEnterMenu += ResetAnimation;
        LevelManager.Instance.OnEnterMenu += GoImmune;
    }

    private void Unsubscribe()
    {
        EquipmentManager.Instance.OnWeaponChanged -= OnWeaponChanged;
        EquipmentManager.Instance.OnHatChanged -= OnHatChanged;
        EquipmentManager.Instance.OnPantsChanged -= OnPantsChanged;
        EquipmentManager.Instance.OnShieldChanged -= OnShieldChanged;
        LevelManager.Instance.OnGameVictory -= OnVictory;
        LevelManager.Instance.OnGameStart -= OnInit;
        LevelManager.Instance.OnEnterMenu -= ResetAnimation;
        LevelManager.Instance.OnEnterMenu -= GoImmune;
    }
}
