using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jiahao
{
    [CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
    public class GameSceneSO : ScriptableObject
    {
        public AssetReference sceneReference; //ÒýÓÃ
        public SceneType sceneType;
    }
}
