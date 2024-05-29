using UnityEngine;
using TMPro;
using UnityEngine.UI;
using YG;

public class TutorialPlayer : MonoBehaviour
{
    public static TutorialPlayer instance;
    public TextMeshProUGUI[] tutorialMessages;
    public Button[] tutorialButtons;
    private int currentMessageIndex = -1;

    public GameObject[] traningPanel;
    private bool tutorialHidden = false;

    void Awake()
    {
        TutorialPlayer.instance = this;
    }

    void Update()
    {
        if (!tutorialHidden && Input.GetMouseButtonDown(0))
        {
            ShowNextMessage();
        }
    }

    void ShowNextMessage()
    {
        if (currentMessageIndex >= 0 && currentMessageIndex < tutorialMessages.Length)
        {
            tutorialMessages[currentMessageIndex].gameObject.SetActive(false);
        }

        currentMessageIndex++;

        if (currentMessageIndex < tutorialMessages.Length)
        {
            tutorialMessages[currentMessageIndex].gameObject.SetActive(true);
        }
        else
        {
            HideTutorial();
        }
    }

    public void GetData()
    {
        // ��������� ������ �� ����������
        SavesYG savesData = YandexGame.savesData;

        // ���������, ������ �� ����� ��������
        bool tutorialCompleted = savesData.tutorialCompleted;

        // ���� �������� �� ��������, ���������� ���
        if (!tutorialCompleted)
        {
            foreach (var button in tutorialButtons)
            {
                button.interactable = false;
            }

            ShowNextMessage();
        }
        else
        {
            // ���� �������� ��������, �������� ���
            HideTutorial();
        }
    }

    void HideTutorial()
    {
        foreach (var message in tutorialMessages)
        {
            message.gameObject.SetActive(false);
        }

        for (int i = 0; i < tutorialButtons.Length; i++)
        {
            if (i != 0)
            {
                tutorialButtons[i].interactable = true;
            }
        }
        foreach (var panel in traningPanel)
        {
            panel.SetActive(false);
        }

        tutorialHidden = true;

        // ��������� �������� tutorialCompleted � true ��� ���������� ���������
        YandexGame.savesData.tutorialCompleted = true;

        // ���������� ������ � ���������
        YandexGame.SaveProgress();
    }
}
