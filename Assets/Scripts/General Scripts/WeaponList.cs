using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList
{
    public static WeaponClass rifle;
    public static WeaponClass claws;

    void Start()
    {
        rifle = new WeaponClass(magazineCount : 3, shotsPerSecond : 2, magazingSize : 20, weaponRange : 40, weaponDamage : 20);

        // If melee attack magazineSize should be zero.
        claws = new WeaponClass(magazineCount : 1, shotsPerSecond : 2, magazingSize : 0, weaponRange : 10, weaponDamage : 20);
    }
}
