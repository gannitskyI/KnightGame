using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public static Health instance;
    [SerializeField] private int maxHealth = 3; // ������������ ���������� ������
    [SerializeField] private GameObject gameOverWindow; // ������ �� game object ���� ���������
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int healthIncreasePrice; // ��������� ���� ���������� ��������

    private int healthPlayer;

    void Awake()
    {
        instance = this;
        healthPlayer = maxHealth; // ���������� ��������� ���������� ������ ������ �������������
        UpdateLifeUI();
        UpdatePriceUI();
    }

    public void PlayerDied()
    {
        GameManager.instance.RetryPosition();
        GameManager.instance.ScoreDeny();
        GameManager.instance.EnemyDeny();
        LifeDeny();
        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        lifeText.text = healthPlayer.ToString();
    }

    void UpdatePriceUI()
    {
        priceText.text = "+" + healthIncreasePrice.ToString();
    }

    public void LifeDeny()
    {
        healthPlayer--;

        if (healthPlayer <= 0)
        {
            // ���� ������ ������ ��� ����� 0, ���������� ���� ���������
            gameOverWindow.SetActive(true);
            GameManager.instance.UpdateScoreRecord();
            Time.timeScale = 0f;
        }
    }

    public void LifeAdd()
    {
        if (healthPlayer < maxHealth) // ���������, �� ���������� �� ������������ ���������� ������
        {
            healthPlayer++;
            UpdateLifeUI();
        }
    }

    public void AddThreeLives()
    {
        if (healthPlayer <= maxHealth - 3) // ���������, �� ���������� �� ������������ ���������� ������
        {
            healthPlayer += 3;
            UpdateLifeUI();
        }
        else
        {
            // ������� ��������� � ���������� ������������� ���������� ������
            Debug.Log("���������� ������������ ���������� ������!");
        }
    }

    public void IncreaseHealth()
    {
        if (MoneyCounter.score >= healthIncreasePrice)
        {
            if (healthPlayer < maxHealth) // ���������, �� ���������� �� ������������ ���������� ������
            {
                healthPlayer++;
                MoneyCounter.score -= healthIncreasePrice;
                MoneyCounter.Instance.UpdateScoreText();
                UpdateLifeUI();
                gameOverWindow.SetActive(false); // ������ ���� ��������� ����� ������� ���������� ��������

                // ��������� ���� ���������� �������� �� 50
                healthIncreasePrice += 50;
                UpdatePriceUI(); // �������� ����� ���� ���������� ��������
                Time.timeScale = 1;
            }
            else
            {
                // ������� ��������� � ���������� ������������� ���������� ������
                Debug.Log("���������� ������������ ���������� ������!");
            }
        }
        else
        {
            // ������� ��������� � ������������� ���������� ����� ��� ����������� ������ ��������
            Debug.Log("������������ ����� ��� ������� ���������� ��������!");
        }
    }
}
