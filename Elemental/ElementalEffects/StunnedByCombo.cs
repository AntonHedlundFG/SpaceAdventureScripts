using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Stun Combo Effect", menuName = "Elemental/Effects/Stun Combo Effect")]
    public class StunnedByCombo : ElementalHitModifier
    {
        [SerializeField] private ElementalType _hitType;
        [SerializeField] private ElementalType _targetAlreadyHasType;
        [SerializeField] private float _stunDuration;
        [SerializeField] private bool _removeExistingType = true;
        [SerializeField] private bool _coverInNewType = true;
        [SerializeField] private string _audioPath;
        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (!target.AffectedBy(_targetAlreadyHasType) || hitData.HitType != _hitType)
            {
                return;
            }

            hitData.StunDuration = Mathf.Max(hitData.StunDuration, _stunDuration);
            if (_removeExistingType)
            {
                hitData.RemoveTypes.Add(_targetAlreadyHasType);
            }

            if (_coverInNewType)
            {
                hitData.ElementalDuration = _stunDuration;
            }

            if (_audioPath != "")
            {
                FMODUnity.RuntimeManager.PlayOneShot(_audioPath);
            }
        }
    }
}