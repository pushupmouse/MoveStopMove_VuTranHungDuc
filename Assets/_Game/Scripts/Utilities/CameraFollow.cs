using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform player;

    private Transform _camera;
    private Vector3 offset;
    private float offsetValue = 0.1f;

    private void Awake()
    {
        _camera = transform;
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
        _camera.DOMoveX(player.position.x + offset.x, speed * Time.fixedDeltaTime);
        _camera.DOMoveY(player.position.y + offset.y, speed * Time.fixedDeltaTime);
        _camera.DOMoveZ(player.position.z + offset.z, speed * Time.fixedDeltaTime);
    }

    public void FindPlayer()
    {
        offset = _camera.position - player.position;
    }

    public void AdjustCamera()
    {
        offset += new Vector3(0f, offsetValue, -offsetValue);
    }

    public void SetPlayerReference(Transform playerReference)
    {
        player = playerReference;
    }
}
