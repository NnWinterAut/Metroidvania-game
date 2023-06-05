using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Jiahao;

namespace Jiahao
{
    public class UIManager : MonoBehaviour
    {
        public PlayerStateBar playerStateBar;

        [Header("监听事件")]
        public CharacterEventSO healthEvent;
        public SceneLoadEventSO unloadedSceneEvent;
        public VoidEventSO loadDataEvent;
        public VoidEventSO gameOverEvent;
        public VoidEventSO backToMenuEvent;


        [Header("组件")]
        public GameObject gameOverPanel;
        public GameObject restartBtn;

        private void OnEnable() //注册事件
        {
            healthEvent.OnEventRaised += OnHealthEvent;
            unloadedSceneEvent.LoadRequestEvent += OnUnLoadedSceneEvent;
            gameOverEvent.OnEventRaised += OnGameOverEvent;
            loadDataEvent.OnEventRaised += OnLoadDataEvent;
            backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        }

        private void OnDisable()
        {
            healthEvent.OnEventRaised -= OnHealthEvent;
            unloadedSceneEvent.LoadRequestEvent -= OnUnLoadedSceneEvent;
            gameOverEvent.OnEventRaised -= OnGameOverEvent;
            loadDataEvent.OnEventRaised -= OnLoadDataEvent;
            backToMenuEvent.OnEventRaised -= OnLoadDataEvent;


        }

        private void OnGameOverEvent()
        {
            gameOverPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(restartBtn);
        }

        private void OnLoadDataEvent()
        {
            gameOverPanel.SetActive(false);
        }

        private void OnUnLoadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
        {
            var isMenu = sceneToLoad.sceneType == SceneType.Menu; //请求场景为Menu

            playerStateBar.gameObject.SetActive(!isMenu); //隐藏UI
            
        }

        private void OnHealthEvent(Character character) //访问Character数值
        {
            var persentage = character.currentHealth / character.maxHealth;
            playerStateBar.OnHealthChange(persentage);

            playerStateBar.OnPowerChange(character);
        }


    }
}



