using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Jiahao
{
    public class SceneLoader : MonoBehaviour //场景加载
    {
        [Header("事件监听")] //监听Scene Event
        public SceneLoadEventSO loadEventSO;
        public GameSceneSO firstLoadedScene;

        [Header("广播事件")]

        [Header("场景")]
        private bool fadeScreen;
        private Vector3 positionToGo;
        private GameSceneSO currentLoadedScene;
        private GameSceneSO sceneToLoad;
        public float fadeDuration;

        private void Awake()
        {
            currentLoadedScene = firstLoadedScene;
            currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        }

        private void OnEnable()
        {
            loadEventSO.LoadRequestEvent += OnLoadRequestEvent; //注册事件
        }

        private void OnDisable() {

            loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        }

        private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) //场景加载
        {
            sceneToLoad = locationToLoad;
            positionToGo = posToGo;
            this.fadeScreen = fadeScreen;

            if (currentLoadedScene != null)
            {
                StartCoroutine(UnLoadPreviousScene());
            }

        }
        private IEnumerator UnLoadPreviousScene() //卸载场景
        {

            if (fadeScreen)
            {
                //实现渐入渐出


            }

            yield return new WaitForSeconds(fadeDuration);
            yield return currentLoadedScene.sceneReference.UnLoadScene(); //等待场景卸载完成

            LoadNewScene();

        }
        private void LoadNewScene() { //加载新场景

            sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            
        }

    }
}
