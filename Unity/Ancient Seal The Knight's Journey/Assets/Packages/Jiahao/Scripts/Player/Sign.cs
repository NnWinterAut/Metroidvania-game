using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jiahao {

    public class Sign : MonoBehaviour //��������
    {
        public GameObject signSprite;
        public Transform playerTrans;
        private PlayerInputControl playerInput;
        private Animator anim;
        private bool canPress;
        private Interactable targetItem; 

        public void Awake()
        {

            anim = signSprite.GetComponent<Animator>(); //��������嶯��
            playerInput = new PlayerInputControl();
            playerInput.Enable();
        }

        private void OnEnable()
        {
            InputSystem.onActionChange += OnActionChange; //��������豸
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
                GetComponent<AudioDefination>().PlayAudioClip(); //������Ч
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
            signSprite.GetComponent<SpriteRenderer>().enabled = canPress; //�Ӵ�ʱ������
            signSprite.transform.localScale = playerTrans.localScale; //ͼ��������ƶ�
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Interactable"))
            {
                canPress = true;
                targetItem = other.GetComponent<Interactable>(); //��Ӧ��ײ��Ľӿ�����
            
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
                canPress = false; //ͼ����ʧ
        }
    }

}
