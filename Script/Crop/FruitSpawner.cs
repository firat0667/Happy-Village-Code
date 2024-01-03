using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [Header("Elements")]
    public GameObject cherryPrefab;
    public CropData CropData;
    public Transform[] spawnPoints;
    public float timeBetweenSpawns = 5f;
    public float collectionTime = 3f;
    public int maxFallenCherries = 5; // Maksimum düþen çilek sayýsý

    private ObjectPool cherryPool;
    private int fallenCherriesCount = 0;
    public GameObject ScaleControl;
    public GameObject Berries;

    private void OnEnable()
    {
        if(ScaleControl.transform.localScale.x>=1)
        Berries.SetActive(true);
        else { Berries.SetActive(false); }
    }
    void Start()
    {
        cherryPool = new ObjectPool(cherryPrefab, spawnPoints.Length);
        StartCoroutine(SpawnCherries());
    }

    IEnumerator SpawnCherries()
    {
        while (true)
        {
            if (fallenCherriesCount < maxFallenCherries)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    Transform spawnPoint = GetRandomSpawnPoint();

                    GameObject cherry = cherryPool.GetObject();
                    cherry.transform.position = spawnPoint.position;

                    cherry.transform.rotation = spawnPoint.rotation;

                    yield return StartCoroutine(GrowCherry(cherry));

                    Rigidbody cherryRb = cherry.GetComponent<Rigidbody>();
                    cherryRb.useGravity = true;

                    yield return new WaitForSeconds(collectionTime);

                    cherryRb.useGravity = false;
                    cherryPool.ReturnObject(cherry);
                }
            }
            else
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
                fallenCherriesCount = 0; // Yeniden baþlat
            }
        }
    }

    Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex];
    }

    IEnumerator GrowCherry(GameObject cherry)
    {
        float timer = 0f;
        Vector3 startScale = Vector3.one * 0.1f;
        Vector3 targetScale = Vector3.one;

        while (timer < CropData.RawFruitTime)
        {
            cherry.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / CropData.RawFruitTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // Çilek toplandýðýnda çaðrýlan bir metod
    public void CherryCollected()
    {
        fallenCherriesCount++;
    }
}

public class ObjectPool
{
    private GameObject prefab;
    private int poolSize;
    private GameObject[] pool;

    public ObjectPool(GameObject prefab, int size)
    {
        this.prefab = prefab;
        this.poolSize = size;
        InitializePool();
    }

    private void InitializePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.transform.parent = null;
        obj.SetActive(false);
    }
}
