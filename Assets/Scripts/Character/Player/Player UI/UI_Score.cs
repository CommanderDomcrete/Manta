using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Score : MonoBehaviour
{
    private TextMeshProUGUI score;
    protected virtual void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();
        score.text = 0.ToString();
    }

    public virtual void SetScore(int newValue)
    {
        score.text = newValue.ToString();
    }
}
