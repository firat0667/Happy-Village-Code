using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectPool : MonoBehaviour
{
    public static EffectObjectPool Instance;
    public GameObject particlePrefab; // Klonlamak istediðiniz parçacýk nesnesi
    public int poolSize = 10; // Nesne havuzunun boyutu
    private Queue<GameObject> objectQueue; // Nesne havuzu kuyruðu
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Nesne havuzu kuyruðunu oluþturun
        objectQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.SetActive(false); // Nesneyi etkisiz hale getir
            objectQueue.Enqueue(particle);
        }
    }

    public void PlayParticle(Vector3 position, Quaternion rotation)
    {
        // Nesne havuzu kuyruðundan etkisiz bir nesne alýn
        GameObject particle = GetInactiveObject();

        if (particle != null)
        {
            // Etkisiz nesneyi etkinleþtirin
            particle.transform.position = position;
            particle.transform.rotation = rotation;
            particle.SetActive(true);
        }
    }

    private GameObject GetInactiveObject()
    {
        // Nesne havuzu kuyruðundan etkisiz bir nesne alýn
        while (objectQueue.Count > 0)
        {
            GameObject particle = objectQueue.Dequeue();
            if (!particle.activeInHierarchy)
            {
                return particle;
            }
        }
        return null; // Etkisiz nesne bulunamadý
    }
}
