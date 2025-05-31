using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public AlphabetButton[] alphabetButtons;
    public DeleteButton deleteButton;
    public UnityEngine.UI.Button attackButton;

    void Update()
    {
        // 1. Letter keys
        foreach (AlphabetButton button in alphabetButtons)
        {
            if (Input.GetKeyDown(button.letter.ToLower()) && button.amount > 0)
            {
                button.OnClick();
            }
        }

        // 2. Backspace → Delete last letter
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            deleteButton.OnClick();
        }

        // 3. Enter → Trigger attack (if interactable)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (attackButton.interactable)
                attackButton.onClick.Invoke();
        }
    }
}
