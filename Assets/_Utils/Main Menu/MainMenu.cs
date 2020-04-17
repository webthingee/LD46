using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init();
    }

    public void LoadSettingsScene()
    {
        SceneKeeper.LoadSettingsScene();
    }

    public void UnloadSettingsScene()
    {
        SceneKeeper.UnloadSettingsScene();
    }
    
    public void LoadGameScene()
    {
        SceneKeeper.LoadGameScene();
    }
}