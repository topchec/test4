using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSegmentGenerator : MonoBehaviour
{
    [System.Serializable]
    public class RoomType
    {
        public string name;
        public GameObject[] segmentPrefabs; // Варианты для этого типа комнаты
    }

    public RoomType[] roomTypes;
    public Vector2Int gridSize = new Vector2Int(5, 5);
    public float cellSize = 10f;
    public int[,] roomGrid; // Матрица типов комнат

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        InitializeGrid();
        PlaceRooms();
        ConnectRooms();
    }

    void InitializeGrid()
    {
        roomGrid = new int[gridSize.x, gridSize.y];

        // Заполняем сетку случайными типами комнат
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Пустые клетки по краям
                if (x == 0 || y == 0 || x == gridSize.x - 1 || y == gridSize.y - 1)
                {
                    roomGrid[x, y] = -1; // -1 означает пустую клетку
                }
                else
                {
                    roomGrid[x, y] = Random.Range(0, roomTypes.Length);
                }
            }
        }

        // Гарантируем главную комнату в центре
        roomGrid[gridSize.x / 2, gridSize.y / 2] = 0; // Тип 0 - стартовая комната
    }

    void PlaceRooms()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (roomGrid[x, y] != -1)
                {
                    Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                    SpawnRoom(x, y, position);
                }
            }
        }
    }

    void SpawnRoom(int gridX, int gridY, Vector3 position)
    {
        int roomTypeIndex = roomGrid[gridX, gridY];
        RoomType roomType = roomTypes[roomTypeIndex];

        // Выбираем случайный вариант комнаты
        int variantIndex = Random.Range(0, roomType.segmentPrefabs.Length);
        GameObject room = Instantiate(
            roomType.segmentPrefabs[variantIndex],
            position,
            Quaternion.identity,
            transform // Для иерархии
        );

        room.name = $"Room_{gridX}_{gridY}_{roomType.name}";
    }

    void ConnectRooms()
    {
        // Добавляем коридоры между комнатами
        for (int x = 1; x < gridSize.x - 1; x++)
        {
            for (int y = 1; y < gridSize.y - 1; y++)
            {
                if (roomGrid[x, y] != -1)
                {
                    // Проверяем соседей
                    if (roomGrid[x + 1, y] != -1)
                        SpawnCorridor(x, y, x + 1, y);
                    if (roomGrid[x, y + 1] != -1)
                        SpawnCorridor(x, y, x, y + 1);
                }
            }
        }
    }

    void SpawnCorridor(int fromX, int fromY, int toX, int toY)
    {
        Vector3 fromPos = new Vector3(fromX * cellSize, fromY * cellSize, 0);
        Vector3 toPos = new Vector3(toX * cellSize, toY * cellSize, 0);
        Vector3 corridorPos = (fromPos + toPos) / 2;

        // Создаем коридор (нужен префаб коридора)
        // Instantiate(corridorPrefab, corridorPos, Quaternion.identity);
    }
}
