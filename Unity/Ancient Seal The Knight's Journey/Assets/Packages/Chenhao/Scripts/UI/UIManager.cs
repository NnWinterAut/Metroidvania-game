using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chenhao
{
    public class UIManager : MonoBehaviour
    {
        public PlayerStatBar playerStatBar;

        [Header("Event Listening")]

        public CharacterEventSO healthEvent;

        private void OnEnable()
        {
            healthEvent.OnEventRaised += OnHealthEvent;
        }

        private void OnDisable()
        {
            healthEvent.OnEventRaised -= OnHealthEvent;
        }

        private void OnHealthEvent(Character character)
        {
            var persentage = character.currentHealth / character.maxHealth;
            playerStatBar.OnHealthChange(persentage);
        }

    }
}