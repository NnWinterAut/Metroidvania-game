using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Jiahao
{
    public class SceneLoader : MonoBehaviour //��������
    {
        [Header("�¼�����")] //����Scene Event
        public SceneLoadEventSO loadEventSO;
        public GameSceneSO firstLoadedScene;

        [Header("�㲥�¼�")]

        [Header("����")]
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
            loadEventSO.LoadRequestEvent += OnLoadRequestEvent; //ע���¼�
        }

        private void OnDisable() {

            loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        }

        private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) //��������
        {
            sceneToLoad = locationToLoad;
            positionToGo = posToGo;
            this.fadeScreen = fadeScreen;

            if (currentLoadedScene != null)
            {
                StartCoroutine(UnLoadPreviousScene());
            }

        }
        private IEnumerator UnLoadPreviousScene() //ж�س���
        {

            if (fadeScreen)
            {
                //ʵ�ֽ��뽥��


            }

            yield return new WaitForSeconds(fadeDuration);
            yield return currentLoadedScene.sceneReference.UnLoadScene(); //�ȴ�����ж�����

            LoadNewScene();

        }
        private void LoadNewScene() { //�����³���

            sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            
        }

    }
}
