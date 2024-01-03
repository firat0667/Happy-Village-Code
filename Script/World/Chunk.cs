using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject _unlockedElements;
    [SerializeField] private GameObject _lockedElements;
    [SerializeField] private TextMeshPro _priceText;
    [SerializeField] private MeshFilter _chunkFilter;
    private ChunkWalls _walls;

    [Header("Settings")]
    [SerializeField] private int _initialPrice;
    private int _currentPrice;
    private bool _unlocked;
    private int _configuration;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;
    public WallSkin[] WallSkins;

    private void Awake()
    {
        _walls = GetComponent<ChunkWalls>();
    }

    void Start()
    {
        // Debug.Log(transform.position);
    }

    void Update()
    {

    }

    public void Initialize(int price)
    {
        _currentPrice = price;
        _priceText.text = _currentPrice.ToString();
        if (_currentPrice <= 0)
            Unlock(false);
    }

    public void TryUnlock()
    {
        if (CashManager.Instance.GetCoins() <= 0)
            return;
        _currentPrice--;
        CashManager.Instance.UseCoins(1);
        onPriceChanged?.Invoke();
        _priceText.text = _currentPrice.ToString();
        if (_currentPrice <= 0)
            Unlock();
    }

    private void Unlock(bool triggerAction = true)
    {
        _unlockedElements.SetActive(true);
        _lockedElements.SetActive(false);
        _unlocked = true;

        if (triggerAction)
            onUnlocked?.Invoke();
    }

    public bool IsUnlocked()
    {
        return _unlocked;
    }

    public int GetInitialPrice()
    {
        return _initialPrice;
    }

    public int GetCurrentPrice()
    {
        return _currentPrice;
    }

    public void UpdateWalls(int configuration)
    {
        this._configuration = configuration;
        _walls.Configure(configuration);
        for (int i = 0; i < WallSkins.Length; i++)
        {
            WallSkins[i].Walls();
        }
    }

    public void DisplayLockedElements()
    {
       _lockedElements.SetActive(true) ;
    }

    public void SetRenderer(Mesh mesh)
    {
        _chunkFilter.mesh = mesh;
    }
}
