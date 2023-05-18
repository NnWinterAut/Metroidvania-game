using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTime : MonoBehaviour
{
    [SerializeField] Text TimeText;
    //[SerializeField] VoidEventChannel levelStartedEventChannel;
    //[SerializeField] VoidEventChannel levelClearedEventChannel;

    float clearTime;
    //bool stop = true;

    private void FixedUpdate()
    {
        //if (stop) { return; }

        clearTime += Time.fixedDeltaTime;
        TimeText.text = System.TimeSpan.FromSeconds(value: clearTime).ToString(format: @"mm\:ss\:ff");
    }
    //void OnEnable()
    //{
    //    levelStartedEventChannel.AddListener(action: LevelStart);
    //    levelClearedEventChannel.RemoveListener(action: LevelClear);
    //}
    //void OnDisable()
    //{
    //    levelStartedEventChannel.AddListener(action: LevelStart);
    //    levelClearedEventChannel.RemoveListener(action: LevelClear);
    //}
    //void LevelStart()
    //{
    //    stop = false;
    //}
    //void LevelClear()
    //{
    //    stop = true;
    //}
}
