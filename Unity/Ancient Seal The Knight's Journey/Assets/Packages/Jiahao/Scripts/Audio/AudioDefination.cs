using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class AudioDefination : MonoBehaviour
    {
        //�㲥����
        public PlayAudioEventSO playAudioEvent;
        public AudioClip audioClip;

        public bool EnablePlay;

        private void OnEnable()
        {
            if (EnablePlay)
            {
                PlayAudioClip();
            }
        }

        public void PlayAudioClip()
        {   //�����¼�

            playAudioEvent.RaiseEvent(audioClip);
        }
    }
}
