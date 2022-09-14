using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    private static ScenePersist _instance;
    public static ScenePersist Instance { get { return _instance; } }

    int sceneNum;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sceneNum = 0;
    }


    #region Scene Manage
    [ContextMenu("ToMenuScene")]
    public void ToMainMenuScene()
    {
        sceneNum++;
        if (sceneNum > 1)
        {
            sceneNum = 0;
        }
        Debug.Log($"Going to scene {sceneNum}");
        SceneManager.LoadScene("Main Menu");
    }

    public void ToGameScene()
    {
        sceneNum++;
        if (sceneNum > 1)
        {
            sceneNum = 0;
        }
        Debug.Log($"Going to scene {sceneNum}");
        SceneManager.LoadScene("Game");
    }

    #endregion
}
