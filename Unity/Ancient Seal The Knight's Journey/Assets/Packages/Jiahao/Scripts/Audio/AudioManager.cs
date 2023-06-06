using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Event����")]
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;

    public AudioSource BGMSource;
    public AudioSource FXSource;

    private void OnEnable() //�¼�����
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    public void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}

