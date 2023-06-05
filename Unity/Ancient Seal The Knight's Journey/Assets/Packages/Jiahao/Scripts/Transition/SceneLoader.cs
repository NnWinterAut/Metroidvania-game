using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Jiahao
{
    public class SceneLoader : MonoBehaviour, IsSaveable //场景加载
    {
        public Transform playerTrans;
        public Vector3 firstPosition;
        public Vector3 menuPosition;

        [Header("Event监听")] 
        public SceneLoadEventSO loadEventSO; //监听Scene Event
        public VoidEventSO newGameEvent;
        public VoidEventSO backToMenuEvent;

        [Header("广播事件")]
        public VoidEventSO afterSceneLoadedEvent;
        public FadeEventSO fadeEvent;
        public SceneLoadEventSO unloadedSceneEvent;

        [Header("场景")]
        public GameSceneSO firstLoadScene;
        public GameSceneSO menuScene;
        private GameSceneSO currentLoadedScene;
        private GameSceneSO sceneToLoad;
        private Vector3 positionToGo;
        private bool fadeScreen;
        private bool isLoading;
        public float fadeDuration;

        private void Awake()
        {

        }

        private void Start()
        {
            loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);

            //NewGame();
        }

        private void OnEnable()
        {
            loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
            newGameEvent.OnEventRaised += NewGame;
            backToMenuEvent.OnEventRaised += OnBackToMenuEvent;

            IsSaveable saveable = this;
            saveable.RegisterSaveData();
        }

        private void OnDisable() {

            loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
            newGameEvent.OnEventRaised -= NewGame;
            backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;

            IsSaveable saveable = this;
            saveable.UnRegisterSaveData();

        }

        private void OnBackToMenuEvent()
        {
            sceneToLoad = menuScene;
            loadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
        }

        private void NewGame() {

            sceneToLoad = firstLoadScene;

            loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        }

        private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) //场景加载
        {
            if (isLoading)            
                return;
           
            isLoading = true;
            sceneToLoad = locationToLoad;
            positionToGo = posToGo;
            this.fadeScreen = fadeScreen;

            if (currentLoadedScene != null)
            {
                StartCoroutine(UnLoadPreviousScene());
            }
            else 
            {

                LoadNewScene();
            
            }

        }
        private IEnumerator UnLoadPreviousScene() //卸载场景
        {

            if (fadeScreen)
            {
                //实现渐入渐出, 变黑
                fadeEvent.FadeIn(fadeDuration);

            }

            yield return new WaitForSeconds(fadeDuration);

            //调整血条显示
            unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

            yield return currentLoadedScene.sceneReference.UnLoadScene(); //等待场景卸载完成

            playerTrans.gameObject.SetActive(false); //加载过程中关闭人物

            LoadNewScene(); //加载新场景

        }
        private void LoadNewScene() { //加载新场景

            var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            loadingOption.Completed += OnLoadCompleted;
        }

        private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            //场景加载完成后
            currentLoadedScene = sceneToLoad;

            playerTrans.position = positionToGo; //Palyer的位置

            playerTrans.gameObject.SetActive(true); //启动Player

            if (fadeScreen)
            {
                fadeEvent.FadeOut(fadeDuration);

            }

            isLoading = false;

            if (currentLoadedScene.sceneType == SceneType.Loaction) {

                afterSceneLoadedEvent.RaiseEvent(); //场景加载完成后, 需要执行的事件

            }
        }

        public DataDefination GetDataID()
        {
            return GetComponent<DataDefination>();
        }

        public void GetSaveData(Data data)
        {
            data.SaveGameScene(currentLoadedScene);
        }

        public void LoadData(Data data)
        {
            var playerID = playerTrans.GetComponent<DataDefination>().ID;

            if (data.characterPosDict.ContainsKey(playerID))
            {
                positionToGo = data.characterPosDict[playerID].ToVector3();
                sceneToLoad = data.GetSavedScene();

                OnLoadRequestEvent(sceneToLoad, positionToGo, true);
            }
        }
    }
}
