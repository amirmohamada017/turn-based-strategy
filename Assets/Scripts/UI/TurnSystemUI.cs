using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
        UpdateTurnNumberText();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnNumberText();
    }

    private void UpdateTurnNumberText()
    {
        var turnNumber = TurnSystem.Instance.GetTurnNumber();
        turnNumberText.text = $"Turn: {turnNumber}";
    }
}