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

    public float moveSpeed = 1f; // Скорость перемещения монетки
    public float oscillationDistance = 1f; // Расстояние колебания
   

    private Tween oscillationTween; // Ссылка на твин для перемещения монетки

    public delegate void MoneyUpdatedHandler(int newScore);
    public static event MoneyUpdatedHandler OnMoneyUpdated;

    public AudioClip coinPickupSound; // Аудиоклип для звука сбора монеты
    public AudioSource audioSource; // Аудиоисточник


    private void Start()
    {
        MoneyCounter.Instance = this;
        FindAudioSourceAndTextMeshPro();
        LoadScore();
    }


    private void FindAudioSourceAndTextMeshPro()
    {
        // Находим все объекты AudioSource в сцене
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Ищем AudioSource с именем "AudioSource"
        AudioSource foundAudioSource = audioSources.FirstOrDefault(audio => audio.gameObject.name == "AudioSource");

        // Проверяем, найден ли AudioSource
        if (foundAudioSource != null)
        {
            // Если найден, присваиваем его переменной audioSource
            audioSource = foundAudioSource;
        }
        else
        {
            Debug.LogWarning("AudioSource с именем 'AudioSource' не найден на сцене.");
        }

        // Находим все объекты TextMeshPro в сцене
        TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();

        // Ищем TextMeshPro с именем "MoneyCounter"
        TextMeshProUGUI foundTextMeshPro = textMeshPros.FirstOrDefault(text => text.gameObject.name == "MoneyCounter");

        // Проверяем, найден ли TextMeshPro
        if (foundTextMeshPro != null)
        {
            // Если найден, присваиваем его переменной coinText
            coinText = foundTextMeshPro;
        }
        else
        {
            Debug.LogWarning("TextMeshPro с именем 'MoneyCounter' не найден на сцене.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddMoney(moneyAmount);
            // Останавливаем анимацию перед уничтожением объекта
            if (oscillationTween != null)
                oscillationTween.Kill();

            // Воспроизводим звук сбора монеты, только если AudioSource активен
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
        // Создаем новый экземпляр ScoreData и сохраняем текущий счет
        ScoreData data = new ScoreData { Score = score };

        // Сохраняем данные в YandexGame.savesData.scoreData
        YandexGame.savesData.scoreData = data;

        // Теперь остаётся сохранить данные
        YandexGame.SaveProgress();
    }

    public void LoadScore()
    {
        // Загружаем данные из YandexGame.savesData.scoreData
        ScoreData data = YandexGame.savesData.scoreData;

        // Если данные существуют, обновляем счет
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

 
