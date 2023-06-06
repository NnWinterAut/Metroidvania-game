using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("Event����")]
    public VoidEventSO afterSceneLoadedEvent;
    private CinemachineConfiner2D confiner2D;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    public void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent; //ע���¼�
    }

    public void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent; //ע���¼�
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBound();
    }

    private void GetNewCameraBound() //���ҵ�ǰ����TagΪBound������ͷ�߽�
    {

        var obj = GameObject.FindGameObjectWithTag("Bound");
            
        if (obj == null) {
                
            return;
            
        }

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache(); //�建��
        
    }

}

