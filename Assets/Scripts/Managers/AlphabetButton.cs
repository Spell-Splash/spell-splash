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

        GetComponent<Button>().interactable = (amount > 0);
    }

    public bool TryUseLetter()
    {
        if (amount > 0)
        {
            SetAmount(amount - 1);
            return true;
        }
        return false;
    }

    public void OnClick()
    {
        WordBuilder.Instance.TryAddLetterFromButton(this);
    }
}
