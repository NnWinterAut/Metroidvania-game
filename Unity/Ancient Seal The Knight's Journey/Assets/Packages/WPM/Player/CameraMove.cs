using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Rigidbody2D playerRigid;

    #region ---- Obsolete ----
#pragma warning disable CS0414 // Prevent warning
    [Obsolete]
    private float speed = 1f;
    [Obsolete]
    private Vector2 offset = new(0, 0);
#pragma warning restore CS0414
    #endregion

    void Start()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Lock the camera to the player.
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
        // Camera follow player with speed and offset
        // transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z), speed * Time.deltaTime);
    }
}
