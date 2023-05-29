using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Jiahao { 

    [CreateAssetMenu(menuName = "Event/PlayAudioEvent")]
    public class PlayAudioEvent : MonoBehaviour
    {
        public UnityAction<AudioClip> onEventRaised;

        public void RaiseEvent(AudioClip audioClip) {

            onEventRaised?.Invoke(audioClip);
        }
 
    }


}


