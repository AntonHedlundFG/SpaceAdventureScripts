using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Knockback Effect", menuName = "Elemental/Effects/Knockback")]
    public class KnockbackByElement : ElementalHitModifier
    {
        [SerializeField] private ElementalType _type;
        [SerializeField] [Range(0f, 50f)] private float _knockbackMinForce = 2f;
        [SerializeField] [Range(0f, 50f)] private float _knockbackMaxForce = 5f;
        [SerializeField] [Range(0f, 5f)] private float _knockbackMaxDistance = 2f;

        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (_type != hitData.HitType)
            {
                return;
            }

            float distance = Vector3.Distance(hitData.OriginLocation, target.transform.position);
            float t = Mathf.InverseLerp(_knockbackMaxDistance, 0f, distance);
            hitData.KnockbackForce = Mathf.Lerp(_knockbackMinForce, _knockbackMaxForce, t);

            Debug.Log("Knockback effect not implemented, force: " + hitData.KnockbackForce);

        }
    }
}