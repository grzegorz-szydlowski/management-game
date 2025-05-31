using System;
using Core.GameSystems;
using Enumerations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDBehaviorScript : MonoBehaviour
{
    private TMP_Text moraleText;
    private TMP_Text stageText;
    private TMP_Text budgetText;
    private TMP_Text timeText;

    private void Start()
    {
        moraleText = transform.Find("Morale").GetComponent<TMP_Text>();
        stageText = transform.Find("Stage").GetComponent<TMP_Text>();
        budgetText = transform.Find("Budget").GetComponent<TMP_Text>();
        timeText = transform.Find("Time").GetComponent<TMP_Text>();
        UpdateHUD(GameManagerScript.Instance.currentGameState.Morale,GameManagerScript.Instance.currentGameState.Stage,GameManagerScript.Instance.currentGameState.Budget,GameManagerScript.Instance.currentGameState.Time);
        
    }

    public void UpdateHUD(int morale, int stage, int budget,int time)
    {
        moraleText.text = GetMoraleTextFromValue(morale);
        stageText.text = "Stage: " + stage;
        budgetText.text = budget+" USD";
        timeText.text = time + " H";
    }

    private static string GetMoraleTextFromValue(int moraleValue)
    {
        return moraleValue switch
        {
            <= 0 => $"{moraleValue} ({MoraleType.Terrible.ToString()})",
            <= 35 => $"{moraleValue} ({MoraleType.Bad.ToString()})",
            <= 65 => $"{moraleValue} ({MoraleType.Okay.ToString()})",
            <= 100 => $"{moraleValue} ({MoraleType.Good.ToString()})",
            _ => $"{moraleValue} ({MoraleType.Excellent.ToString()})"
        };
    }
}
