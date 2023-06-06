using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Jiahao;

namespace Jiahao
{
    [CreateAssetMenu(menuName = "Event/CharacterEventSO")] //创建Event
    public class CharacterEventSO : ScriptableObject
    {
        //传递Character参数
        public UnityAction<Character> OnEventRaised; //Event订阅

        public void RaiseEvent(Character character)
        {

            OnEventRaised?.Invoke(character);

        }
    }
}



