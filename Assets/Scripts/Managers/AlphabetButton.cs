using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetButton : MonoBehaviour
{
    public string letter;
    public int amount = 0;
    public TMP_Text amountText;

    public void SetAmount(int newAmount)
    {
        amount = newAmount;
        amountText.text = amount.ToString();
    }

    public bool TryUseLetter()
    {
        if (amount > 0)
        {
            amount--;
            amountText.text = amount.ToString();
            return true;
        }
        return false;
    }

    public void OnClick()
    {
        WordBuilder.Instance.TryAddLetterFromButton(this);
    }
}
