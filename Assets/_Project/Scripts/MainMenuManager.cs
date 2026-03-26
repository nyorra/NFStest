using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string citySceneName = "CityScene";
    [SerializeField] private string garageSceneName = "GarageScene";

    public void PlayGame()
    {
        if (!PlayerPrefs.HasKey("SelectedCarIndex"))
        {
            PlayerPrefs.SetInt("SelectedCarIndex", 0);
        }
        
        SceneManager.LoadScene(citySceneName);
    }

    public void OpenGarage()
    {
        SceneManager.LoadScene(garageSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
