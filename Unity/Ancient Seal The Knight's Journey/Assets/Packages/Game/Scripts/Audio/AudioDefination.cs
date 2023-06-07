using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    //广播传递
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
    {   //呼叫事件

        playAudioEvent.RaiseEvent(audioClip);
    }
}
