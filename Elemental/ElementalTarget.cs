using UnityEngine;

namespace Elemental
{
    public class ElementalTarget : MonoBehaviour
    {
        [SerializeField] private HitModifierList _modList;
        private EnemyHealth _health;
        private SpiderAI _spiderAI;
        private SpiderAIPatrol _spiderAIPatrol;

        [SerializeField] private string[] _soundPaths = new string[4];

        private float[] _elementsExpireTimes = new float[4]; //Keeps track of which elements affect the target, and when they run out.

        private void Start()
        {
            _health = GetComponent<EnemyHealth>();
            _spiderAI = GetComponent<SpiderAI>();
            _spiderAIPatrol = GetComponent<SpiderAIPatrol>();

            for (int i = 0; i < _elementsExpireTimes.Length; i++)
            {
                _elementsExpireTimes[i] = Time.time - 1f; //Each element starts expired.
            }
        }

        public bool AffectedBy(ElementalType type) => _elementsExpireTimes[(int)type] > Time.time; //False if time has expired, otherwise true.

        public virtual void ApplyHit(WeaponHit hitData)
        {


            if (_modList != null)
            {
                ApplyModifiers(ref hitData);
            }

            if (_health != null)
            {
                _health.DealDamage(hitData.HitDamage, hitData.Source);

                if (hitData.DotDuration > 0f)
                {
                    _health.DealDoTDamage(hitData.DoTDamagePerSecond, hitData.DotDuration);
                }

                //Play sound if paths have been added
                int index = (int)hitData.HitType;
                
                if (!hitData.IsSpread && index < _soundPaths.Length && _soundPaths[index] != "")
                {
                    FMODUnity.RuntimeManager.PlayOneShot(_soundPaths[index]);
                }

            }

            if (_spiderAI != null)
            {
                // TODO: Knockback, but this requires a RigidBody on the enemy. Is it worth it? Knockback requires disabling NavMeshAgent and activating the RB temporarily. 
                // Maybe we can come up with something else for water?
                if (hitData.SlowDuration > 0f)
                {
                    Debug.Log(hitData.SlowDuration);
                    _spiderAI.ApplySlow(hitData.SlowMultiplier, hitData.SlowDuration);
                }

                if (hitData.StunDuration > 0f)
                {
                    _spiderAI.ApplyStun(hitData.StunDuration);
                }
            }
            else if (_spiderAIPatrol)
            {
                if (hitData.SlowDuration > 0f)
                {
                    _spiderAIPatrol.ApplySlow(hitData.SlowMultiplier, hitData.SlowDuration);
                }

                if (hitData.StunDuration > 0f)
                {
                    _spiderAIPatrol.ApplyStun(hitData.StunDuration);
                }
            }

            if (hitData.ElementalDuration > 0f)
            {
                //Updates expiration time if new effect lasts longer than previous one.
                _elementsExpireTimes[(int)hitData.HitType] = Mathf.Max(_elementsExpireTimes[(int)hitData.HitType], Time.time + hitData.ElementalDuration);
            }

            if (hitData.SpreadElementalRange > 0f)
            {
                CreateSpreadEffect(hitData);
            }

            foreach (ElementalType type in hitData.RemoveTypes)
            {
                _elementsExpireTimes[(int)type] = Time.time;
            }


        }

        private void ApplyModifiers(ref WeaponHit hitData)
        {
            foreach (ElementalHitModifier hitMod in _modList.GetMods())
            {
                hitMod.ModifyHit(ref hitData, this);

                if (hitData.Immune)
                {
                    return;
                }
            }
        }

        private void CreateSpreadEffect(WeaponHit hitData)
        {
            WeaponHit spreadHitData = new WeaponHit(hitData.SpreadElementalType, hitData.Source, hitData.OriginLocation,
                hitData.SpreadDamage);
            spreadHitData.IsSpread = true;

            Collider[] colliders = Physics.OverlapSphere(transform.position, hitData.SpreadElementalRange);
            foreach (Collider collider in colliders)
            {
                ElementalTarget target = collider.GetComponentInParent<ElementalTarget>();
                if (target == null)
                {
                    continue;
                }
                target.ApplyHit(spreadHitData);
            }


            if (hitData.SpreadEffect != null)
            {
                GameObject explosion = Instantiate(hitData.SpreadEffect, hitData.OriginLocation, Quaternion.identity);
                explosion.GetComponent<Explosion>()?.SetRadius(hitData.SpreadElementalRange);
            }
        }

    }
}

