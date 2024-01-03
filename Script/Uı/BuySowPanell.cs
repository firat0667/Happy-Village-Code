using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySowPanell : MonoBehaviour
{
    public SowData SowData;
    public Image Image;
    public TextMeshProUGUI SeasonName;
    public TextMeshProUGUI Price;
    public Button Buton;
    public Image BgImage;
    private void OnEnable()
    {
        SeasonName.text=SowData.SeasonState.ToString();
        Price.text = SowData.SowPrice9x.ToString();
        Image.sprite = SowData.SowIcon;
    }

    private void Start()
    {
        Buton.onClick.AddListener(BuySow);

    }
    public void BuySow()
    {
        SelectedSow selectedSow = SelectedSow.Instance;
        selectedSow.Data = SowData;
        selectedSow.BaseImage=BgImage;
        for (int i = 0; i < selectedSow.Buttons.Length; i++)
        {
            if (selectedSow.Buttons[i]==BgImage)
                selectedSow.Buttons[i].color = Color.green;
            else
            {
                selectedSow.Buttons[i].color = Color.white;
            }
        }
    }
}
