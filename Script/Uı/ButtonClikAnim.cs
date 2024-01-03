using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClikAnim : MonoBehaviour
{
    public Image button;
    public Color hoverColor = Color.green;
    public Color Clicker = Color.green;
    public float animationDuration = 0.5f;

    private Color originalColor;
    private bool isHovering = false;

    private void Start()
    {
        // Butonun �st�ndeki Image bile�enini al�n
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }

    public void OnPointerEnter()
    {
      
    }

    public void OnPointerExit()
    {
        if (isHovering)
        {
            Image buttonImage = button.GetComponentInParent<Image>();
            if (buttonImage != null)
            {
                LeanTween.color(buttonImage.rectTransform, originalColor, animationDuration);
                isHovering = false;
            }
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("Butona t�kland�!");
        if (!isHovering)
        {
            Image buttonImage = button.GetComponentInParent<Image>();
            if (buttonImage != null)
            {
                LeanTween.color(buttonImage.rectTransform, Clicker, animationDuration);
                isHovering = true;
            }
        }
    }
}
