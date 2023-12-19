using UnityEngine;
using Elemental;

public class ElementalProjectile : Projectile
{

    public override void OnTriggerEnter(Collider other)
    {
        ElementalTarget[] targets = other.GetComponentsInParent<ElementalTarget>();
        if (targets != null || targets.Length != 0)
        {
            WeaponHit hit = new WeaponHit(_settings.Type, _instigator, transform.position, _settings.BaseDamage);
            foreach (ElementalTarget target in targets)
            {
                target.ApplyHit(hit);

            }
        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            return;
        }
        CreateImpactEffect();
        Destroy(gameObject);
    }

}
