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
        public string characterName; // ��� ���������
        public RuntimeAnimatorController animatorController; // ���������� �������� ���������
        public int price; // ���� ���������
        public bool isPurchased; // ����, ������������, ������ �� ��������
        public Button buyButton; // ������ ��� ������� ���������
        public Button selectButton; // ������ ��� ������ ���������
        public Text statusText; // ��������� ���� ��� ����������� ������� ���������
    }
    public List<CharacterInfo> characters;

    private void Start()
    {
        LoadCharactersData();

        // ���������, ���� �� ��������� ���������
        bool hasPurchasedCharacters = characters.Any(c => c.isPurchased);

        // ���� ��� ��������� ����������, �������� � �������� ������� � ������
        if (!hasPurchasedCharacters && characters.Count > 0)
        {
            characters[0].isPurchased = true; // �������� ������� ���������
            SelectCharacter(characters[0]); // �������� ������� ���������
        }

        // ������������� �� ������� ������ ������� � ������ ���������
        foreach (var character in characters)
        {
            // ������� ��������� ����������, ����� ��������� ������� �������� character
            var tempCharacter = character;

            tempCharacter.buyButton.onClick.AddListener(() => BuyCharacter(tempCharacter));
            tempCharacter.selectButton.onClick.AddListener(() => SelectCharacter(tempCharacter));
        }

        // ��������� ������� ����������
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
            // �������� ��������� ��������� �� ������ �����
            MoneyCounter.Instance.AddMoney(-character.price);
            character.isPurchased = true;

            // ��������� ������� ����������
            UpdateCharacterStatus();
            SaveCharactersData();
        }
    }
 
    private void SelectCharacter(CharacterInfo character)
    {
        if (character.isPurchased)
        {
            // ������� ������ Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // �������� ��������� Animator
                Animator playerAnimator = player.GetComponent<Animator>();

                // ���������, ������ �� ��������� Animator
                if (playerAnimator != null)
                {
                    // ������������� ����� ���������� ��������
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

            // ���������� ��� ��������� � ��������������� ��������� ������ ������
            foreach (var c in characters)
            {
                if (c != character && c.selectButton.gameObject.activeSelf == false)
                {
                    c.selectButton.gameObject.SetActive(true);
                }
            }

            // �������� ������ ������ ��� �������� ���������
            character.selectButton.gameObject.SetActive(false);

            // ���������� ����� �������
            character.statusText.gameObject.SetActive(true);
        
            SaveCharactersData();
            // ��������� ������� ����������
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
        // ������� ������ ����� ����������� ����� ������
        YandexGame.savesData.characterData.Clear();

        // ��������� ������ � ������ ��������� � ������
        foreach (var character in characters)
        {
            YandexGame.savesData.characterData.Add(new CharacterData { Name = character.characterName, IsPurchased = character.isPurchased, IsSelected = !character.selectButton.gameObject.activeSelf });
        }

        // ��������� ������
        YandexGame.SaveProgress();
    }

    private void LoadCharactersData()
    {
        // ������������� ������ isPurchased � isSelected � false ��� ���� ���������� ����� ��������� ������
        foreach (var character in characters)
        {
            character.isPurchased = false;
            character.selectButton.gameObject.SetActive(true); // �������� ������ ������ ��� ���� ����������
        }

        // ��������� ������ �� YandexGame.savesData
        foreach (var savedData in YandexGame.savesData.characterData)
        {
            // ���� ��������� � ������� ������
            var character = characters.FirstOrDefault(c => c.characterName == savedData.Name);
            if (character != null)
            {
                // ���� �������� ������, ��������� ��� ������� isPurchased � isSelected
                character.isPurchased = savedData.IsPurchased;
                character.selectButton.gameObject.SetActive(!savedData.IsSelected);
            }
        }

        // �������� ���������, ���� �� ��� ������ �����
        var selectedCharacter = characters.FirstOrDefault(c => !c.selectButton.gameObject.activeSelf);
        if (selectedCharacter != null)
        {
            SelectCharacter(selectedCharacter);
        }

        // ��������� ������� ����������
        UpdateCharacterStatus();
    }
}

[System.Serializable]
public class CharacterData
{
    public string Name;
    public bool IsPurchased;
    public bool IsSelected; // ����� ���� ��� ������������ ���������� ���������
}
