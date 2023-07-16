using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed;


    void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(transform.right * 300);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShotBullet();
        }
    }

    /// <summary>
    /// 弾発射
    /// </summary>
    private void ShotBullet()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);

        Destroy(gameObject, 5);
    }
}
