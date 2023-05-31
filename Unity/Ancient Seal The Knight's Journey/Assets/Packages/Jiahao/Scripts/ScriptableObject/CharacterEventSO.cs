using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Jiahao;

namespace Jiahao
{
    [CreateAssetMenu(menuName = "Event/CharacterEventSO")] //����Event
    public class CharacterEventSO : ScriptableObject
    {
        //����Character����
        public UnityAction<Character> OnEventRaised; //Event����

        public void RaiseEvent(Character character)
        {

            OnEventRaised?.Invoke(character);

        }
    }
}



