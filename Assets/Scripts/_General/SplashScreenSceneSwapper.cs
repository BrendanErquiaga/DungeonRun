using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenSceneSwapper : MonoBehaviour 
{
    public string levelToLoad = "MainMenu";

	public void LoadLevel(string levelToLoad)
	{
        SceneManager.LoadScene(levelToLoad);
	}

    public void SwapScene()
    {
        SceneManager.LoadScene(this.levelToLoad);
    }
}