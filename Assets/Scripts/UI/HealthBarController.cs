using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    public Image fillImage;
    private float maxHP;

    public TMP_Text hpText;

    public void Initialize(float maxHealth)
    {
        maxHP = maxHealth;
        SetHP(maxHealth);
    }

    public void SetHP(float currentHP)
    {
        float fill = Mathf.Clamp01(currentHP / maxHP);
        fillImage.fillAmount = fill;

        if (hpText != null)
        {
            hpText.text = $"{(int)currentHP} / {(int)maxHP}";
        }
    }
}
