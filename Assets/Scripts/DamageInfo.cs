using UnityEngine;

public struct DamageInfo 
{
    public float BaseDamage;
    public HitZone Hitzone;

    public DamageInfo(float baseDamage, HitZone hitzone = HitZone.Body)
    {
        BaseDamage = baseDamage;
        Hitzone = hitzone;
    }

}
