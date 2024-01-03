using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpriteProbability
{
    public Sprite Sprite;
    [Range(0f, 1f)]
    public float Probability;
    public int MinAmount; // Minimum ��kma miktar�
    public int MaxAmount; // Maximum ��kma miktar�

    public int GetRandomAmount()
    {
        // Rastgele bir miktar de�eri �ret
        return Random.Range(MinAmount, MaxAmount + 1); // +1 ��nk� maxAmount da dahil edilsin istiyoruz
    }
}

public class CaseOpeningUI : MonoBehaviour
{
    public Image resultImage; // Var olan Image nesnesi
    public Text resultText;
    public Button openButton;
    public SpriteProbability[] spriteProbabilities;

    public int MinResourceAmount = 1;
    public int MaxResourceAmount = 5;
    public float SpawnInterval = 5f;
    public float InitialDisplayTime = 0.1f;
    public float MaxDisplayTime = 1f;
    public float ImageSpacing = 10f;
    public float TotalDuration = 30f; // Toplam s�re

    private Coroutine spawnCoroutine;
    private GameObject lastResultImage; // En son ��kan objeyi saklamak i�in de�i�ken

    private void Start()
    {
        openButton.onClick.AddListener(OpenCase);

        // SpawnObjects coroutine'unu ba�lat
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    private void OpenCase()
    {
        StartCoroutine(ShowResources());
    }

    private IEnumerator ShowResources()
    {
        float elapsedTime = 0f;

        while (elapsedTime < TotalDuration)
        {
            int numberOfResources = Random.Range(MinResourceAmount, MaxResourceAmount + 1);
            string result = "��kan kaynaklar:\n";

            for (int i = 0; i < numberOfResources; i++)
            {
                Sprite selectedSprite = GetRandomSprite();

                if (selectedSprite != null)
                {
                    resultImage.sprite = selectedSprite;
                }

                // En son ��kan objeyi sakla
                lastResultImage = resultImage.gameObject;

                // G�sterme s�resi zamanla artacak
                float displayTime = Mathf.Lerp(InitialDisplayTime, MaxDisplayTime, elapsedTime / TotalDuration);

                // Belirli bir s�re boyunca g�sterdikten sonra silme
                yield return new WaitForSeconds(displayTime);
            }

            resultText.text = result;

            // Toplam s�reyi g�ncelle
            elapsedTime += TotalDuration;
        }
    }

    private Sprite GetRandomSprite()
    {
        float totalProbability = 0f;
        foreach (var spriteProbability in spriteProbabilities)
        {
            totalProbability += spriteProbability.Probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (var spriteProbability in spriteProbabilities)
        {
            cumulativeProbability += spriteProbability.Probability;
            if (randomValue <= cumulativeProbability)
            {
                int amount = spriteProbability.GetRandomAmount();
                return spriteProbability.Sprite;
            }
        }

        return null;
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // �nceki spawn i�lemi tamamlanmadan yeni spawn i�lemine ba�lanmamas� i�in bekleme
            yield return new WaitUntil(() => spawnCoroutine == null);

            // SpawnObjects coroutine'unun i�ine spawn i�lemlerini ekle
            spawnCoroutine = StartCoroutine(ShowResources());

            // Belirli bir s�re bekleyerek spawn i�lemleri aras�nda uyum sa�lanmas�
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
