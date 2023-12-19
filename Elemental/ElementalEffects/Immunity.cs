using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Immunity Effect", menuName = "Elemental/Effects/Immunity")]
    public class Immunity : ElementalHitModifier
    {
        [SerializeField] private ElementalType _type;
        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (_type == hitData.HitType)
            {
                hitData.Immune = true;
            }
        }
    }
}