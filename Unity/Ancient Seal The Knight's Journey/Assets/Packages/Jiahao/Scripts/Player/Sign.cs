using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jiahao {

    public class Sign : MonoBehaviour //场景互动
    {
        public GameObject signSprite;
        public Transform playerTrans;
        private PlayerInputControl playerInput;
        private Animator anim;
        private bool canPress;
        private Interactable targetItem; 

        public void Awake()
        {

            anim = signSprite.GetComponent<Animator>(); //获得子物体动画
            playerInput = new PlayerInputControl();
            playerInput.Enable();
        }

        private void OnEnable()
        {
            InputSystem.onActionChange += OnActionChange; //检测输入设备
            playerInput.Player.Confirm.started += OnConfirm;
        }

        private void OnDisable()
        {
            canPress = false;
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            if (canPress) {

                targetItem.TriggerAction();
                GetComponent<AudioDefination>().PlayAudioClip(); //播放音效
            }
        }

        private void OnActionChange(object obj, InputActionChange actionChange)
        {
            if (actionChange == InputActionChange.ActionStarted)
            {
                var device = ((InputAction)obj).activeControl.device;
                switch (device.device)
                {
                    case Keyboard:
                        anim.Play("keyboard");
                        break;
                }
            
            }
        }

        public void Update()
        {
            signSprite.GetComponent<SpriteRenderer>().enabled = canPress; //接触时才启动
            signSprite.transform.localScale = playerTrans.localScale; //图标随玩家移动
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Interactable"))
            {
                canPress = true;
                targetItem = other.GetComponent<Interactable>(); //对应碰撞体的接口类型
            
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
                canPress = false; //图标消失
        }
    }

}
