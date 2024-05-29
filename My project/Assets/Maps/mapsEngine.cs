using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapsEngine : MonoBehaviour
{
    public static mapsEngine instance;

    [SerializeField]
    private GameObject[] mapsPrefabs; // ������ �������� ����.
    [SerializeField]
    private List<GameObject> activeMaps = new List<GameObject>(); // �������� ����� �� �����.

    private bool[] mapLoaded; // ������ ��� ������������ ��������� �������� ����.

    public int currentMapIndex = 0; // ������ ������� �����.

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // ����������, ��� ���� ������ ���� ��������� mapsEngine.

        // �������������� ������ ��������� �������� ����.
        mapLoaded = new bool[mapsPrefabs.Length];
    }

    void Start()
    {
        // ������� ������ ����� �� ������ ����.
        SpawnMap();
    }

    public void SpawnMap()
    {
        // ������� ����� ����� �� ������� � ��������� �� � �������� �����.
        GameObject newMap = Instantiate(mapsPrefabs[currentMapIndex], Vector3.zero, Quaternion.identity);
        activeMaps.Add(newMap);

        // �������� ����� ��� �����������.
        mapLoaded[currentMapIndex] = true;
    }

    public void ChangeMap(int index)
    {
        // ���������, �� �������� �� �� ��������� �����, ������� ��� �������.
        if (currentMapIndex != index && index < mapsPrefabs.Length)
        {
            // ���������� ������� �����, ���� ��� ����������.
            if (activeMaps.Count > 0)
            {
                Destroy(activeMaps[0]);
                activeMaps.RemoveAt(0);
            }

            // ������������� ����� ����� � ��������� ��������.
            GameObject newMap = Instantiate(mapsPrefabs[index], Vector3.zero, Quaternion.identity);
            activeMaps.Add(newMap);

            // ��������� ������ ������� �����.
            currentMapIndex = index;

            // �������� ����� ��� �����������.
            mapLoaded[index] = true;
        }
        // ���� ������ ����� �� ���������, ������ �� ������.
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
