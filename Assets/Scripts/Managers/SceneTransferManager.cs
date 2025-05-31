using UnityEngine;

public class SceneTransferManager : MonoBehaviour
{
    public static SceneTransferManager Instance;
    public Vector3 returnPosition;
    public Vector3 returnCameraPosition;

    public string returnScene;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveReturnPoint(Vector3 playerPosition, string sceneName, Vector3 cameraPosition)
    {
        returnPosition = playerPosition;
        returnScene = sceneName;
        returnCameraPosition = cameraPosition;
        IsReturningFromGuild = true;
    }


    public bool IsReturningFromGuild { get; private set; }

    public void ClearReturnFlag()
    {
        IsReturningFromGuild = false;
    }

}
