using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;

    [Header("�޵�ʱ��")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    private void Start() {

        currentHealth = maxHealth;
    }

    private void Update() {

        if (invulnerable) {

            invulnerableCounter -= Time.deltaTime; //��ʼ2�뵹��ʱ
            if (invulnerableCounter <= 0) {

                invulnerable = false; //���Լ����ܵ��˺�
            }
        }
    
    }

    public void TakeDamage(Attack attacker) //�ܵ��˺�
    {
        if (invulnerable) {
            return;
        }
        if (currentHealth - attacker.damage > 0) //Ѫ���㹻��
        {
            currentHealth -= attacker.damage; //˲���ܵ��˺�
            TriggerInvulnerable(); //�������, �����޵�
        }
        else {
            currentHealth = 0; //��������;
         
        }


    }

    public void TriggerInvulnerable() {

        if (invulnerable == false) {
            invulnerable = true; //��ʼ��Update�м�ʱ
            invulnerableCounter = invulnerableDuration; //��ʱ��ʼ, 2���޵�
        }
    
    }
}
