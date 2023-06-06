using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class TeleportPoint : MonoBehaviour, Interactable //�����л�
    {
        public Vector3 PlayerPosition;
        public GameSceneSO GoToScene;
        public SceneLoadEventSO loadEventSO;

        public void TriggerAction()
        {
            loadEventSO.RaiseLoadRequestEvent(GoToScene, PlayerPosition, true); //������������Event
        }
    }
}
