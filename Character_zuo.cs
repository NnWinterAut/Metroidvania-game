using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //����unity�¼�
using Jiahao;



    public class Character : MonoBehaviour, IsSaveable
    {
        [Header("�¼�����")]
        public VoidEventSO newGameEvent;

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

        public UnityEvent<Transform> OnTakeDamage; //����Player���˲��Һ��˵��¼�
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
            saveable.RegisterSaveData();//ǿ��ִ��
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
                invulnerableCounter -= Time.deltaTime; //��ʼ2�뵹��ʱ
                if (invulnerableCounter <= 0)
                {
                    invulnerable = false; //���Լ����ܵ��˺�
                }
            }

            if (currentPower < maxPower) {
                
                currentPower += Time.deltaTime * powerRecoverSpeed; //cool down
            
            }

        }

        public void OnSlide(int cost) 
        {
            currentPower -= cost;// cost energy
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
                OnTakeDamage?.Invoke(attacker.transform); //ִ������
                TriggerInvulnerable(); //�������, �����޵�
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
            if (other.CompareTag("Water"))
            {
                if (currentHealth > 0)
                {//����������Ѫ��
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

                //֪ͨUI����
                OnHealthChange?.Invoke(this);
            }
        }
    }
