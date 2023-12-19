using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Covered In Effect", menuName = "Elemental/Effects/Covered In Element")]
    public class CoveredInElement : ElementalHitModifier
    {
        [SerializeField] private ElementalType _type;
        [SerializeField] private float _duration;

        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (_type != hitData.HitType)
            {
                return;
            }

            hitData.ElementalDuration = _duration;
        }
    }
}