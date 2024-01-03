using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpriteProbability
{
    public Sprite Sprite;
    [Range(0f, 1f)]
    public float Probability;
    public int MinAmount; // Minimum çýkma miktarý
    public int MaxAmount; // Maximum çýkma miktarý

    public int GetRandomAmount()
    {
        // Rastgele bir miktar deðeri üret
        return Random.Range(MinAmount, MaxAmount + 1); // +1 çünkü maxAmount da dahil edilsin istiyoruz
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
    public float TotalDuration = 30f; // Toplam süre

    private Coroutine spawnCoroutine;
    private GameObject lastResultImage; // En son çýkan objeyi saklamak için deðiþken

    private void Start()
    {
        openButton.onClick.AddListener(OpenCase);

        // SpawnObjects coroutine'unu baþlat
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
            string result = "Çýkan kaynaklar:\n";

            for (int i = 0; i < numberOfResources; i++)
            {
                Sprite selectedSprite = GetRandomSprite();

                if (selectedSprite != null)
                {
                    resultImage.sprite = selectedSprite;
                }

                // En son çýkan objeyi sakla
                lastResultImage = resultImage.gameObject;

                // Gösterme süresi zamanla artacak
                float displayTime = Mathf.Lerp(InitialDisplayTime, MaxDisplayTime, elapsedTime / TotalDuration);

                // Belirli bir süre boyunca gösterdikten sonra silme
                yield return new WaitForSeconds(displayTime);
            }

            resultText.text = result;

            // Toplam süreyi güncelle
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
            // Önceki spawn iþlemi tamamlanmadan yeni spawn iþlemine baþlanmamasý için bekleme
            yield return new WaitUntil(() => spawnCoroutine == null);

            // SpawnObjects coroutine'unun içine spawn iþlemlerini ekle
            spawnCoroutine = StartCoroutine(ShowResources());

            // Belirli bir süre bekleyerek spawn iþlemleri arasýnda uyum saðlanmasý
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
