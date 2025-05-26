using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public void OnClick()
    {
        WordBuilder.Instance.DeleteLastLetter();
    }
}
