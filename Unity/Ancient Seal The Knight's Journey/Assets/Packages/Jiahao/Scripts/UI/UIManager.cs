using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Jiahao;

namespace Jiahao
{
    public class UIManager : MonoBehaviour
    {
        public PlayerStateBar playerStateBar;
        [Header("�����¼�")]
        public CharacterEventSO healthEvent;

        private void OnEnable() //ע���¼�
        {
            healthEvent.OnEventRaised += OnHealthEvent;
        }

        private void OnDisable()
        {
            healthEvent.OnEventRaised -= OnHealthEvent;
        }

        private void OnHealthEvent(Character character) //����Character��ֵ
        {
            var persentage = character.currentHealth / character.maxHealth;

            playerStateBar.OnHealthChange(persentage);
        }
    }
}



