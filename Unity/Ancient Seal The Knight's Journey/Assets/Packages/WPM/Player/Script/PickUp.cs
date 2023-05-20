using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WPM
{
    public class PickUp : MonoBehaviour
    {
        //public AudioClip soundEffect;
        //public GameObject pickupEffect;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //PlayerManager manager = collision.GetComponent<PlayerManager>();
            //if (manager)
            //{
            //    bool pickup= manager.PickupItem(gameObject);
            //    if (pickup)
            //    {
            //        Destroy(gameObject);
            //        RemoveItem();
            //    }
            //}
        }

        private void RemoveItem()
        {
            //AudioSource.PlayClipAtPoint(soundEffect,transform.position);
            //Instantiate(pickupEffect,transform.position, Quaternion.identity);
        }
    }
}