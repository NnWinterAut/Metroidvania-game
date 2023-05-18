using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create a custom asset menu item
[CreateAssetMenu (menuName = "EventChannels/VoidEventChannel", fileName= "VoidEventChannel_")]
public class VoidEventChannel : ScriptableObject
{
    System.Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }

    //Î¯ÍÐ¶©ÔÄ
    public void AddListener(System.Action action)
    {
        OnEventRaised += action;
    }

    public void RemoveListener(System.Action action)
    {
        OnEventRaised -= action;
    }
}
 