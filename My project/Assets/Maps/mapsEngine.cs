using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapsEngine : MonoBehaviour
{
    public static mapsEngine instance;

    [SerializeField]
    private GameObject[] mapsPrefabs; // Массив префабов карт.
    [SerializeField]
    private List<GameObject> activeMaps = new List<GameObject>(); // Активные карты на сцене.

    private bool[] mapLoaded; // Массив для отслеживания состояния загрузки карт.

    public int currentMapIndex = 0; // Индекс текущей карты.

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // Убеждаемся, что есть только один экземпляр mapsEngine.

        // Инициализируем массив состояний загрузки карт.
        mapLoaded = new bool[mapsPrefabs.Length];
    }

    void Start()
    {
        // Создаем первую карту на старте игры.
        SpawnMap();
    }

    public void SpawnMap()
    {
        // Создаем новую карту из префаба и добавляем ее в активные карты.
        GameObject newMap = Instantiate(mapsPrefabs[currentMapIndex], Vector3.zero, Quaternion.identity);
        activeMaps.Add(newMap);

        // Помечаем карту как загруженную.
        mapLoaded[currentMapIndex] = true;
    }

    public void ChangeMap(int index)
    {
        // Проверяем, не пытаемся ли мы загрузить карту, которая уже активна.
        if (currentMapIndex != index && index < mapsPrefabs.Length)
        {
            // Уничтожаем текущую карту, если она существует.
            if (activeMaps.Count > 0)
            {
                Destroy(activeMaps[0]);
                activeMaps.RemoveAt(0);
            }

            // Устанавливаем новую карту с указанным индексом.
            GameObject newMap = Instantiate(mapsPrefabs[index], Vector3.zero, Quaternion.identity);
            activeMaps.Add(newMap);

            // Обновляем индекс текущей карты.
            currentMapIndex = index;

            // Помечаем карту как загруженную.
            mapLoaded[index] = true;
        }
        // Если индекс карты не изменился, ничего не делаем.
    }


    public bool IsMapLoaded(int index)
    {
        if (index < mapLoaded.Length)
        {
            return mapLoaded[index];
        }
        return false;
    }
}
