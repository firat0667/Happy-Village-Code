using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private enum _chunkShape
    {
        None,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
        Top,
        Right,
        Bottom,
        Left,
        Four
    }
    [Header("Elements")]
    [SerializeField] private Transform _world;
    private Chunk[,] _grid;

    [Header("Settings")]
    [SerializeField] private int _gridSize;
    [SerializeField] private int _gridScale;

    [Header("Data")]
    private WorldData _worldData;
    private string _dataPath;
    private bool _shouldSave;

    [Header("Chunk Meshes")]
    [SerializeField] private Mesh[] _meshShapes;   

    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallBack;
        Chunk.onPriceChanged += ChunkPriceChangedCallBack;
    }

    void Start()
    {
       // _dataPath = Application.dataPath + "/WorldData.txt";
        // PC platformunda kayýt yolu
#if UNITY_EDITOR || UNITY_STANDALONE
       // _dataPath = Path.Combine(Application.dataPath, "WorldData.txt");
#endif

        // Android platformunda kayýt yolu
#if UNITY_ANDROID
        _dataPath = Path.Combine(Application.persistentDataPath, "WorldData.txt");
#endif
      //  _world.GetChild(0).GetComponent<Chunk>().SetRenderer(_meshShapes[(int)_chunkShape.TopRight]);
        LoadWorld();
        Initialize();
        InvokeRepeating("TrySaveGame", 1, 1);
        Debug.Log("Persistent Data Path: " + Application.dataPath);
    }
    public void DeleteSaveData()
    {
        if (File.Exists(_dataPath))
        {
            File.Delete(_dataPath);
            Debug.Log("Kayýt dosyasý silindi.");
        }
        else
        {
            Debug.Log("Kayýt dosyasý bulunamadý.");
        }
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        BuildManager.Instance.DeleteSaveData();
        InventoryManager.Instance.DeleteSaveData();
        SeasonManager.Instance.DeleteSeason();
    }

    private void ChunkPriceChangedCallBack()
    {
        _shouldSave = true;
    }

    private void ChunkUnlockedCallBack()
    {
        Debug.Log("Chunk Unlocked");
        UpdateChunkWall();
        UpdateGridRenderers();
        SaveWorld();
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallBack;
        Chunk.onPriceChanged -= ChunkPriceChangedCallBack;
    }

    private void Initialize()
    {
        for (int i = 0; i < _world.childCount; i++)
        {
            _world.GetChild(i).GetComponent<Chunk>().Initialize(_worldData.chunkPrices[i]);
        }

        InitializeGrid();
        UpdateChunkWall();
        UpdateGridRenderers();
    }

    private void InitializeGrid()
    {
        _grid = new Chunk[_gridSize, _gridSize];
        for (int i = 0; i < _world.childCount; i++)
        {
            Chunk chunk = _world.GetChild(i).GetComponent<Chunk>();
            Vector2Int chunkGridPos = new Vector2Int(
                Mathf.RoundToInt(chunk.transform.position.x / _gridScale),
                Mathf.RoundToInt(chunk.transform.position.z / _gridScale)
            );
            chunkGridPos += new Vector2Int(_gridSize / 2, _gridSize / 2);

            _grid[chunkGridPos.x, chunkGridPos.y] = chunk;
        }
    }

    private void UpdateChunkWall()
    {
        for (int x = 0; x < _gridSize; x++)
        {
            for (int y = 0; y < _gridSize; y++)
            {
                Chunk chunk = _grid[x, y];

                if (chunk == null)
                    continue;

                Chunk frontChunk = GetChunk(x, y + 1) ? _grid[x, y + 1] : null;
                Chunk rightChunk = GetChunk(x + 1, y) ? _grid[x + 1, y] : null;
                Chunk backChunk = GetChunk(x, y - 1) ? _grid[x, y - 1] : null;
                Chunk leftChunk = GetChunk(x - 1, y) ? _grid[x - 1, y] : null;

                int configuration = 0;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    configuration = configuration + 1;

                if (rightChunk != null && rightChunk.IsUnlocked())
                    configuration = configuration + 2;

                if (backChunk != null && backChunk.IsUnlocked())
                    configuration = configuration + 4;

                if (leftChunk != null && leftChunk.IsUnlocked())
                    configuration = configuration + 8;

                chunk.UpdateWalls(configuration);
                SetChunkRenderer(chunk, configuration);
            }
        }
    }

    private void SetChunkRenderer(Chunk chunk, int configuration)
    {
        
        switch (configuration)
        {
            case 0:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.Four]);
                    break;
                case 1:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.Bottom]);
                     break;
            case 2:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.Left]);
                break;
            case 3:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.BottomLeft]);
                break;
            case 4:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.Top]);
                break;
            case 5:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
            case 6:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.TopLeft]);
                break;
            case 7:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
            case 8:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.Right]);
                break;
            case 9:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.BottomRight]);
                break;
            case 10:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
            case 11:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
            case 12:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.TopRight]);
                break;
            case 13:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
            case 14:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
            case 15:
                chunk.SetRenderer(_meshShapes[(int)_chunkShape.None]);
                break;
        }
    }
    private Chunk GetChunk(int x, int y)
    {
        if (x < 0 || x >= _gridSize || y < 0 || y >= _gridSize)
            return null;

        return _grid[x, y];
    }
    private void UpdateGridRenderers()
    {
        for (int x = 0; x <_gridSize; x++)
        {
            for (int y = 0; y <_gridSize; y++)
            {
                Chunk chunk = _grid[x, y];

                if (chunk == null)
                    continue;
                if(chunk.IsUnlocked())
                    continue;

                Chunk frontChunk = GetChunk(x, y + 1) ? _grid[x,y+1]: null;
                Chunk rightChunk = GetChunk(x + 1, y) ? _grid[x+1, y] : null;
                Chunk backChunk = GetChunk(x, y - 1)  ? _grid[x, y - 1] : null;
                Chunk leftChunk = GetChunk(x - 1, y)  ? _grid[x - 1, y] : null;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (rightChunk != null && rightChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (backChunk != null && backChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (leftChunk != null && leftChunk.IsUnlocked())
                    chunk.DisplayLockedElements();

            }
        }
    }

    private void TrySaveGame()
    {
        if (_shouldSave)
        {
            SaveWorld();
            _shouldSave = false;
        }
    }

    private void LoadWorld()
    {
        string data = "";
        if (!File.Exists(_dataPath))
        {
            FileStream fileStream = new FileStream(_dataPath, FileMode.Create);
            _worldData = new WorldData();
            for (int i = 0; i < _world.childCount; i++)
            {
                int chunkInitialPrice = _world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                _worldData.chunkPrices.Add(chunkInitialPrice);
            }

            string worldDataString = JsonUtility.ToJson(_worldData, true);
            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);
            fileStream.Write(worldDataBytes);
            fileStream.Close();
        }
        else
        {
            data = File.ReadAllText(_dataPath);
            _worldData = JsonUtility.FromJson<WorldData>(data);
            if (_worldData.chunkPrices.Count < _world.childCount)
                UpdateData();
        }
    }

    private void UpdateData()
    {
        int missingData = _world.childCount - _worldData.chunkPrices.Count;
        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = _world.childCount - missingData;
            int chunkPrice = _world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            _worldData.chunkPrices.Add(chunkPrice);
        }
    }

    private void SaveWorld()
    {
        if (_worldData.chunkPrices.Count != _world.childCount)
            _worldData = new WorldData();

        for (int i = 0; i < _world.childCount; i++)
        {
            int chunkCurrentPrice = _world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            if (_worldData.chunkPrices.Count > i)
                _worldData.chunkPrices[i] = chunkCurrentPrice;
            else
                _worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(_worldData, true);
        File.WriteAllText(_dataPath, data);
        Debug.LogWarning(data);
    }
}
