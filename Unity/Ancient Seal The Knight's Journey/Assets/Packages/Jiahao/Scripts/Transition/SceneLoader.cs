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
    public class SceneLoader : MonoBehaviour //��������
    {
        public Transform playerTrans;
        public Vector3 firstPosition;


        [Header("Event����")] //����Scene Event
        public SceneLoadEventSO loadEventSO;
        public GameSceneSO firstLoadedScene;

        [Header("�㲥�¼�")]
        public VoidEventSO afterSceneLoadedEvent;
        public FadeEventSO fadeEvent;

        [Header("����")]
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
            loadEventSO.LoadRequestEvent += OnLoadRequestEvent; //ע���¼�
        }

        private void OnDisable() {

            loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        }

        private void NewGame() {

            sceneToLoad = firstLoadedScene;
            OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        }

        private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) //��������
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
        private IEnumerator UnLoadPreviousScene() //ж�س���
        {

            if (fadeScreen)
            {
                //ʵ�ֽ��뽥��
                fadeEvent.FadeIn(fadeDuration);

            }

            yield return new WaitForSeconds(fadeDuration);
            yield return currentLoadedScene.sceneReference.UnLoadScene(); //�ȴ�����ж�����

            playerTrans.gameObject.SetActive(false); //���ع����йر�����

            LoadNewScene(); //�����³���

        }
        private void LoadNewScene() { //�����³���

            var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            loadingOption.Completed += OnLoadCompleted;
        }

        private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            //����������ɺ�
            currentLoadedScene = sceneToLoad;

            playerTrans.position = positionToGo; //Palyer��λ��

            playerTrans.gameObject.SetActive(true); //����Player

            if (fadeScreen)
            {
                fadeEvent.FadeOut(fadeDuration);

            }

            isLoading = false;

            afterSceneLoadedEvent.RaiseEvent(); //����������ɺ�, ��Ҫִ�е��¼�
        }
    }
}
