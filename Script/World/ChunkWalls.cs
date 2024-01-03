using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _backWall;
    [SerializeField] private GameObject _leftWall;

    public void Configure(int configuration)
    {
        _frontWall.SetActive(!IsKthBitSet(configuration, 0));
        _rightWall.SetActive(!IsKthBitSet(configuration, 1));
        _backWall.SetActive(!IsKthBitSet(configuration, 2));
        _leftWall.SetActive(!IsKthBitSet(configuration, 3));
    }

    private bool IsKthBitSet(int configuration, int k)
    {
        return (configuration & (1 << k)) != 0;
    }
}
