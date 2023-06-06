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
    public class SceneLoader : MonoBehaviour, IsSaveable //��������
    {
        public Transform playerTrans;
        public Vector3 firstPosition;
        public Vector3 menuPosition;

        [Header("Event����")] 
        public SceneLoadEventSO loadEventSO; //����Scene Event
        public VoidEventSO newGameEvent;
        public VoidEventSO backToMenuEvent;

        [Header("�㲥�¼�")]
        public VoidEventSO afterSceneLoadedEvent;
        public FadeEventSO fadeEvent;
        public SceneLoadEventSO unloadedSceneEvent;

        [Header("����")]
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

        private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen) //��������
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
        private IEnumerator UnLoadPreviousScene() //ж�س���
        {

            if (fadeScreen)
            {
                //ʵ�ֽ��뽥��, ���
                fadeEvent.FadeIn(fadeDuration);

            }

            yield return new WaitForSeconds(fadeDuration);

            //����Ѫ����ʾ
            unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

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

            if (currentLoadedScene.sceneType == SceneType.Loaction) {

                afterSceneLoadedEvent.RaiseEvent(); //����������ɺ�, ��Ҫִ�е��¼�

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
