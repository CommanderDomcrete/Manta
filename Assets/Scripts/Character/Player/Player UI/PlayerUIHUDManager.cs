using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerUIHUDManager : MonoBehaviour
{
    [SerializeField] UI_StatBar energyBar;
    [SerializeField] UI_Reticule reticule;
    [SerializeField] UI_HitPoints hitPoints;
    [SerializeField] UI_Score score;

    public void Start()
    {
        gameObject.SetActive(true);
    }
    public void SetNewEnergyValue(float newValue)
    {
        energyBar.SetStat(newValue);
    }

    public void SetMaxEnergyValue(int maxEnergy)
    {
        energyBar.SetMaxStat(maxEnergy);
    }

    public void SetMaxHitPointsValue(int maxValue)
    {
        hitPoints.SetMaxHitPoints(maxValue);
    }

    public void SetNewHitPointsValue(int newValue)
    {
        hitPoints.SetHitPoints(newValue);

    }

    public void SetNewScoreValue(int newScore)
    {
        score.SetScore(newScore);
    }
}
