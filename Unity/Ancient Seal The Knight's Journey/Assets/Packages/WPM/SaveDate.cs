using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM
{
    public class SaveDate : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            changeShow();
        }

        //关闭存档界面，显示主界面
        public void changeShow()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                GameObject saveDateManu = GameObject.FindGameObjectWithTag("SaveDataUI").gameObject;
                GameObject mainMenu = GameObject.FindGameObjectWithTag("mainMenu").gameObject;
                saveDateManu.transform.GetChild(0).gameObject.SetActive(false);
                mainMenu.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}