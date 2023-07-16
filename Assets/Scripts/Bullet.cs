using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed;


    /// <summary>
    /// 弾発射
    /// </summary>
    public void ShotBullet()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);

        Destroy(gameObject, 5);
    }
}
