using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public int StartCoinNumber;
    public Text CoinNumber;
    public static int CurrentCoinNumber;

    void Start()
    {
        CurrentCoinNumber = StartCoinNumber;
    }

    // Update is called once per frame
    void Update()
    {
        CoinNumber.text = CurrentCoinNumber.ToString();
    }

}
