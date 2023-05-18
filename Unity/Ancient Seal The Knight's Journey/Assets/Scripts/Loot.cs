using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite LootSprite;
    public string LootName;
    public int dropChance;
    
    public Loot(string LootName, int dropChance)
    {
        this.LootName = LootName;
        this.dropChance = dropChance;
    }
}
