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
    public class SceneLoader : MonoBehaviour //场景加载
    {
        public Transform playerTrans;
        public Vector3 firstPosition;


        [Header("Event监听")] //监听Scene Event
        public SceneLoadEventSO loadEventSO;
        public GameSceneSO firstLoadedScene;

        [Header("广播事件")]
        public VoidEventSO afterSceneLoadedEvent;
        public FadeEventSO fadeEvent;

        [Header("场景")]
        private bool fadeScreen;
        private Vector3 positionToGo;
        private GameSceneSO currentLoadedScene;
        private GameSceneSO sceneToLoad;
        public float fadeDuration;

        private bool isLoading;

        private void Awake()
        {
 
        }

        private void Start()
        {
            NewGame();
        }

        private void OnEnable()
        {
            loadEventSO.LoadRequestEvent += OnLoadRequestEvent; //注册事件
        }

        private void OnDisable() {

            loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        }

        private void NewGame() {

            sceneToLoad = firstLoadedScene;
            OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        }

        private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) //场景加载
        {
            if (isLoading) {
                
                return;
            
            }

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
                //实现渐入渐出
                fadeEvent.FadeIn(fadeDuration);

            }

            yield return new WaitForSeconds(fadeDuration);
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

            afterSceneLoadedEvent.RaiseEvent(); //场景加载完成后, 需要执行的事件
        }
    }
}
