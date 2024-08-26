using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public Transform rightWeaponHolder;
    public Transform leftWeaponHolder;
    public Transform weaponHolder;
    public Gun[] allGuns;
    Gun equippedGunRight;
    Gun equippedGunLeft;

    private void Start() {

    }
    public void EquipGun(Gun gunToEquipRight, Gun gunToEquipLeft) {
        if (equippedGunRight != null) {
            Destroy(equippedGunRight.gameObject);
        }
        equippedGunRight = Instantiate(gunToEquipRight, rightWeaponHolder.position, rightWeaponHolder.rotation);
        equippedGunRight.transform.parent = rightWeaponHolder;

        equippedGunLeft = Instantiate(gunToEquipLeft, leftWeaponHolder.position, leftWeaponHolder.rotation);
        equippedGunLeft.transform.parent = leftWeaponHolder;
    }
    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGunRight != null)
        {
            Destroy(equippedGunRight.gameObject);
        }
        equippedGunRight = Instantiate(gunToEquip, weaponHolder.position, weaponHolder.rotation);
        equippedGunRight.transform.parent = weaponHolder;
    }

    public void EquipGun(int weaponIndexLeft, int weaponIndexRight) {
        EquipGun(allGuns[weaponIndexLeft], allGuns[weaponIndexRight]);
    }

    public void EquipGun(int weaponIndex)
    {
        EquipGun(allGuns[weaponIndex]);
    }

    public void OnTriggerHoldRight() {
        if (equippedGunRight != null) 
        {
            equippedGunRight.OnTriggerHold();
        }
    }

    public void OnTriggerReleaseRight() {
        if (equippedGunRight != null) 
        {
            equippedGunRight.OnTriggerRelease();
        }
    }

    public void OnTriggerHoldLeft()
    {
        if (equippedGunLeft != null)
        {
            equippedGunLeft.OnTriggerHold();
        }
    }
    public void OnTriggerReleaseLeft()
    {
        if (equippedGunLeft != null)
        {
            equippedGunLeft.OnTriggerRelease();
        }
    }

    /*public float GunHeight {
        get {
            return weaponHold.position.y;

        }
    }*/

    /*public void Aim(Vector3 aimPoint) {
        if (equippedGun != null) {
            equippedGun.Aim(aimPoint);
        }
    }*/
    public void Reload() {
        if(equippedGunRight != null) {
            equippedGunRight.Reload();
        }
    }
}
