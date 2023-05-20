using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chenhao
{
    public class PlayerStatBar : MonoBehaviour
    {
        public Image healthImage;

        public Image healthDelayImage;

        public Image MagicImage;


        private void Update()
        {
            if (healthDelayImage.fillAmount > healthImage.fillAmount)
            {
                healthDelayImage.fillAmount -= Time.deltaTime;
            }
        }

        public void OnHealthChange(float persentage)
        {
            healthImage.fillAmount = persentage;
        }
    }
}