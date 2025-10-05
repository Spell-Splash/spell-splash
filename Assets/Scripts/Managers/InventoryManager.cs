using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("UI slots (left bottom)")]
    public Image[] itemSlots;

    [Header("Possible rewards")]
    public Sprite[] itemIcons;

    public bool TryAddRandomItem()
    {
        // find an empty slot
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].sprite == null)
            {
                Debug.Log("-------------Item filled");
                itemSlots[i].sprite = itemIcons[Random.Range(0, itemIcons.Length)];
                itemSlots[i].gameObject.SetActive(true);
                return true;
            }
        }
        return false; // inventory full
    }
}
