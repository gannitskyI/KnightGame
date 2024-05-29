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
        // «агружаем данные из сохранений
        SavesYG savesData = YandexGame.savesData;

        // ѕровер€ем, прошел ли игрок туториал
        bool tutorialCompleted = savesData.tutorialCompleted;

        // ≈сли туториал не завершен, показываем его
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
            // ≈сли туториал завершен, скрываем его
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

        // ”становка значени€ tutorialCompleted в true при завершении туториала
        YandexGame.savesData.tutorialCompleted = true;

        // —охранение данных о прогрессе
        YandexGame.SaveProgress();
    }
}
