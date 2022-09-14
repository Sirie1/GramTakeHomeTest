using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public void ToGameScene()
    {
        ScenePersist.Instance.ToGameScene();
    }
}
