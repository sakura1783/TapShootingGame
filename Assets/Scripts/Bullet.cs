using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    //public float bulletSpeed;

    public int bulletPower;

    public BulletDataSO.BulletData bulletData;

    [SerializeField] private Image imgBullet;


    /// <summary>
    /// 弾発射
    /// </summary>
    public void ShotBullet(Vector3 direction, BulletDataSO.BulletData bulletData = null)
    {
        this.bulletData = bulletData;

        if (bulletData == null)
        {
            return;
        }

        imgBullet.sprite = this.bulletData.bulletSprite;

        GetComponent<Rigidbody2D>().AddForce(direction * this.bulletData.bulletSpeed);
        Debug.Log($"弾のスピード：{this.bulletData.bulletSpeed}");

        Destroy(gameObject, 5);
    }
}
