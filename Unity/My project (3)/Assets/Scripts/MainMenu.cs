using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingMenu");
    }
    public void GoTomainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadGame()
    {
        // Load the save data
        GameObject mainMenu = GameObject.FindGameObjectWithTag("mainMenu").transform.gameObject;
        GameObject saveDateUI = GameObject.FindGameObjectWithTag("SaveDateUI").transform.gameObject;
        saveDateUI.transform.GetChild(0).gameObject.SetActive(true);
        mainMenu.transform.GetChild(0).gameObject.SetActive(false);
    }
}
