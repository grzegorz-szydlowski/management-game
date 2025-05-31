using System;
using System.Collections.Generic;
using Core.Data.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskChoiceScript : MonoBehaviour
{
    private TaskDTO data;
    private Action<int, string> onAssigned;
    private string currentSelectedDeveloper;

    [SerializeField] private TMP_Text taskText;
    [SerializeField] private Button dev1Button;
    [SerializeField] private Button dev2Button;
    [SerializeField] private Button dev3Button;
    [SerializeField] private Button dev4Button;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    private Dictionary<string, Button> devButtons;

    public void Setup(TaskDTO task, Action<int, string> onAssignCallback)
    {
        data = task;
        onAssigned = onAssignCallback;
        taskText.text = $"{task.Title}\nCost per hour - {task.CostPerHour}";

        devButtons = new Dictionary<string, Button>
        {
            { "Bob", dev1Button },
            { "Mark", dev2Button },
            { "Dan", dev3Button },
            { "Jenny", dev4Button }
        };

        dev1Button.onClick.AddListener(() => OnDeveloperClicked("Bob"));
        dev2Button.onClick.AddListener(() => OnDeveloperClicked("Mark"));
        dev3Button.onClick.AddListener(() => OnDeveloperClicked("Dan"));
        dev4Button.onClick.AddListener(() => OnDeveloperClicked("Jenny"));
    }

    private void OnDeveloperClicked(string developerName)
    {
        if (currentSelectedDeveloper == developerName)
            return;

        currentSelectedDeveloper = developerName;
        
        foreach (var kvp in devButtons)
        {
            var image = kvp.Value.GetComponent<Image>();
            image.sprite = kvp.Key == developerName ? selectedSprite : normalSprite;
        }
        
        onAssigned?.Invoke(data.Id, developerName);
    }
}