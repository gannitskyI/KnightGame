using UnityEngine;
using TMPro;
using System.IO;
using DG.Tweening;
using YG;
using System.Linq;

public class MoneyCounter : MonoBehaviour
{
    public static MoneyCounter Instance;
    public TextMeshProUGUI coinText;
    public int moneyAmount = 5;
    public static int score = 0;

    public float moveSpeed = 1f; // �������� ����������� �������
    public float oscillationDistance = 1f; // ���������� ���������
   

    private Tween oscillationTween; // ������ �� ���� ��� ����������� �������

    public delegate void MoneyUpdatedHandler(int newScore);
    public static event MoneyUpdatedHandler OnMoneyUpdated;

    public AudioClip coinPickupSound; // ��������� ��� ����� ����� ������
    public AudioSource audioSource; // �������������


    private void Start()
    {
        MoneyCounter.Instance = this;
        FindAudioSourceAndTextMeshPro();
        LoadScore();
    }


    private void FindAudioSourceAndTextMeshPro()
    {
        // ������� ��� ������� AudioSource � �����
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // ���� AudioSource � ������ "AudioSource"
        AudioSource foundAudioSource = audioSources.FirstOrDefault(audio => audio.gameObject.name == "AudioSource");

        // ���������, ������ �� AudioSource
        if (foundAudioSource != null)
        {
            // ���� ������, ����������� ��� ���������� audioSource
            audioSource = foundAudioSource;
        }
        else
        {
            Debug.LogWarning("AudioSource � ������ 'AudioSource' �� ������ �� �����.");
        }

        // ������� ��� ������� TextMeshPro � �����
        TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();

        // ���� TextMeshPro � ������ "MoneyCounter"
        TextMeshProUGUI foundTextMeshPro = textMeshPros.FirstOrDefault(text => text.gameObject.name == "MoneyCounter");

        // ���������, ������ �� TextMeshPro
        if (foundTextMeshPro != null)
        {
            // ���� ������, ����������� ��� ���������� coinText
            coinText = foundTextMeshPro;
        }
        else
        {
            Debug.LogWarning("TextMeshPro � ������ 'MoneyCounter' �� ������ �� �����.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddMoney(moneyAmount);
            // ������������� �������� ����� ������������ �������
            if (oscillationTween != null)
                oscillationTween.Kill();

            // ������������� ���� ����� ������, ������ ���� AudioSource �������
            if (audioSource != null && audioSource.enabled)
                audioSource.PlayOneShot(coinPickupSound);

            Destroy(gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        SaveScore();
    }

    public void AddMoney(int amount)
    {
        score += amount;
        UpdateScoreText();
        SaveScore();
        OnMoneyUpdated?.Invoke(score);
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        SaveScore();
        OnMoneyUpdated?.Invoke(score);
    }
    public void UpdateScoreText()
    {
        coinText.text = score.ToString() + " ";
    }

    private void SaveScore()
    {
        // ������� ����� ��������� ScoreData � ��������� ������� ����
        ScoreData data = new ScoreData { Score = score };

        // ��������� ������ � YandexGame.savesData.scoreData
        YandexGame.savesData.scoreData = data;

        // ������ ������� ��������� ������
        YandexGame.SaveProgress();
    }

    public void LoadScore()
    {
        // ��������� ������ �� YandexGame.savesData.scoreData
        ScoreData data = YandexGame.savesData.scoreData;

        // ���� ������ ����������, ��������� ����
        if (data != null)
        {
            score = data.Score;
            coinText.text = score.ToString() + " ";
        }
    }
 
}


[System.Serializable]
public class ScoreData
{
    public int Score;
}

 
