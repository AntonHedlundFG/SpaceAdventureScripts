using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Physics")]
    [Range(2f, 100f)] public float JumpPower = 10f;
    [Range(5f, 100f)] public float Gravity = 9.82f;
    [Range(5f, 100f)] public float MovementSpeed = 10f;
    [Range(0f, 10f)] public float DragMultiplier = 3f;

    [Header("Looking Around")]
    [Range(90f, 360f)] public float XRotateSpeed = 180f;
    [Range(5f, 360f)] public float YRotateSpeed = 30f;
    [Range(-80f, 0f)] public float MinYRotation = -50f;
    [Range(0f, 80f)] public float MaxYRotation = 60f;

    [Header("Resource Interaction")]
    [Range(0.5f, 5f)] public float ResourceUseRadius = 2f;
    public LayerMask ResourceLayerMask;

    [Header("Health")] 
    [Range(1, 200)] public int MaxHealth = 10;
    [Range(1, 100)] public int ResurrectHealth = 5;
    [Range(0f, 1f)] public float DyingSpeedMultiplier = 0.1f;
    [Range(0f, 1f)] public float DyingAimSpeedMultiplier = 0f;
}
