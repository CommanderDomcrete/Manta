using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_HitPoints : MonoBehaviour
{
    private TextMeshProUGUI hitPoints;
    
    protected virtual void Awake()
    {
        hitPoints = GetComponent<TextMeshProUGUI>(); 
    }
    
    public virtual void SetHitPoints(int newValue)
    {
        if(newValue < 0)
        {
            newValue = 0;
        }
        hitPoints.text = newValue.ToString();  
    }

    public virtual void SetMaxHitPoints(int maxValue)
    {
        hitPoints.text = maxValue.ToString();
    }

}
