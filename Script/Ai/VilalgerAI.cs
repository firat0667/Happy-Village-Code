using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VilalgerAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject NearestTree;
    public Animator Animator;
    private bool isChopping = false;
    private bool isRunning = false;
    public float choppingDistance = 0.75f;
    public float runningSpeedThreshold = 3.0f;
    public float walkingSpeedThreshold = 2.0f; // Y�r�me animasyonu i�in e�ik h�z
    public float constantSpeed = 5.0f; // Hareket etme sabit h�z�
    public VillagerType VillagerType;
    public GameObject MinerHelmet;
    public GameObject LabrorerBag;
    public GameObject WoodCutterHat;
    private void OnEnable()
    {
        if(VillagerType==VillagerType.Miner)
            MinerHelmet.SetActive(true);
        else MinerHelmet.SetActive(false);
        if (VillagerType == VillagerType.Labrorer)
            LabrorerBag.SetActive(true);
        else LabrorerBag.SetActive(false);
        if (VillagerType == VillagerType.WoodCutter)
            WoodCutterHat.SetActive(true);
        else WoodCutterHat.SetActive(false);
        Destroy(gameObject,600f);
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(VillagerType == VillagerType.WoodCutter)
        VisitNearestTree();
        if (VillagerType == VillagerType.Miner)
         VisitNearestRock();
        if (VillagerType == VillagerType.Labrorer)
            VisitNearestMushroom();
    }

    void Update()
    {
        VillagerTypeMethod(VillagerType);
    }
  public void VillagerTypeMethod(VillagerType type)
    {
        switch (type)
        {
            case VillagerType.WoodCutter:
                WoodCutter();
                break;
            // Di�er durumlar i�in case'leri buraya ekleyebilirsiniz.
            case VillagerType.Farmer:
                Farmer();
                break;
            case VillagerType.Miner:
                RockMiner();
                break;
            case VillagerType.Labrorer:
                Labrorer();
                break;
            default:
                // Belirtilen tip i�in �zel bir i�lem yapmak istemiyorsan�z, burada varsay�lan i�lemi tan�mlayabilirsiniz.
                break;
        }
    }
    private void Labrorer()
    {
        // E�er a�aca ula�t�ysa en yak�ndaki a�aca git
        if (agent.remainingDistance < choppingDistance && !agent.pathPending)
        {
            VisitNearestMushroom();

            if (NearestTree != null)
            {
                float distance = Vector3.Distance(transform.position, NearestTree.transform.position);

                if (!isChopping && distance <= agent.stoppingDistance)
                {
                    StartLootingAnim();
                    StopRunningAnimation();
                    
                }
                else if (distance > agent.stoppingDistance)
                {
                    if (agent.velocity.magnitude > walkingSpeedThreshold)
                    {
                        StartRunningAnimation();
                    }
                    else
                    {
                        StartWalkAnim();
                    }

                    StopLootingAnim();
                }
            }
            else
            {
                StopLootingAnim();
                StopRunningAnimation();

                // Ziyaret edilecek ba�ka a�a� kalmad���nda idle animasyonunu ba�lat
                StartIdleAnimation();
            }
        }
        else if (agent.remainingDistance >= choppingDistance)
        {
            StopLootingAnim();
        }
    }

    private void Farmer()
    {
       
    }

    private void RockMiner()
    {
        // E�er a�aca ula�t�ysa en yak�ndaki a�aca git
        if (agent.remainingDistance < choppingDistance && !agent.pathPending)
        {
            VisitNearestRock();

            if (NearestTree != null)
            {
                float distance = Vector3.Distance(transform.position, NearestTree.transform.position);

                if (!isChopping && distance <= agent.stoppingDistance+.25f)
                {
                    StartDigAnim();
                    StopRunningAnimation();
                }
                else if (distance-0.25f > agent.stoppingDistance)
                {
                    if (agent.velocity.magnitude > walkingSpeedThreshold)
                    {
                        StartRunningAnimation();
                    }
                    else
                    {
                        StartWalkAnim();
                    }

                    StopDigAnim();
                }
            }
            else
            {
                StopDigAnim();
                StopRunningAnimation();

                // Ziyaret edilecek ba�ka a�a� kalmad���nda idle animasyonunu ba�lat
                StartIdleAnimation();
            }
        }
        else if (agent.remainingDistance >= choppingDistance)
        {
            StopDigAnim();
        }
    }

    private void WoodCutter()
    {
        // E�er a�aca ula�t�ysa en yak�ndaki a�aca git
        if (agent.remainingDistance < choppingDistance && !agent.pathPending)
        {
            VisitNearestTree();

            if (NearestTree != null)
            {
                float distance = Vector3.Distance(transform.position, NearestTree.transform.position);

                if (!isChopping && distance <= agent.stoppingDistance)
                {
                    StartCuttingAnimation();
                    StopRunningAnimation();
                    StopWalkAnim();
                }
                else if (distance > agent.stoppingDistance)
                {
                    if (agent.velocity.magnitude > walkingSpeedThreshold)
                    {
                        StartRunningAnimation();
                    }
                    else
                    {
                        StartWalkAnim();
                    }

                    StopCuttingAnimation();
                }
            }
            else
            {
                StopCuttingAnimation();
                StopRunningAnimation();

                // Ziyaret edilecek ba�ka a�a� kalmad���nda idle animasyonunu ba�lat
                StartIdleAnimation();
            }
        }
        else if (agent.remainingDistance >= choppingDistance)
        {
            StopCuttingAnimation();
        }
    }

    void VisitNearestTree()
    {
        NearestTree = FindNearestTree();

        // E�er bulunduysa, o a�aca git
        if (NearestTree != null)
        {
            agent.SetDestination(NearestTree.transform.position);
        }
        else
        {
            Debug.Log("Ziyaret edilecek ba�ka a�a� kalmad�.");
        }
    }
    void VisitNearestRock()
    {
        NearestTree = FindNearestRock();

        // E�er bulunduysa, o a�aca git
        if (NearestTree != null)
        {
            agent.SetDestination(NearestTree.transform.position);
        }
        else
        {
            Debug.Log("Ziyaret edilecek ba�ka a�a� kalmad�.");
        }
    }
    void VisitNearestMushroom()
    {
        NearestTree = FindNearestMushroom();

        // E�er bulunduysa, o a�aca git
        if (NearestTree != null)
        {
            agent.SetDestination(NearestTree.transform.position);
        }
        else
        {
            Debug.Log("Ziyaret edilecek ba�ka a�a� kalmad�.");
        }
    }
    GameObject FindNearestTree()
    {
        GameObject nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (GameObject tree in ResourcesObjectPool.Instance.treeList)
        {
            if (tree.activeSelf)
            {
                float distanceToTree = Vector3.Distance(transform.position, tree.transform.position);

                if (distanceToTree < shortestDistance)
                {
                    nearest = tree.gameObject;
                    shortestDistance = distanceToTree;
                }
            }
        }

        return nearest;
    }
    GameObject FindNearestRock()
    {
        GameObject nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (GameObject tree in ResourcesObjectPool.Instance.rockList)
        {
            if (tree.activeSelf)
            {
                float distanceToTree = Vector3.Distance(transform.position, tree.transform.position);

                if (distanceToTree < shortestDistance)
                {
                    nearest = tree.gameObject;
                    shortestDistance = distanceToTree;
                }
            }
        }

        return nearest;
    }
    GameObject FindNearestMushroom()
    {
        GameObject nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (GameObject tree in ResourcesObjectPool.Instance.flowerList)
        {
            if (tree.activeSelf)
            {
                float distanceToTree = Vector3.Distance(transform.position, tree.transform.position);

                if (distanceToTree < shortestDistance)
                {
                    nearest = tree.gameObject;
                    shortestDistance = distanceToTree;
                }
            }
        }

        return nearest;
    }

    void StartCuttingAnimation()
    {
        Animator.SetLayerWeight(4, 1);
        isChopping = true;
    }

    void StopCuttingAnimation()
    {
        Animator.SetLayerWeight(4, 0);
        isChopping = false;
    }

    void StartRunningAnimation()
    {
        Animator.Play("Run");
        isRunning = true;
    }

    void StopRunningAnimation()
    {
        isRunning = false;
    }

    void StartIdleAnimation()
    {
        // Burada idle animasyonunu ba�latmak i�in gerekli kodu ekleyin
        Animator.Play("Idle");

    }

    void StartWalkAnim()
    {
        Animator.Play("Walking",0,0f);
        isRunning = true;
    }
    void StopWalkAnim()
    {
        isRunning = false;
    }
    public  void StartDigAnim()
    {
        Animator.Play("Dig");
        Animator.SetLayerWeight(6, 1);
    }
    void StopDigAnim()
    {
        Animator.SetLayerWeight(6, 0);
    }
    public void StopLootingAnim()
    {
        Animator.SetLayerWeight(5, 0);
    }
    public void StartLootingAnim()
    {
        Animator.SetLayerWeight(5, 1);
    }
}
