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
        [Header("监听事件")]
        public CharacterEventSO healthEvent;

        private void OnEnable() //注册事件
        {
            healthEvent.OnEventRaised += OnHealthEvent;
        }

        private void OnDisable()
        {
            healthEvent.OnEventRaised -= OnHealthEvent;
        }

        private void OnHealthEvent(Character character) //访问Character数值
        {
            var persentage = character.currentHealth / character.maxHealth;

            playerStateBar.OnHealthChange(persentage);
        }
    }
}



