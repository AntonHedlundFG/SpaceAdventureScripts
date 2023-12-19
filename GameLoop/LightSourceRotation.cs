using System;
using UnityEngine;

public class LightSourceRotation : MonoBehaviour
{
    [SerializeField] private float dayLengthInSeconds = 240f;
    private float speed = 1.5f;
    private Transform lightTransform;
    
    
    private void Awake()
    {
        lightTransform = GetComponent<Transform>();
        speed = 360 / dayLengthInSeconds;
    }

    private void Update()
    {
        lightTransform.RotateAround(Vector3.zero, Vector3.right, speed * Time.deltaTime);        
    }


}
