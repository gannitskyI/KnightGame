using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public static Health instance;
    [SerializeField] private int maxHealth = 3; // ћаксимальное количество жизней
    [SerializeField] private GameObject gameOverWindow; // —сылка на game object окна проигрыша
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int healthIncreasePrice; // начальна€ цена увеличени€ здоровь€

    private int healthPlayer;

    void Awake()
    {
        instance = this;
        healthPlayer = maxHealth; // ”становить начальное количество жизней равным максимальному
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
            // ≈сли жизней меньше или равно 0, активируем окно проигрыша
            gameOverWindow.SetActive(true);
            GameManager.instance.UpdateScoreRecord();
            Time.timeScale = 0f;
        }
    }

    public void LifeAdd()
    {
        if (healthPlayer < maxHealth) // ѕроверить, не достигнуто ли максимальное количество жизней
        {
            healthPlayer++;
            UpdateLifeUI();
        }
    }

    public void AddThreeLives()
    {
        if (healthPlayer <= maxHealth - 3) // ѕроверить, не достигнуто ли максимальное количество жизней
        {
            healthPlayer += 3;
            UpdateLifeUI();
        }
        else
        {
            // ¬ывести сообщение о достижении максимального количества жизней
            Debug.Log("ƒостигнуто максимальное количество жизней!");
        }
    }

    public void IncreaseHealth()
    {
        if (MoneyCounter.score >= healthIncreasePrice)
        {
            if (healthPlayer < maxHealth) // ѕроверить, не достигнуто ли максимальное количество жизней
            {
                healthPlayer++;
                MoneyCounter.score -= healthIncreasePrice;
                MoneyCounter.Instance.UpdateScoreText();
                UpdateLifeUI();
                gameOverWindow.SetActive(false); // —крыть окно проигрыша после покупки увеличени€ здоровь€

                // ”величить цену увеличени€ здоровь€ на 50
                healthIncreasePrice += 50;
                UpdatePriceUI(); // ќбновить текст цены увеличени€ здоровь€
                Time.timeScale = 1;
            }
            else
            {
                // ¬ывести сообщение о достижении максимального количества жизней
                Debug.Log("ƒостигнуто максимальное количество жизней!");
            }
        }
        else
        {
            // ¬ывести сообщение о недостаточном количестве монет или предприн€ть другие действи€
            Debug.Log("Ќедостаточно монет дл€ покупки увеличени€ здоровь€!");
        }
    }
}
