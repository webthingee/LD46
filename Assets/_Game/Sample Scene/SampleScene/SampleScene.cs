using UnityEngine;

public class SampleScene : MonoBehaviour
{
    private bool _showSettings;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _showSettings = !_showSettings;
            SceneKeeper.SettingsScene(_showSettings);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _showSettings = !_showSettings;
            SceneKeeper.SettingsScene(_showSettings);
        }
    }

    public void LoadMainScene()
    {
        SceneKeeper.LoadMainMenuScene();
    }
}