using System;
using System.Collections.Generic;
using Core;
using Core.Data.Models;
using Core.GameSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventPanelScript : MonoBehaviour
{
    private Action<int, Reaction> onReaction;
    private List<Reaction> reactions;
    private ScenarioManager scenarioManager;
    [SerializeField] private TMP_Text reactionText; 
    [SerializeField] private Button doNothingButton;
    [SerializeField] private Button reaction1Button;
    [SerializeField] private Button reaction2Button;
    [SerializeField] private Button reaction3Button;

    public void Setup(RandomEvent randomEvent, Action<int,Reaction> onReactionCallback )
    {
        onReaction = onReactionCallback;
        scenarioManager = SystemProvider.GetSystem<ScenarioManager>();
        reactions = scenarioManager.GetReactionsByEventId(randomEvent.Id);
        reactionText.text = randomEvent.Name;
        doNothingButton.onClick.AddListener(()=>OnButtonClick(0));
        doNothingButton.GetComponentInChildren<TMP_Text>().text = reactions[0].Action;
        reaction1Button.onClick.AddListener(()=>OnButtonClick(1));
        reaction1Button.GetComponentInChildren<TMP_Text>().text = reactions[1].Action;
        reaction2Button.onClick.AddListener(()=>OnButtonClick(2));
        reaction2Button.GetComponentInChildren<TMP_Text>().text = reactions[2].Action;
        reaction3Button.onClick.AddListener(()=>OnButtonClick(3));
        reaction3Button.GetComponentInChildren<TMP_Text>().text = reactions[3].Action;
    }

    private void OnButtonClick(int reactionIndex)
    {
        onReaction?.Invoke(reactionIndex,reactions[reactionIndex]);
    }
}
