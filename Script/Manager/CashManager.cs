using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance;
    public int Money;
    // Start is called before the first frame update
    [Header("Settings")]
    private int _coins;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        LoadData();
        UpdateCoinsContainer();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCoins(int coin)
    {
        _coins += coin;
        SoundManager.Instance.CoinSound.Play();
        UpdateCoinsContainer();
        //Debug.Log("We now Have " + _coins + " coins");
        SaveData();
    }
    private void UpdateCoinsContainer()
    {
        GameObject[] coinsContainers = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach ( GameObject coinContainer in coinsContainers)
        {
            coinContainer.GetComponent<TextMeshProUGUI>().text = _coins.ToString();
        }
    }
    private void LoadData()
    {
        
        _coins = PlayerPrefs.GetInt("Coins");
        Money = _coins;
    }
    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", _coins);
        Money = _coins;
    }

    public void UseCoins(int amount)
    {
        AddCoins(-amount);
    }

    public int GetCoins()
    {
        return _coins;
    }
    [NaughtyAttributes.Button]
    public void Add1000Coins()
    {
        AddCoins(1000);
    }
}
