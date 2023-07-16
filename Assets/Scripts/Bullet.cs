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
    public void ShotBullet(Vector3 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);

        Destroy(gameObject, 5);
    }
}
