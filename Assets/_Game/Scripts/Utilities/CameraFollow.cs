using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 startPosition;

    private Transform _camera;
    private Vector3 offset;
    private float offsetValue = 0.1f;


    private void Start()
    {
        _camera = transform;
        _camera.position = startPosition;
        OnInit();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Follow();
        }
    }

    private void Follow()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.fixedDeltaTime);
        _camera.position = smoothedPosition;
    }

    public void OnInit()
    {
        _camera.position = startPosition;
        offset = _camera.position - player.position;
    }

    public void AdjustCamera(int level)
    {
        offset += new Vector3(0f, offsetValue * level, -offsetValue * level);
    }

    public void SetPlayerReference(Transform playerReference)
    {
        player = playerReference;
    }
}
