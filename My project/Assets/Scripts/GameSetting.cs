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
        // �������� ����������� ������
        LoadAudioData();

        // ��������� ��������� �������� � ����������� �� isSoundOn
        soundOnObject.SetActive(isSoundOn);
        soundOffObject.SetActive(!isSoundOn);
    }

    public void RepeatCheckSound()
    {
        // �������� ��������� �����
        if (!isSoundOn)
        {
            // ��������� ��� �����
            foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
            {
                audioSource.enabled = false;
            }
        }
    }

    public void ToggleAllSounds()
    {
        // ����� ��� �������������� � �����
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // ������ �� ������� ��������������
        foreach (AudioSource audioSource in allAudioSources)
        {
            // �������� ��� ��������� ������������� � ����������� �� �������� ���������
            audioSource.enabled = !isSoundOn;
        }

        // ������������� ���������
        isSoundOn = !isSoundOn;

        // �������� ��������� ��������
        soundOnObject.SetActive(isSoundOn);
        soundOffObject.SetActive(!isSoundOn);

        // ��������� ������ �� ����� � ����
        SaveAudioData();
        Debug.Log("isSoundOn: " + isSoundOn);
    }

    public void SaveAudioData()
    {
        // ������� ������ AudioData � �������� ���������� ����������
        AudioData data = new AudioData
        {
            isSoundOn = isSoundOn,
        };

        // ��������� ������ � YandexGame.savesData.audioData
        YandexGame.savesData.audioData = data;

        // ������ ������� ��������� ������
        YandexGame.SaveProgress();
    }

    public void LoadAudioData()
    {
        // ��������� ������ �� YandexGame.savesData.audioData
        AudioData data = YandexGame.savesData.audioData;

        // ���� ������ ����������, ��������� ����� ���������
        if (data != null)
        {
            isSoundOn = data.isSoundOn;

            // ������� ��� �������������� � �����
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

            // �������� �� ������� ��������������
            foreach (AudioSource audioSource in allAudioSources)
            {
                // �������� ��� ��������� ������������� � ����������� �� �������� isSoundOn
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
