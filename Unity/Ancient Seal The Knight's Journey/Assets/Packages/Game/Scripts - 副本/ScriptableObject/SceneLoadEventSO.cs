using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{   //ȥ�ĸ��������������
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 playerPosition, bool fadeScreen) {
        //������������
        LoadRequestEvent?.Invoke(locationToLoad, playerPosition, fadeScreen);
    }

}