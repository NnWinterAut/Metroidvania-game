using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jiahao
{
    public class Menu : MonoBehaviour
    {
        public GameObject newGameButton;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(newGameButton); //МќХЬбЁжа
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
