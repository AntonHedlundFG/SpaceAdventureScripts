using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Combo Damage Effect", menuName = "Elemental/Effects/Combo Damage")]
    public class DamageMod : ElementalHitModifier
    {
        [SerializeField] private ElementalType _hitType;
        [SerializeField] private ElementalType _targetAlreadyHasType;
        [SerializeField] private int _multiplier = 1;
        [SerializeField] private bool _coverInNewType = true;
        [SerializeField] private bool _removeExistingType = false;
        [SerializeField] private int _damageOverTimeDPS = 0;
        [SerializeField] private int _damageOverTimeDuration = 0;
        [SerializeField] private string _audioPath;

        public override void ModifyHit(ref WeaponHit hitData, ElementalTarget target)
        {
            if (!target.AffectedBy(_targetAlreadyHasType) || hitData.HitType != _hitType)
            {
                return;
            }

            hitData.HitDamage *= _multiplier;

            if (_removeExistingType)
            {
                hitData.RemoveTypes.Add(_targetAlreadyHasType);
            }

            if (hitData.DotDuration > 0f || _damageOverTimeDuration <= 0f)
            {
                return;
            }

            hitData.DotDuration = _damageOverTimeDuration;
            hitData.DoTDamagePerSecond = _damageOverTimeDPS;

            if (_coverInNewType)
            {
                hitData.ElementalDuration = _damageOverTimeDuration;
            }

            if (_audioPath != "")
            {
                FMODUnity.RuntimeManager.PlayOneShot(_audioPath);
            }

        }
    }
}


