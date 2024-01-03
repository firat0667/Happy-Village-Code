using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static TransactionEffectManager Instance;
    [Header("Elements")]
    [SerializeField] private ParticleSystem _coinPs;
    [SerializeField] private RectTransform _transformCoin;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;
    private int _coinsAmount;
    private Camera _camera;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }
    private void Start()
    {
        _camera = Camera.main;
    }

    [NaughtyAttributes.Button]
    private void PlayCoinParticlesTest()
    {
        PlayCoinParticles(100);
    }
    public void PlayCoinParticles(int amount)
    {
        if (_coinPs.isPlaying)
            return;
        ParticleSystem.Burst burst = _coinPs.emission.GetBurst(0);
        burst.count = amount;
        _coinPs.emission.SetBurst(0, burst);

        ParticleSystem.MainModule main = _coinPs.main;
        main.gravityModifier = 2;

        _coinPs.Play();
        _coinsAmount = amount;
        StartCoroutine(PlayCoinParticlesCoroutine());
    }

    IEnumerator PlayCoinParticlesCoroutine()
    {
        yield return new WaitForSeconds(1);

        ParticleSystem.MainModule main = _coinPs.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_coinsAmount];
        _coinPs.GetParticles(particles);
        Vector3 direction=(_transformCoin.position-_camera.transform.position).normalized;
        Vector3 targetPosition=_camera.transform.position+direction*Vector3.Distance(_camera.transform.position,_coinPs.transform.position);
        while (_coinPs.isPlaying)
        {
            _coinPs.GetParticles(particles);
            for (int i = 0; i < particles.Length;i++)
            {
                if (particles[i].remainingLifetime <= 0)
                    continue;
                particles[i].position = Vector3.MoveTowards(particles[i].position,targetPosition,_moveSpeed*Time.deltaTime);
                if (Vector3.Distance(particles[i].position, targetPosition) < 0.1f)
                {
                    particles[i].position += Vector3.up*100000;
                   // particles[i].remainingLifetime = 0;
                    CashManager.Instance.AddCoins(1);
                }
               
            }
            _coinPs.SetParticles(particles);
            yield return null;
        }
        
    }
}
