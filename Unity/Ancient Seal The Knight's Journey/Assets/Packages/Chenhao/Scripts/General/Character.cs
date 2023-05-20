using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Chenhao
{
    public class Character : MonoBehaviour
    {
        [Header("Basic attributes")]

        public float maxHealth;
        public float currentHealth;

        [Header("invincible")]
        public float invulnerableDuration;
        private float invulnerableCounter;
        public bool invulnerable;

        public UnityEvent<Character> OnHealthChange;
        public UnityEvent<Transform> OnTakeDamage;
        public UnityEvent OnDie;

        private void Start()
        {
            currentHealth = maxHealth;
            OnHealthChange?.Invoke(this);
        }

        private void Update()
        {
            if (invulnerable)
            {
                invulnerableCounter -= Time.deltaTime;

                if (invulnerableCounter <= 0)
                {
                    invulnerable = false;
                }
            }
        }

        public void TakeDamage(Attack attacker)
        {
            if (invulnerable)
                return;

            if (currentHealth - attacker.damage > 0)
            {
                currentHealth -= attacker.damage;
                TriggerInvulnerable();
                //Hurt
                OnTakeDamage?.Invoke(attacker.transform);

            }
            else
            {
                currentHealth = 0;
                //Death
                OnDie.Invoke();

            }
            OnHealthChange?.Invoke(this);

        }

        private void TriggerInvulnerable()
        {
            if (!invulnerable)
            {
                invulnerable = true;
                invulnerableCounter = invulnerableDuration;
            }
        }
    }
}