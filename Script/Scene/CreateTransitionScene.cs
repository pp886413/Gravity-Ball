using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTransitionScene : MonoBehaviour
{
    public GameObject TransitionScene;

    private GameObject InstTransitionScene;

    private string SceneName;

    private void Start()
    {
        Create(SceneName);
    }
    public void Create(string SceneName)
    {
        InstTransitionScene = Instantiate(TransitionScene);
        InstTransitionScene.GetComponent<ChangeScene>().SceneName = SceneName;
        PlayerController.IsGamePause = false;
    }
    public void SetSceneName(string SceneName)
    {
        this.SceneName = SceneName;
    }
}
