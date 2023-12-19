using System.Collections.Generic;
using UnityEngine;

namespace Elemental
{
    public enum ElementalType
    {
        Earth,
        Fire,
        Water,
        Air
    }

    public struct WeaponHit
    {
        public ElementalType HitType; //Incoming attack elemental type.
        public GameObject Source; //The player GameObject that fired the projectile. Can be used to determine aggro

        public float ElementalDuration; //If set, the target is "covered in" this element for the duration. Used for combination effects.

        public int HitDamage; 
        public int DoTDamagePerSecond; 
        public int DotDuration;

        public Vector3 OriginLocation; //Where did the projectile "trigger"? Used for knockback direction
        public float KnockbackForce; //Is there a knockback effect?

        public float SlowMultiplier; //Slow effect, 0 <= x <= 1
        public float SlowDuration;

        public List<ElementalType> RemoveTypes; //List of ElementalTypes to remove on application? For example, Covered In Tar + Hit By Fire = Tar removed + Explosion

        public bool Immune; //Set to true if target has an Immunity modifier, prevents damage.

        //A spread occurs if an elemental combo causes an AoE effect, like the Tar + Fire combination which creates an explosion.
        //The explosion can deal damage, or simply apply the element to nearby enemies.
        public bool IsSpread; //This is true if the WeaponHit comes from a Spread effect, so that Spread effects cannot propagate infinitely
        public ElementalType SpreadElementalType; 
        public float SpreadElementalRange;
        public int SpreadDamage;
        public GameObject SpreadEffect; //The ParticleEffect visual, if there is one.

        public float StunDuration;

        public WeaponHit(ElementalType hitType, GameObject source, Vector3 originLocation, int hitDamage = 0)
        {
            HitType = hitType;
            Source = source;
            ElementalDuration = 0f;
            HitDamage = hitDamage;
            DoTDamagePerSecond = 0;
            DotDuration = 0;
            OriginLocation = originLocation;
            KnockbackForce = 0f;
            SlowMultiplier = 1f;
            SlowDuration = 0f;
            RemoveTypes = new List<ElementalType>();
            Immune = false;
            SpreadElementalType = ElementalType.Fire;
            SpreadElementalRange = 0f;
            SpreadDamage = 0;
            IsSpread = false;
            SpreadEffect = null;
            StunDuration = 0f;
        }
    }
}