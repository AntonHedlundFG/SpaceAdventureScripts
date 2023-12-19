using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Spread Effect", menuName = "Elemental/Effects/Spread")]
    public class SpreadElement : ElementalHitModifier
    {
        [SerializeField] private ElementalType _hitElementalType;

        [SerializeField] private bool _requiresExisting = false;
        [SerializeField] private ElementalType _requiredExistingType;
        [SerializeField] private bool _removeExisting = true;

        [SerializeField] private float _spreadRange = 3f;
        [SerializeField] private int _damage = 0;

        [SerializeField] private GameObject _spreadEffect;
        [SerializeField] private string _audioPath;
        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (hitData.IsSpread)
            {
                return;
            }

            if (hitData.HitType != _hitElementalType)
            {
                return;
            }

            if (_requiresExisting && !target.AffectedBy(_requiredExistingType))
            {
                return;
            }

            if (_removeExisting)
            {
                hitData.RemoveTypes.Add(_requiredExistingType);
            }

            hitData.SpreadDamage = _damage;
            hitData.SpreadElementalRange = _spreadRange;
            hitData.SpreadElementalType = hitData.HitType;

            if (_spreadEffect != null)
            {
                hitData.SpreadEffect = _spreadEffect;
            }

            if (_audioPath != "")
            {
                FMODUnity.RuntimeManager.PlayOneShot(_audioPath);
            }
        }
    }

}
