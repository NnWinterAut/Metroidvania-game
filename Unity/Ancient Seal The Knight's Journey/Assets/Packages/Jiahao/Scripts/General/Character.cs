using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //调用unity事件
using Jiahao;

namespace Jiahao
{
    public class Character : MonoBehaviour, IsSaveable
    {
        [Header("事件监听")]
        public VoidEventSO newGameEvent;

        [Header("基本属性")]
        public float maxHealth;
        public float currentHealth;
        public float maxPower;
        public float currentPower;
        public float powerRecoverSpeed;

        [Header("无敌时间")]
        public float invulnerableDuration;
        public float invulnerableCounter;
        public bool invulnerable;

        public UnityEvent<Transform> OnTakeDamage; //添加Player受伤并且后退的事件
        public UnityEvent<Character> OnHealthChange;
        public UnityEvent onDie;

        private void NewGame()
        {
            currentHealth = maxHealth;
            currentPower = maxPower;
            OnHealthChange?.Invoke(this);
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {

            newGameEvent.OnEventRaised += NewGame;
            IsSaveable saveable = this;
            saveable.RegisterSaveData();//强行执行
        }

        private void OnDisable()
        {
            newGameEvent.OnEventRaised -= NewGame;
            IsSaveable saveable = this;
            saveable.UnRegisterSaveData(); 
        }

        private void Update()
        {

            if (invulnerable)
            {
                invulnerableCounter -= Time.deltaTime; //开始2秒倒计时
                if (invulnerableCounter <= 0)
                {
                    invulnerable = false; //可以继续受到伤害
                }
            }

            if (currentPower < maxPower) {
                
                currentPower += Time.deltaTime * powerRecoverSpeed; //恢复Power
            
            }

        }

        public void OnSlide(int cost)
        {
            currentPower -= cost;
            OnHealthChange?.Invoke(this);
        }

        public void TakeDamage(Attack attacker) //受到伤害
        {
            if (invulnerable)
            {
                return;
            }
            if (currentHealth - attacker.damage > 0) //血量足够厚
            {
                //执行受伤
                currentHealth -= attacker.damage; //瞬间受到伤害
                OnTakeDamage?.Invoke(attacker.transform); //执行受伤
                TriggerInvulnerable(); //激活触发器, 激活无敌
            }
            else
            {
                currentHealth = 0; //人物死亡;
                onDie?.Invoke();
            }

            //传递
            OnHealthChange?.Invoke(this);

        }

        public void TriggerInvulnerable() // 触发受伤无敌
        {

            if (invulnerable == false)
            {
                invulnerable = true; //开始在Update中计时
                invulnerableCounter = invulnerableDuration; //计时开始, 2秒无敌
            }

        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Water"))
            {
                if (currentHealth > 0)
                {//死亡、更新血量
                    currentHealth = 0;
                    OnHealthChange?.Invoke(this);
                    onDie?.Invoke();
                }
            }
        }

        public DataDefination GetDataID()
        {
            return GetComponent<DataDefination>(); //

        }

        public void GetSaveData(Data data)
        {
            if (data.characterPosDict.ContainsKey(GetDataID().ID))
            {
                data.characterPosDict[GetDataID().ID] = new SerializeVector3(transform.position);
                data.floatSavedData[GetDataID().ID + "health"] = this.currentHealth;
                data.floatSavedData[GetDataID().ID + "power"] = this.currentPower;
            }
            else
            {
                data.characterPosDict.Add(GetDataID().ID, new SerializeVector3(transform.position));
                data.floatSavedData.Add(GetDataID().ID + "health", this.currentHealth);
                data.floatSavedData.Add(GetDataID().ID + "power", this.currentPower);
            }
        }

        public void LoadData(Data data)
        {
            if (data.characterPosDict.ContainsKey(GetDataID().ID))
            {
                this.currentHealth = data.floatSavedData[GetDataID().ID + "health"];
                this.currentPower = data.floatSavedData[GetDataID().ID + "power"];
                transform.position = data.characterPosDict[GetDataID().ID].ToVector3();

                //通知UI更新
                OnHealthChange?.Invoke(this);
            }
        }
    }
}