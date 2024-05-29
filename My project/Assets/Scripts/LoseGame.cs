using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
public class LoseGame : MonoBehaviour
{
    [SerializeField]
    private GameObject endPanel;

    public void Retry()
    {
        YandexGame.FullscreenShow();
        Health.instance.AddThreeLives();
        GameManager.instance.HideCurrentAndOpenZeroMap();
        GameManager.instance.ResetScore();
        endPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void BuyLife()
    {
        Health.instance.IncreaseHealth();
        MoneyCounter.Instance.UpdateScoreText();
    }
}
