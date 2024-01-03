using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAiDestinations : MonoBehaviour
{
    public static VillagerAiDestinations Instance;
    public List<Transform> TreeTransform=new List<Transform>();
    public List<Transform> RockTransform=new List<Transform>();
    public List<Transform> MushroomTransform = new List<Transform>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RemoveInactiveObjectsFromList(TreeTransform);
        RemoveInactiveObjectsFromList(RockTransform);
        RemoveInactiveObjectsFromList(MushroomTransform);
    }
    private void RemoveInactiveObjectsFromList(List<Transform> pool)
    {
        pool.RemoveAll(obj => !obj.gameObject.activeSelf);
    }
}
