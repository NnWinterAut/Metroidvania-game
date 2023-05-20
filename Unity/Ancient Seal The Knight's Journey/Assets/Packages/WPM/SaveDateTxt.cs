using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM
{
    public class SaveDateTxt : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            CloseSaveDataUI();
        }

        public void CloseSaveDataUI()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                //主界面
                GameObject mainMenu = GameObject.FindGameObjectWithTag("mainMenu").transform.gameObject;
                //存档界面调用
                GameObject saveDataUI = GameObject.FindGameObjectWithTag("SaveDateUI").transform.gameObject;
                mainMenu.transform.GetChild(0).gameObject.SetActive(true);
                saveDataUI.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        public void Data1()
        {

        }
        public void Data2()
        {

        }
        public void Data3()
        {

        }
    }
}