using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class Chest : MonoBehaviour, Interactable
    {
        public SpriteRenderer spriteRenderer;
        public Sprite openSprite; //�л�ͼƬ
        public Sprite closeSprite;
        public bool isDone; //����״̬

        public void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void TriggerAction()
        {
            if (!isDone)
            {
                OpenChest();
            }
        }

        private void OnEnable()
        {
            spriteRenderer.sprite = isDone ? openSprite : closeSprite; //ȷ���Ƿ��

        }

        public void OpenChest()
        {

            spriteRenderer.sprite = openSprite;
            isDone = true;
            this.gameObject.tag = "Untagged"; //ȡ����ʾ

        }
    }
}
