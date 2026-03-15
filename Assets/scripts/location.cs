using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class location : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public GameObject[] roomPrefabs0;
    public GameObject[] roomPrefabs1;
    public GameObject[] roomPrefabs2;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        SpawnLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnLocation()
    {
        int randomIndex = Random.Range(0, roomPrefabs.Length);

        if (randomIndex == 0)
        {
            spawn0();
           
        }
        else if (randomIndex == 1)
        {
            spawn1();

        }
        else if (randomIndex == 2)
        {
             spawn2();
        }
    }
    void spawn0()
    {
        Vector2 spawnPosition = new Vector2(23.74f, -8.64f);
        GameObject newLocation = Instantiate(roomPrefabs[0], spawnPosition, Quaternion.identity);
        int randomIndex0 = Random.Range(0, roomPrefabs0.Length);
        if (randomIndex0 == 0)
        {
            Vector2 spawnPosition1 = new Vector2(171.51f,-35.51f);
            GameObject newLocatio1 = Instantiate(roomPrefabs0[0], spawnPosition1, Quaternion.identity);
            Vector2 spawnPosition2 = new Vector2(39.9f,11.47f);
            GameObject newLocation2 = Instantiate(roomPrefabs0[1], spawnPosition2, Quaternion.identity);
        }
        else if (randomIndex0 == 1)
        {
            Vector2 spawnPosition1 = new Vector2(-31.81f,12.99f);
            GameObject newLocation1 = Instantiate(roomPrefabs0[1], spawnPosition1, Quaternion.identity);
            Vector2 spawnPosition2 = new Vector2(242.55f,-21.76f);
            GameObject newLocation2 = Instantiate(roomPrefabs0[0], spawnPosition2, Quaternion.identity);
        }
    }
    void spawn1()
    {
        Vector2 spawnPosition = new Vector2(95.5f, -54.76f);
        GameObject newLocation = Instantiate(roomPrefabs[1], spawnPosition, Quaternion.identity);
        int randomIndex0 = Random.Range(0, roomPrefabs1.Length);
        if (randomIndex0 == 0)
        {
            Vector2 spawnPosition1 = new Vector2(95.39f,-10.17f);
            GameObject newLocation1 = Instantiate(roomPrefabs1[0], spawnPosition1, Quaternion.identity);
            Vector2 spawnPosition2 = new Vector2(39.84f,11.45f);
            GameObject newLocation2 = Instantiate(roomPrefabs1[1], spawnPosition2, Quaternion.identity);
        }
        else if (randomIndex0 == 1)
        {
            Vector2 spawnPosition1 = new Vector2(-36.1f,-7.76f);
            GameObject newLocation1 = Instantiate(roomPrefabs1[1], spawnPosition1, Quaternion.identity);
            Vector2 spawnPosition2 = new Vector2(166.54f,3.63f);
            GameObject newLocation2 = Instantiate(roomPrefabs1[0], spawnPosition2, Quaternion.identity);
        }
    }
    void spawn2()
    {
        Vector2 spawnPosition = new Vector2(-107.76f, -6.27f);
        GameObject newLocation = Instantiate(roomPrefabs[2], spawnPosition, Quaternion.identity);
        int randomIndex0 = Random.Range(0, roomPrefabs2.Length);
        if (randomIndex0 == 0)
        {
            Vector2 spawnPosition1 = new Vector2(94.93f,5.11f);
            GameObject newLocation1 = Instantiate(roomPrefabs2[0], spawnPosition1, Quaternion.identity);
            Vector2 spawnPosition2 = new Vector2(242.7f,-21.76f);
            GameObject newLocation2 = Instantiate(roomPrefabs2[1], spawnPosition2, Quaternion.identity);
        }
        else if (randomIndex0 == 1)
        {
            Vector2 spawnPosition1 = new Vector2(166.7f, -41.02f);
            GameObject newLocatio1 = Instantiate(roomPrefabs2[1], spawnPosition1, Quaternion.identity);
            Vector2 spawnPosition2 = new Vector2(166.5f,3.58f);
            GameObject newLocation2 = Instantiate(roomPrefabs2[0], spawnPosition2, Quaternion.identity);
        }
    }

}