using Ach.Quest;
using Ach.UI;
using UnityEngine;

public sealed class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestZone[] questSteps;
    [SerializeField] private UIManager uiManager;
    private int _currentStep = 0;

    private void Awake()
    {
        foreach (var step in questSteps)
        {
            step.Activate(false);
        }
        questSteps[0].Activate(true);
    }

    private void Start()
    {
        Invoke(nameof(ShowTip), 2f);
    }

    public void ProgressStep()
    {
        questSteps[_currentStep].Activate(false);
        _currentStep++;

        if (_currentStep >= questSteps.Length)
        {
            QuestCompleteHandler();
            return;
        }
        
        questSteps[_currentStep].Activate(true);
        ShowTip();
    }

    private void QuestCompleteHandler()
    {
        
    }

    public void ShowTip()
    {
        uiManager.ShowQuestTip(questSteps[_currentStep].QuestTip);
    }
}
