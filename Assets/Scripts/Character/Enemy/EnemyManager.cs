using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : CharacterManager
{
    [HideInInspector] public GunController gunController;
    [HideInInspector] public EnemyFlightManager enemyFlightManager;
    [HideInInspector] public EnemyTargetingManager enemyTargetingManager;
    

    protected override void Awake()
    {
        base.Awake();
        //^RUNS THE AWAKE FUNCTION FROM THE CHARACTER MANAGER
        //AND DO MORE STUFF JUST FOR THE PLAYER
        enemyTargetingManager = GetComponent<EnemyTargetingManager>();
        enemyFlightManager = GetComponent<EnemyFlightManager>();
        gunController = GetComponent<GunController>();
        
    }

    public void Start()
    {
        gunController.EquipGun(0);
    }
    protected override void Update()
    {
        if (gameObject == null)
            return;
        base.Update();

        enemyFlightManager.HandleAllMovement();
        enemyTargetingManager.HandleAllTargeting();
        if (GameManager.instance.gameOver)
        {
            Kill();
        }
    }

    public override void TakeDamage(Vector3 hitPoint, Vector3 hitDirection)
    {
        if (gameObject == null)
            return;
        base.TakeDamage(hitPoint, hitDirection);
    }
    protected override void Die()
    {
        if (gameObject == null)
            return;
        base.Die();
         Destroy(gameObject);

    }
    void Kill()
    {
        if (gameObject == null)
            return;
        Destroy(gameObject);
    }
}
