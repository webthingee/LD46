using UnityEngine;

public class SampleScene : MonoBehaviour
{
    private bool _showSettings;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        
        _showSettings = !_showSettings;
        SceneKeeper.SettingsScene(_showSettings);
    }
}