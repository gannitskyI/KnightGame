using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class Reward : MonoBehaviour
{ 
    [SerializeField]

    private int score;
       
    // ������������� �� ������� �������� ������� � OnEnable
    private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;

    // ������������ �� ������� �������� ������� � OnDisable
    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

    // ����������� ����� ��������� �������
    void Rewarded(int id)
    {
        MoneyCounter.Instance.AddScore(100);
    }

    // ����� ��� ������ ����� �������
    public void OpenRewardAd(int id)
    {
        // �������� ����� �������� ����� �������
        YandexGame.RewVideoShow(id);
    }
}
