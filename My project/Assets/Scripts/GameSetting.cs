using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance;
    public GameObject settingPanel;
    public GameObject soundOnObject;
    public GameObject soundOffObject;

    [SerializeField]
    public bool isSoundOn = true;

    void Start()
    {
        // Загрузка сохраненных данных
        LoadAudioData();

        // Установка состояния объектов в зависимости от isSoundOn
        soundOnObject.SetActive(isSoundOn);
        soundOffObject.SetActive(!isSoundOn);
    }

    public void RepeatCheckSound()
    {
        // Проверка включения звука
        if (!isSoundOn)
        {
            // Отключить все звуки
            foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
            {
                audioSource.enabled = false;
            }
        }
    }

    public void ToggleAllSounds()
    {
        // Найти все аудиоисточники в сцене
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // Пройти по каждому аудиоисточнику
        foreach (AudioSource audioSource in allAudioSources)
        {
            // Включить или выключить аудиоисточник в зависимости от текущего состояния
            audioSource.enabled = !isSoundOn;
        }

        // Инвертировать состояние
        isSoundOn = !isSoundOn;

        // Обновить состояние объектов
        soundOnObject.SetActive(isSoundOn);
        soundOffObject.SetActive(!isSoundOn);

        // Сохранить данные об аудио в файл
        SaveAudioData();
        Debug.Log("isSoundOn: " + isSoundOn);
    }

    public void SaveAudioData()
    {
        // Создаем объект AudioData с текущими значениями переменных
        AudioData data = new AudioData
        {
            isSoundOn = isSoundOn,
        };

        // Сохраняем данные в YandexGame.savesData.audioData
        YandexGame.savesData.audioData = data;

        // Теперь остаётся сохранить данные
        YandexGame.SaveProgress();
    }

    public void LoadAudioData()
    {
        // Загружаем данные из YandexGame.savesData.audioData
        AudioData data = YandexGame.savesData.audioData;

        // Если данные существуют, обновляем аудио настройки
        if (data != null)
        {
            isSoundOn = data.isSoundOn;

            // Находим все аудиоисточники в сцене
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

            // Проходим по каждому аудиоисточнику
            foreach (AudioSource audioSource in allAudioSources)
            {
                // Включаем или выключаем аудиоисточник в зависимости от значения isSoundOn
                audioSource.enabled = isSoundOn;
            }
        }
        else
        {
            Debug.LogWarning("Audio data does not exist.");
        }
    }

    public void ShowSetting() => settingPanel.SetActive(true);
    public void CloseSetting() => settingPanel.SetActive(false);

    [Serializable]
    public class AudioData
    {
        public bool isSoundOn;
    }
}
