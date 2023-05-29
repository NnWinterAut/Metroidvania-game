using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Jiahao
{ 
    public class CameraControl : MonoBehaviour
    {
        private CinemachineConfiner2D confiner2D;
        private void Awake()
        {
            confiner2D = GetComponent<CinemachineConfiner2D>();
        }

        private void Start()
        {
            GetNewCameraBounds();
        }

        private void GetNewCameraBounds() //查找当前场景Tag为Bound的摄像头边界
        {

            var obj = GameObject.FindGameObjectWithTag("Bound");
            
            if (obj == null) {
                
                return;
            
            }

            confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
            confiner2D.InvalidateCache(); //清缓存
        
        }

    }
}

