using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour{
    public Transform firePoint;
    public GameObject bulletPrefab;

    //Check once per frame if Fire1 (leftCtrl) is pressed
    void Update (){
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }
    void Shoot(){
        //What shooting does
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }
}

