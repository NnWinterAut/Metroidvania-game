using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton); //����ѡ��
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
