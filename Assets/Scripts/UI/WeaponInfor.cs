using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfor : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;

    public int weaponDamage;

    public float weaponRange;

}
