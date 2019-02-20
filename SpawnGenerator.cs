using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject[] propPrefab;
    public int count = 100;
    private BoxCollider area;
    private List<GameObject> props = new List<GameObject>();
    void Start()
    {
        area = GetComponent<BoxCollider>();
        for(int i = 0; i < count; i++)
        {
            Spawn();
        }
        area.enabled = false;

    }

    private void Spawn()
    {
        int selection = Random.Range(0, propPrefab.Length);
        Vector3 spawnPosition = GetRandomPosition();
        
        GameObject instance = Instantiate(propPrefab[selection],spawnPosition, Quaternion.identity);
        props.Add(instance); // 다음 라운드에서 키고 끄기 위해서
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-(size.x / 2f), size.x / 2f);
        float posY = basePosition.y;
        float posZ = basePosition.z + Random.Range(-(size.z / 2f), size.z / 2f);
        Vector3 spawnPos = new Vector3(posX,posY,posZ);
        return spawnPos;
    }

    public void Reset() //GameManager에서 END페이즈시 호출
    {
        for(int i = 0; i <props.Count; i++)
        {
            props[i].transform.position = GetRandomPosition();
            props[i].SetActive(true);
        }
    }
}
