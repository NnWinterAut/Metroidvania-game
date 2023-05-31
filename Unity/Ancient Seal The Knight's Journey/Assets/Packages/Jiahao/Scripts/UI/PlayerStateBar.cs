using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jiahao;

namespace Jiahao { 

    public class PlayerStateBar : MonoBehaviour
    {
        public Image health;
        public Image healthDelay;
        public Image power;

        public void OnHealthChange(float persentage) { //health变更百分比的变化

            health.fillAmount = persentage;
        }
    }

}

