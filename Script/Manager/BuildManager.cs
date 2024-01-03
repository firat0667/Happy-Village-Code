using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public Transform Builds;
    private BuildData _buildData;
    public List<BuildData> buildingList = new List<BuildData>();
    private string _savePath;
    public static BuildManager Instance;
    private Dictionary<string, List<int>> prefabGroups = new Dictionary<string, List<int>>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
#if UNITY_ANDROID
        _savePath = Path.Combine(Application.persistentDataPath, "BuildData.txt");
#endif
        LoadWorld();
    }
    public void DeleteSaveData()
    {
        if (File.Exists(_savePath))
        {
            File.Delete(_savePath);
            Debug.Log("Kayýt dosyasý silindi.");
        }
        else
        {
            Debug.Log("Kayýt dosyasý bulunamadý.");
        }
    }
    private void LoadWorld()
    {
        string data = "";
        if (!File.Exists(_savePath))
        {
            FileStream fileStream = new FileStream(_savePath, FileMode.Create);
            _buildData = new BuildData();
            for (int i = 0; i < Builds.childCount; i++)
            {
                Transform building = Builds.GetChild(i);
       
                // Bina adýný alýn ve _buildData sýnýfýna ekleyin
                _buildData.BuildDirNames.Add(building.name);
                // Bina pozisyonunu alýn ve _buildData sýnýfýna ekleyin
                _buildData.BuildPosList.Add(building.position);

                // Bina rotasyonunu alýn ve _buildData sýnýfýna ekleyin
                _buildData.BuildDirList.Add(building.GetComponent<Building>().customPivot.rotation);
            }

            string worldDataString = JsonUtility.ToJson(_buildData, true);
            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);
            fileStream.Write(worldDataBytes);
            fileStream.Close();
        }
        else
        {
            data = File.ReadAllText(_savePath);
            _buildData = JsonUtility.FromJson<BuildData>(data);
            // Prefab objelerin isimleri ile uyumlu olanlarý üretin
            // Ayný isme sahip prefablarý gruplamak için bir sözlük kullanýn
            Dictionary<string, List<int>> prefabGroups = new Dictionary<string, List<int>>();

            for (int i = 0; i < _buildData.BuildDirNames.Count; i++)
            {
                string buildName = _buildData.BuildDirNames[i];

                // Parantez içindekileri ve sonundaki "Clone" ekini kaldýrarak prefab adýný alýn
                string modifiedName = Regex.Replace(buildName, @"\(.+\)$", "");

                if (!prefabGroups.ContainsKey(modifiedName))
                {
                    prefabGroups[modifiedName] = new List<int>();
                }

                prefabGroups[modifiedName].Add(i);
                
            }

            foreach (var kvp in prefabGroups)
            {
                string modifiedName = kvp.Key;
                GameObject prefab = Resources.Load<GameObject>(modifiedName);

                if (prefab != null)
                {
                    foreach (int index in kvp.Value)
                    {
                        GameObject newBuilding = Instantiate(prefab);
                        newBuilding.transform.parent = Builds;
                        newBuilding.transform.position = _buildData.BuildPosList[index];
                        newBuilding.transform.GetComponent<Building>().customPivot.rotation = _buildData.BuildDirList[index];
                        Collider[] childColliders = newBuilding.GetComponentsInChildren<Collider>();

                        // Seçili objenin tüm child collider'larýný geçici olarak devre dýþý býrakýn.
                        foreach (Collider childCollider in childColliders)
                        {
                            childCollider.enabled = true;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Prefab bulunamadý: " + modifiedName);
                }
            }

            if (_buildData.BuildDirNames.Count != Builds.childCount)
                UpdateData();
            Debug.LogWarning(data);


        }
    }
    private void UpdateData()
    {
        int existingBuildCount = _buildData.BuildDirNames.Count;

        if (Builds.childCount > existingBuildCount)
        {
            File.Delete(_savePath);
            for (int i = existingBuildCount; i < Builds.childCount; i++)
            {
                string buildName = Builds.GetChild(i).GetComponent<Building>().name;
                Vector3 buildPos = Builds.GetChild(i).GetComponent<Building>().transform.position;
                Quaternion buildDir = Builds.GetChild(i).GetComponent<Building>().customPivot.rotation;

                _buildData.BuildDirNames.Add(buildName);
                _buildData.BuildPosList.Add(buildPos);
                _buildData.BuildDirList.Add(buildDir);
            }
        }
    }
    public void SaveWorld()
    {
        
        if (_buildData.BuildDirNames.Count != Builds.childCount)
            _buildData = new BuildData();

        for (int i = 0; i < Builds.childCount; i++)
        {
            string buildName = Builds.GetChild(i).GetComponent<Building>().name;
            string modifiedName = Regex.Replace(buildName, @"\(.+\)$", "");
            Vector3 buildPos = Builds.GetChild(i).GetComponent<Building>().transform.position;
            Quaternion buildDir= Builds.GetChild(i).GetComponent<Building>().customPivot.rotation;
            if (_buildData.BuildDirNames.Count > i)
            {
                _buildData.BuildDirNames[i] = modifiedName;
                _buildData.BuildPosList[i]= buildPos;
                _buildData.BuildDirList[i] = buildDir;

            }
                
            else
             _buildData.BuildDirNames.Add(modifiedName);
            _buildData.BuildPosList.Add(buildPos);
            _buildData.BuildDirList.Add(buildDir);
        }

        string data = JsonUtility.ToJson(_buildData, true);
        File.WriteAllText(_savePath, data);
        Debug.LogWarning(data);
    }
    public void DeleteBuilding(GameObject gameObject)
    {

        _buildData.BuildDirNames.Clear();
        _buildData.BuildPosList.Clear();
        _buildData.BuildDirList.Clear();
        gameObject.transform.parent = null;
        Destroy(gameObject);
        SaveWorld();


    }

}
