using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private void OnEnable()
    {
        GamePause(true);
    }
    
    private void OnDisable()
    {
        GamePause(false);
    }
    
    public void CloseButton()
    {
        GamePause(false);
        SceneKeeper.UnloadSettingsScene();
    }

    private void GamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0.000000001f : 1;
    }
}