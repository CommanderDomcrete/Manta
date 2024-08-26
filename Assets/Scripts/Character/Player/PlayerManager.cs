using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public static PlayerManager instance;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public GunController gunController;
    [HideInInspector] public PlayerControls playerControls;


    protected override void Awake()
    {
        base.Awake();
        //^RUNS THE AWAKE FUNCTION FROM THE CHARACTER MANAGER
        //AND DO MORE STUFF JUST FOR THE PLAYER
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        gunController = GetComponent<GunController>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;

        maxEnergy = playerStatsManager.CalculateEnergyBasedOnGenerator(power);
        currentEnergy = playerStatsManager.CalculateEnergyBasedOnGenerator(power);

        PlayerUIManager.instance.playerUIHUDManager.SetMaxEnergyValue(maxEnergy);
        gunController.EquipGun(2, 2);


        PlayerUIManager.instance.playerUIHUDManager.SetMaxHitPointsValue(maxHitPointsValue);
    }
    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();
        PlayerUIManager.instance.playerUIHUDManager.SetNewEnergyValue(currentEnergy);
        PlayerUIManager.instance.playerUIHUDManager.SetNewHitPointsValue(currentHitPointsValue);

        //REGEN STAMINA
        playerStatsManager.RegenerateEnergy();
        if (GameManager.instance.gameOver)
        {
            Restart();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
        PlayerCamera.instance.HandleLockOn();
    }

    public override void TakeDamage(Vector3 hitPoint, Vector3 hitDirection)
    {
        base.TakeDamage(hitPoint, Vector3.up);
    }

    public void Restart()
    {
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHitPointsValue(maxHitPointsValue);
        currentHitPointsValue = maxHitPointsValue;
        dead = false;
    }
}
