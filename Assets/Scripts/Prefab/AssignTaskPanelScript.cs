using System;
using System.Collections.Generic;
using Core.Data.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssignTaskPanelScript : MonoBehaviour
{
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject taskChoicePrefab;
    [SerializeField] private Button okButton;

    private Dictionary<int, string> taskAssignments = new();
    private Action<Dictionary<int, string>> onAssignmentsComplete;
    private List<TaskDTO> currentTasks = new();

    public void Initialize(Action<Dictionary<int, string>> onCompleteCallback)
    {
        onAssignmentsComplete = onCompleteCallback;
        okButton.gameObject.SetActive(false);
        okButton.onClick.AddListener(() =>
        {
            onAssignmentsComplete?.Invoke(taskAssignments);
        });
    }

    public void Populate(List<TaskDTO> tasks)
    {
        taskAssignments.Clear();
        okButton.gameObject.SetActive(false);
        currentTasks = tasks;

        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var task in tasks)
        {
            GameObject taskChoiceGO = Instantiate(taskChoicePrefab, contentContainer);
            TaskChoiceScript choice = taskChoiceGO.GetComponent<TaskChoiceScript>();
            choice.Setup(task, HandleAssignment);
        }
    }

    private void HandleAssignment(int taskId, string developerName)
    {
        // Assign or reassign
        if (taskAssignments.TryGetValue(taskId, out string currentDev) && currentDev == developerName)
        {
            taskAssignments.Remove(taskId); // Toggle off (unassign)
        }
        else
        {
            taskAssignments[taskId] = developerName;
        }

        // Show OK button only when all tasks are assigned
        if (taskAssignments.Count == currentTasks.Count)
        {
            okButton.gameObject.SetActive(true);
        }
        else
        {
            okButton.gameObject.SetActive(false);
        }

        Debug.Log($"Current assignments: {string.Join(", ", taskAssignments)}");
    }
}
