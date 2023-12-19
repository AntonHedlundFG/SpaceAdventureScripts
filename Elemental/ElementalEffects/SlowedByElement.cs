using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Slow Effect", menuName = "Elemental/Effects/Slowed Effect")]
    public class SlowedByElement : ElementalHitModifier
    {
        [SerializeField] private ElementalType _type;
        [SerializeField][Range(0f, 1f)] private float _slowMultiplier;
        [SerializeField][Range(0f, 10f)] private float _slowDuration;
        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (hitData.HitType != _type || _slowMultiplier > hitData.SlowMultiplier)
            {
                return;
            }

            hitData.SlowMultiplier = _slowMultiplier;
            hitData.SlowDuration = _slowDuration;
        }
    }
}