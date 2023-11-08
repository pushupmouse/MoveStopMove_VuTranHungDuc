using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform player;
    private Transform myCamera;
    private Vector3 offset;

    private void Awake()
    {
        myCamera = transform;
        FindPlayer();
    }

    private void Update()
    {
        if (player != null)
        {
            Follow();
        }
    }

    private void Follow()
    {
        myCamera.DOMoveX(player.position.x + offset.x, speed * Time.fixedDeltaTime);
        myCamera.DOMoveY(player.position.y + offset.y, speed * Time.fixedDeltaTime);
        myCamera.DOMoveZ(player.position.z + offset.z, speed * Time.fixedDeltaTime);
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>().transform;
        offset = myCamera.position - player.position;
    }
}
