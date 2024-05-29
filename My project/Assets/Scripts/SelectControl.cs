using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SelectControl : MonoBehaviour
{
    [SerializeField]
    public GameObject mobileControl;

    [SerializeField]
    public GameObject selectPanel;

    private bool isFirstTime;
    [SerializeField]
    private bool isMovileInput;

    private void Awake()
    {
        OnBrauserQuit();
    }
    void Start()
    {

        TutorialPlayer.instance.GetData();
        // Загружаем данные из сохранений
        SavesYG savesData = YandexGame.savesData;
        bool isFirstTime = savesData.isFirstTime;

        if (isFirstTime)
        {
            selectPanel.SetActive(false);
        }
        if (savesData.isMovileInput)
        {
            mobileControl.SetActive(true);
        }
    }
    public void GetData()
    {   
        SavesYG savesData = YandexGame.savesData;
 
        bool isFirstTime = savesData.isFirstTime;
        
        if (!isFirstTime)
        {
            Time.timeScale = 0;
            selectPanel.SetActive(true);
            YandexGame.savesData.isFirstTime = true;
            YandexGame.SaveProgress();
        }
        else
        {
            selectPanel.SetActive(false);
        }
    }
 
    private void OnBrauserQuit()
    {
        // При выходе из игры устанавливаем переменную isFirstTime в false
        YandexGame.savesData.isFirstTime = false;
        YandexGame.savesData.isMovileInput = false;
        // Сохраняем данные
        YandexGame.SaveProgress();
    }

    public void PCControl()
    {
        Time.timeScale = 1;
        mobileControl.SetActive(false);
        selectPanel.SetActive(false);
        TutorialPlayer.instance.GetData();
        
        YandexGame.savesData.isFirstTime = true;
        YandexGame.SaveProgress();
    }

    public void MobileControl()
    {
        Time.timeScale = 1;
        YandexGame.savesData.isMovileInput = true;

        YandexGame.SaveProgress();
        mobileControl.SetActive(true);
        selectPanel.SetActive(false);
        TutorialPlayer.instance.GetData();
        YandexGame.savesData.isFirstTime = true;
        YandexGame.SaveProgress();
    }
}
