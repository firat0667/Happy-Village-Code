using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesObjectPool : MonoBehaviour
{
    public static ResourcesObjectPool Instance;

    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject flowerPrefab;

    public int treePoolSize = 20;
    public int rockPoolSize = 20;
    public int flowerPoolSize = 20;

    public float spawnInterval = 5f;
    public float MinDistanceBetweenTrees = 2.0f;
    public float Increase = 5f;

    public Transform[] spawnAreas; // Birden fazla spawn b�lgesi

    private Queue<GameObject> treeQueue = new Queue<GameObject>();
    private Queue<GameObject> rockQueue = new Queue<GameObject>();
    private Queue<GameObject> mushroomQueue = new Queue<GameObject>();
    public List<GameObject> treeList = new List<GameObject>();
    public List<GameObject> rockList = new List<GameObject>();
    public List<GameObject> flowerList = new List<GameObject>();
    private bool _isPositionActive = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializePool(treePrefab, treeQueue,treeList, treePoolSize);
        InitializePool(rockPrefab, rockQueue,rockList, rockPoolSize);
        InitializePool(flowerPrefab, mushroomQueue,flowerList,flowerPoolSize);

        if (_isPositionActive)
            StartCoroutine(SpawnObjects());
    }

    private void Update()
    {
    }

    public void FindGroundObjects()
    {
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        spawnAreas = new Transform[groundObjects.Length];

        for (int i = 0; i < groundObjects.Length; i++)
        {
            spawnAreas[i] = groundObjects[i].transform;
        }
    }

    private void InitializePool(GameObject prefab, Queue<GameObject> queue, List<GameObject> list, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
            list.Add(obj);
        }
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
                yield return new WaitForSeconds(spawnInterval);
                FindGroundObjects();

                // E�er kare alan�nda maksimum a�a� say�s�na ula��lmam��sa spawn et
                if (CountActiveObjects(treeList) < spawnAreas.Length &&!BuildingGrid.Instance.Isshop)
                {
                    SpawnObject(treePrefab, treeQueue,treeList);
                }

                // E�er kare alan�nda maksimum kaya say�s�na ula��lmam��sa spawn et
                if (CountActiveObjects(rockList) < spawnAreas.Length && !BuildingGrid.Instance.Isshop)
                {
                    SpawnObject(rockPrefab, rockQueue, rockList);
                    // rockList listesindeki aktif objelerin transform'lar�n� RockTransform listesine ekler
                    VillagerAiDestinations.Instance.RockTransform.AddRange(rockList
                .Where(obj => obj.activeSelf)
               .Select(obj => obj.transform)
                .Where(transform => !VillagerAiDestinations.Instance.RockTransform.Contains(transform)));
                }
                // E�er kare alan�nda maksimum mantar say�s�na ula��lmam��sa spawn et
                if (CountActiveObjects(flowerList) < spawnAreas.Length && SeasonManager.Instance.SeasonStateMain != SeasonState.Winter && !BuildingGrid.Instance.Isshop)
                {
                    SpawnObject(flowerPrefab, mushroomQueue,flowerList);
                    VillagerAiDestinations.Instance.MushroomTransform.AddRange(flowerList
           .Where(obj => obj.activeSelf)
          .Select(obj => obj.transform)
           .Where(transform => !VillagerAiDestinations.Instance.MushroomTransform.Contains(transform)));

                }
        }
    }

    private void SpawnObject(GameObject prefab, Queue<GameObject> queue, List<GameObject> list)
    {
        GameObject obj = GetObjectFromQueue(queue, list);
        if (obj != null)
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            obj.transform.position = randomPosition;
            obj.SetActive(true);
        }
    }

    private GameObject GetObjectFromQueue(Queue<GameObject> queue, List<GameObject> list)
    {
        // Kuyruktan uygun bir obje bulunamazsa, listenin ba��na d�n
        if (queue.Count == 0)
        {
            foreach (var obj2 in list)
            {
                obj2.SetActive(false); // Nesnenin durumunu s�f�rla
                queue.Enqueue(obj2);
            }
        }

        GameObject obj = queue.Dequeue();
        if (!obj.activeSelf)
        {
            return obj;
        }

        return null;
    }

    private int CountActiveObjects(List<GameObject> list)
    {
        int count = 0;
        foreach (var obj in list)
        {
            if (obj.activeSelf)
            {
                count++;
            }
        }
        return count;
    }

    private bool IsClearPosition(Vector3 position, Queue<GameObject> queue)
    {
        foreach (var obj in queue)
        {
            if (obj.activeSelf)
            {
                float distance = Vector3.Distance(position, obj.transform.position);
                if (distance < MinDistanceBetweenTrees)
                {
                    return false; // E�er mesafe yeterince uzak de�ilse, pozisyon kullan�lamaz.
                }
            }
        }

        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                return false; // E�er ba�ka bir obje ile �ak��ma varsa ve bu obje "Ground" d���ndaysa, pozisyon kullan�lamaz.
            }
        }

        return true; // Pozisyon kullan�labilir.
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Transform randomSpawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

        Vector3 randomPosition = Vector3.zero;
        for (int i = 0; i < 100; i++) // Rastgele bir konum bulmak i�in 100 kez deneyin
        {
            // Rastgele bir pozisyon hesapla
            randomPosition = randomSpawnArea.position + new Vector3(
                Random.Range(-(randomSpawnArea.localScale.x * Increase) / 2f, (randomSpawnArea.localScale.x * Increase) / 2f),
                0.35f,
                Random.Range(-(randomSpawnArea.localScale.z * Increase) / 2f, (randomSpawnArea.localScale.z * Increase) / 2f)
            );

            // Minimum mesafeyi sa�lamak i�in kontrol ekleyin
            bool isClear = true;
            // T�m kuyruklar� kontrol et
            isClear &= IsClearPosition(randomPosition, treeQueue);
            isClear &= IsClearPosition(randomPosition, rockQueue);
            isClear &= IsClearPosition(randomPosition, mushroomQueue);

            // E�er bu konum temizse, bu konumu kullan
            if (isClear)
            {
                _isPositionActive = true;
                return randomPosition;
            }
        }

        // E�er 100 denemede uygun bir konum bulunamazsa, (0,0,0) noktas�na d�nd�r�n.
        _isPositionActive = false;
        return Vector3.zero;
    }
}
