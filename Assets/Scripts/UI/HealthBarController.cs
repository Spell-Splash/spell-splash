using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image fillImage;
    private float maxHP;

    public void Initialize(float maxHealth)
    {
        maxHP = maxHealth;
        SetHP(maxHealth);
    }

    public void SetHP(float currentHP)
    {
        float fill = Mathf.Clamp01(currentHP / maxHP);
        fillImage.fillAmount = fill;
    }
}
