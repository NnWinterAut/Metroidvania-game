using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class Chest : MonoBehaviour, Interactable
    {
        public SpriteRenderer spriteRenderer;
        public Sprite openSprite; //切换图片
        public Sprite closeSprite;
        public bool isDone; //互动状态

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
            spriteRenderer.sprite = isDone ? openSprite : closeSprite; //确认是否打开

        }

        public void OpenChest()
        {

            spriteRenderer.sprite = openSprite;
            isDone = true;
            this.gameObject.tag = "Untagged"; //取消提示

        }
    }
}
