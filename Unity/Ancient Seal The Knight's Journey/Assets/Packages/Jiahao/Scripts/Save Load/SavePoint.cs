using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, Interactable
{
    [Header("�㲥�¼�")]
    public VoidEventSO saveDataEvent;

    [Header("��������")]
    public SpriteRenderer spriteRenderer;
    public Sprite darkSprite;
    public Sprite lightSprite;
    public bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        //lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;

            saveDataEvent.RaiseEvent();

            this.gameObject.tag = "Untagged";
        }
    }
}
