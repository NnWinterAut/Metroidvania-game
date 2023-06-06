using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jiahao;

namespace Jiahao { 

    public class PlayerStateBar : MonoBehaviour
    {
        private Character currentCharacter;

        public Image health;
        public Image healthDelay;
        public Image power;

        private bool isRecovering;

        private void Update()
        {
            if (healthDelay.fillAmount > health.fillAmount)
            {
                healthDelay.fillAmount -= Time.deltaTime;
            }

            if (isRecovering) //回复Character power
            {
                float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
                power.fillAmount = persentage;

                if (persentage >= 1)
                {
                    isRecovering = false;
                    return;
                }
            }
        }

        public void OnHealthChange(float persentage) { //health变更百分比的变化

            health.fillAmount = persentage;
        }

        public void OnPowerChange(Character character) //Power变更百分比的变化
        {
            isRecovering = true;
            currentCharacter = character;
        }
    }

}

