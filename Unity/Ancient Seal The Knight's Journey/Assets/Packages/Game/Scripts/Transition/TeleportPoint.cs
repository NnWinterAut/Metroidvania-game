using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class TeleportPoint : MonoBehaviour, Interactable //场景切换
    {
        public Vector3 PlayerPosition;
        public GameSceneSO GoToScene;
        public SceneLoadEventSO loadEventSO;

        public void TriggerAction()
        {
            loadEventSO.RaiseLoadRequestEvent(GoToScene, PlayerPosition, true); //启动加载请求Event
        }
    }
}
