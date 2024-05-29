 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using YG; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Vector2 playerInitialPosition;

    [SerializeField]
    private GameObject[] enemies;
    private GameObject player;

    [SerializeField]
    private TextMeshProUGUI[] scoreText;
    [SerializeField]
    private TextMeshProUGUI scoreRecordText;

    private int score;
 
    void Awake()
    {
        instance = this;

        Time.timeScale = 1f;
    }

    void Start()
    {
       
        FindEnemies(); // Находим всех врагов при старте игры
        player = GameObject.FindWithTag("Player");
        UpdateScoreUI();
        ResetScore();
    }

    public void PlayerReachedGoal()
    {
        Health.instance.LifeAdd();
        CheckMap();
        // Увеличиваем счет и обновляем UI
        score++;
        
        UpdateScoreUI();
 
        player.transform.position = playerInitialPosition;

        foreach (GameObject g in enemies)
        {
            // Проверяем на null перед доступом к компоненту EnemyScript
            if (g != null)
            {
                EnemyScript enemyScript = g.GetComponent<EnemyScript>();
                if (enemyScript != null)
                {
                    enemyScript.moveSpeed += 1f; // Увеличиваем скорость врага
                }
            }
        }
    }
    public void RetryPosition()
    {
        player.transform.position = playerInitialPosition;
    }

    public void ScoreDeny() 
    {
        if (score > 0)
        {

            score--;
        }
        else
        {

            return;
        }

        UpdateScoreUI();
    }
    public void EnemyDeny() 
    {
        foreach (GameObject g in enemies)
        {

            if (g != null)
            {
                EnemyScript enemyScript = g.GetComponent<EnemyScript>();
                if (enemyScript != null)
                {
                    // Сбрасываем скорость врага
                    enemyScript.moveSpeed = Mathf.Max(4, enemyScript.moveSpeed - 1);
                }
            }
        }
        CheckMap();
    }
   

    private void CheckMap()
    {
        // Проверяем, нужно ли сменить карту
        if (score < 4)
        {
            mapsEngine.instance.ChangeMap(0);
            FindEnemies();
        }
        else if (score < 9)
        {
            mapsEngine.instance.ChangeMap(1);
            FindEnemies();
        }
        else if (score < 14)
        {
            mapsEngine.instance.ChangeMap(2);
            FindEnemies();
        }
        else if (score < 19)
        {
            mapsEngine.instance.ChangeMap(3);
            FindEnemies();
        }
        else if (score < 24)
        {
            mapsEngine.instance.ChangeMap(4);
            FindEnemies();
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Обновление UI счета
    void UpdateScoreUI()
    {
        foreach (TextMeshProUGUI text in scoreText)
        {
            text.text = score.ToString();
        }
    }

    // Метод для нахождения всех врагов
    void FindEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void HideCurrentAndOpenZeroMap()
    {
        mapsEngine.instance.ChangeMap(0);
        FindEnemies();
    }

    public void UpdateScoreRecord()
    {
        // Загрузка данных о рекордном счете
        GameRecordData recordData = LoadGameRecord();

        // Если текущий счет больше рекордного счета, обновляем рекордный счет
        if (score > recordData.scoreRecord)
        {
            recordData.scoreRecord = score;
            YandexGame.NewLeaderboardScores("BestRecord", score);
            SaveGameRecord();
        }

        // Обновление текста рекордного счета на экране
        scoreRecordText.text = recordData.scoreRecord.ToString();
    }

    // Метод для сохранения рекордного счета
    void SaveGameRecord()
    {
        // Создаем новый экземпляр GameRecordData и сохраняем текущий рекордный счет
        GameRecordData recordData = new GameRecordData { scoreRecord = score };

        // Сохраняем данные в YandexGame.savesData.gameRecordData
        YandexGame.savesData.gameRecordData = recordData;

        // Сохраняем данные
        YandexGame.SaveProgress();
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
    // Метод для загрузки рекордного счета
    GameRecordData LoadGameRecord()
    {
        // Загружаем данные из YandexGame.savesData.gameRecordData
        GameRecordData recordData = YandexGame.savesData.gameRecordData;

        // Если данные существуют, возвращаем их
        if (recordData != null)
        {
            return recordData;
        }
        else
        {
            // Если данные не существуют, возвращаем новый объект GameRecordData
            return new GameRecordData();
        }
    }

}
[System.Serializable]
public class GameRecordData
{
    public int scoreRecord;
}