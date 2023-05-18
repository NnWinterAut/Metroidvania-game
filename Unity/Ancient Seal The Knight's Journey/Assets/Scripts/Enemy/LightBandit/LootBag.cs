using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject DropItemPreFab;
    public List<Loot> LootList=new List<Loot>();

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot>possibleItem= new List<Loot>();
        foreach(Loot item in LootList)
        {
            //检测随机数，小于<=随机数
            if(randomNumber<=item.dropChance)
            {
                possibleItem.Add(item);
            }
        }
        if (possibleItem.Count > 0)
        {
            Loot DroppedItem = possibleItem[Random.Range(0,possibleItem.Count)];
            return DroppedItem;
        }

        Debug.Log("Not Loot Dropped");
        return null;    
    }

    //创建到场景中
    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot DroppedItem = GetDroppedItem();
        if (DroppedItem != null)
        {
            GameObject LootGameObject = Instantiate(DropItemPreFab, spawnPosition, Quaternion.identity);
            LootGameObject.GetComponent<SpriteRenderer>().sprite = DroppedItem.LootSprite;

            //添加
            float dropFroce = 300f;
            Vector2 dropDirection = new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f));
            LootGameObject.GetComponent<Rigidbody2D>().AddForce( dropDirection*dropFroce,ForceMode2D.Impulse);
        }
    }
}
