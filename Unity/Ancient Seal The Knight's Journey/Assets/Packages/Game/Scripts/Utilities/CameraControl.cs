using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("Event监听")]
    public VoidEventSO afterSceneLoadedEvent;
    private CinemachineConfiner2D confiner2D;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    public void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent; //注册事件
    }

    public void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent; //注册事件
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBound();
    }

    private void GetNewCameraBound() //查找当前场景Tag为Bound的摄像头边界
    {

        var obj = GameObject.FindGameObjectWithTag("Bound");
            
        if (obj == null) {
                
            return;
            
        }

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache(); //清缓存
        
    }

}

