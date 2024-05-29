using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using System.Linq;

public class ShopKnight : MonoBehaviour
{
    [SerializeField]
    private GameObject shopWindow;

    [System.Serializable]
    public class CharacterInfo
    {
        public string characterName; // Имя персонажа
        public RuntimeAnimatorController animatorController; // Контроллер анимации персонажа
        public int price; // Цена персонажа
        public bool isPurchased; // Флаг, показывающий, куплен ли персонаж
        public Button buyButton; // Кнопка для покупки персонажа
        public Button selectButton; // Кнопка для выбора персонажа
        public Text statusText; // Текстовое поле для отображения статуса персонажа
    }
    public List<CharacterInfo> characters;

    private void Start()
    {
        LoadCharactersData();

        // Проверяем, есть ли купленные персонажи
        bool hasPurchasedCharacters = characters.Any(c => c.isPurchased);

        // Если нет купленных персонажей, покупаем и выбираем первого в списке
        if (!hasPurchasedCharacters && characters.Count > 0)
        {
            characters[0].isPurchased = true; // Покупаем первого персонажа
            SelectCharacter(characters[0]); // Выбираем первого персонажа
        }

        // Подписываемся на события кнопок покупки и выбора персонажа
        foreach (var character in characters)
        {
            // Создаем временную переменную, чтобы захватить текущее значение character
            var tempCharacter = character;

            tempCharacter.buyButton.onClick.AddListener(() => BuyCharacter(tempCharacter));
            tempCharacter.selectButton.onClick.AddListener(() => SelectCharacter(tempCharacter));
        }

        // Обновляем статусы персонажей
        UpdateCharacterStatus();
    }
 
    public void ShowShop()
    { 
        shopWindow.SetActive(true);
        
    }

    public void CloseShop()
    { 
        shopWindow.SetActive(false);  
    }

    private void BuyCharacter(CharacterInfo character)
    {
        if (!character.isPurchased && MoneyCounter.score >= character.price)
        {
            // Вычитаем стоимость персонажа из общего счета
            MoneyCounter.Instance.AddMoney(-character.price);
            character.isPurchased = true;

            // Обновляем статусы персонажей
            UpdateCharacterStatus();
            SaveCharactersData();
        }
    }
 
    private void SelectCharacter(CharacterInfo character)
    {
        if (character.isPurchased)
        {
            // Находим объект Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // Получаем компонент Animator
                Animator playerAnimator = player.GetComponent<Animator>();

                // Проверяем, найден ли компонент Animator
                if (playerAnimator != null)
                {
                    // Устанавливаем новый контроллер анимации
                    playerAnimator.runtimeAnimatorController = character.animatorController;
                }
                else
                {
                    Debug.LogWarning("Animator component not found on the Player object.");
                }
            }
            else
            {
                Debug.LogWarning("Player object not found.");
            }

            // Перебираем все персонажи и восстанавливаем видимость кнопки выбора
            foreach (var c in characters)
            {
                if (c != character && c.selectButton.gameObject.activeSelf == false)
                {
                    c.selectButton.gameObject.SetActive(true);
                }
            }

            // Скрываем кнопку выбора для текущего персонажа
            character.selectButton.gameObject.SetActive(false);

            // Показываем текст статуса
            character.statusText.gameObject.SetActive(true);
        
            SaveCharactersData();
            // Обновляем статусы персонажей
            UpdateCharacterStatus();
        }
    }
 
    private void UpdateCharacterStatus()
    {
        foreach (var character in characters)
        {
            
           // character.selectButton.gameObject.SetActive(character.isPurchased);
            character.buyButton.gameObject.SetActive(!character.isPurchased);
        }
    }
    private void SaveCharactersData()
    {
        // Очищаем список перед добавлением новых данных
        YandexGame.savesData.characterData.Clear();

        // Добавляем данные о каждом персонаже в список
        foreach (var character in characters)
        {
            YandexGame.savesData.characterData.Add(new CharacterData { Name = character.characterName, IsPurchased = character.isPurchased, IsSelected = !character.selectButton.gameObject.activeSelf });
        }

        // Сохраняем данные
        YandexGame.SaveProgress();
    }

    private void LoadCharactersData()
    {
        // Устанавливаем статус isPurchased и isSelected в false для всех персонажей перед загрузкой данных
        foreach (var character in characters)
        {
            character.isPurchased = false;
            character.selectButton.gameObject.SetActive(true); // Включаем кнопку выбора для всех персонажей
        }

        // Загружаем данные из YandexGame.savesData
        foreach (var savedData in YandexGame.savesData.characterData)
        {
            // Ищем персонажа в текущем списке
            var character = characters.FirstOrDefault(c => c.characterName == savedData.Name);
            if (character != null)
            {
                // Если персонаж найден, обновляем его статусы isPurchased и isSelected
                character.isPurchased = savedData.IsPurchased;
                character.selectButton.gameObject.SetActive(!savedData.IsSelected);
            }
        }

        // Выбираем персонажа, если он был выбран ранее
        var selectedCharacter = characters.FirstOrDefault(c => !c.selectButton.gameObject.activeSelf);
        if (selectedCharacter != null)
        {
            SelectCharacter(selectedCharacter);
        }

        // Обновляем статусы персонажей
        UpdateCharacterStatus();
    }
}

[System.Serializable]
public class CharacterData
{
    public string Name;
    public bool IsPurchased;
    public bool IsSelected; // Новое поле для отслеживания выбранного персонажа
}
