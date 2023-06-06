using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{   //去哪个场景，玩家坐标
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 playerPosition, bool fadeScreen) {
        //场景加载请求
        LoadRequestEvent?.Invoke(locationToLoad, playerPosition, fadeScreen);
    }

}