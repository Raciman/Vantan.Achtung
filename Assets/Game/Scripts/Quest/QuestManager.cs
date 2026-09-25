using Ach.Quest;
using Ach.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestZone[] questSteps;
    [SerializeField] private UIManager uiManager;
    private int _currentStep = 0;
    private bool _isCompleted;
    
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
        if(_isCompleted)
            return;
        
        questSteps[_currentStep].Activate(false);
        _currentStep++;
        questSteps[_currentStep].Activate(true);
        ShowTip();

        if (_currentStep >= questSteps.Length - 1)
        {
            QuestCompleteHandler();
Debug.Log("Completed");
        }
        
    }

    private void QuestCompleteHandler()
    {
        _isCompleted = true;
        Invoke(nameof(LoadWinScene), 4f);
        
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene(3);
    }

    public void ShowTip()
    {
        uiManager.ShowQuestTip(questSteps[_currentStep].QuestTip);
    }
}
