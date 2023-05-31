using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //����unity�¼�
using Jiahao;

namespace Jiahao
{
    public class Character : MonoBehaviour
    {
        [Header("��������")]
        public float maxHealth;
        public float currentHealth;
        public float maxPower;
        public float currentPower;
        public float powerRecoverSpeed;

        [Header("�޵�ʱ��")]
        public float invulnerableDuration;
        public float invulnerableCounter;
        public bool invulnerable;

        public UnityEvent<Transform> OnTakeDamage; //���Player���˲��Һ��˵��¼�
        public UnityEvent<Character> OnHealthChange;
        public UnityEvent onDie;

        private void Start()
        {

            currentHealth = maxHealth;
            currentPower = maxPower;
            OnHealthChange?.Invoke(this); //maxhealth
        }

        private void Update()
        {

            if (invulnerable)
            {
                invulnerableCounter -= Time.deltaTime; //��ʼ2�뵹��ʱ
                if (invulnerableCounter <= 0)
                {
                    invulnerable = false; //���Լ����ܵ��˺�
                }
            }

            if (currentPower < maxPower) {
                
                currentPower += Time.deltaTime * powerRecoverSpeed; //�ָ�Power
            
            }

        }

        public void OnSlide(int cost)
        {
            currentPower -= cost;
            OnHealthChange?.Invoke(this);
        }

        public void TakeDamage(Attack attacker) //�ܵ��˺�
        {
            if (invulnerable)
            {
                return;
            }
            if (currentHealth - attacker.damage > 0) //Ѫ���㹻��
            {
                //ִ������
                currentHealth -= attacker.damage; //˲���ܵ��˺�
                TriggerInvulnerable(); //�������, �����޵�

                OnTakeDamage?.Invoke(attacker.transform); //ִ������
            }
            else
            {
                currentHealth = 0; //��������;
                onDie?.Invoke();
            }

            //����
            OnHealthChange?.Invoke(this);

        }

        public void TriggerInvulnerable() // ���������޵�
        {

            if (invulnerable == false)
            {
                invulnerable = true; //��ʼ��Update�м�ʱ
                invulnerableCounter = invulnerableDuration; //��ʱ��ʼ, 2���޵�
            }

        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Water")) {

                //��������
                currentHealth = 0;

                OnHealthChange?.Invoke(this);
                onDie?.Invoke();
            }
        }
    }
}