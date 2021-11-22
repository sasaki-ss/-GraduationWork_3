using UnityEngine;

public class RunTimeInit
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitBeforeLoad()
    {
        Debug.LogFormat("RuntimeInitializer::InitializeBeforeSceneLoad() called.");

        var systemObj = GameObject.Instantiate(Resources.Load("systemObj"));
        GameObject.DontDestroyOnLoad(systemObj);
    }
}
