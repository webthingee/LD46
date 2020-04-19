using UnityEngine.SceneManagement;

public class SceneKeeper
{
    // Main Menu
    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    
    // Settings Menu
    public static void LoadSettingsScene()
    {
        SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);
    }

    public static void UnloadSettingsScene()
    {
        if (!SceneManager.GetSceneByName("SettingsMenu").isLoaded) return;
        SceneManager.UnloadSceneAsync("SettingsMenu", UnloadSceneOptions.None);
    }
    
    // Settings Menu via bool
    public static void SettingsScene(bool isToLoad)
    {

        if (isToLoad)
        {
            SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);
        }
        else
        {
            if (!SceneManager.GetSceneByName("SettingsMenu").isLoaded) return;
            SceneManager.UnloadSceneAsync("SettingsMenu", UnloadSceneOptions.None);
        }
    }

    // Sample Scene
    public static void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    
    public static void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
    }
    
    public static void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
    }
}