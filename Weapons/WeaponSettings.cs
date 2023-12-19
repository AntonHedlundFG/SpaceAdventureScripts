using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elemental;

[CreateAssetMenu(fileName = "NewWeaponSetting", menuName = "Settings/Weapon Settings")]
public class WeaponSettings : ScriptableObject
{
    [Header("Weapon stats")]
    [SerializeField] public ElementalType Type;
    [Range(0, 50)] public int BaseDamage = 5;
    [Range(0f, 5f)] public float RateOfFire = 0.25f;

    [Header("Bullet behaviour")] 
    public GameObject BulletPrefab;
    [Range(10f, 200f)] public float ProjectileSpeed = 20f;
    public bool AffectedByGravity = false;
    [Range(0f, 10f)] public float Duration = 3f;

    [Header("Sound file paths")] 
    public string FireSound;

    [Header("LayerMask: Do not touch")]
    public LayerMask TargetLayerMask;

    [Header("Explosion, if applicable")] 
    [Range(0.1f, 10f)] public float Radius = 3f;
    public GameObject Explosion;

}
