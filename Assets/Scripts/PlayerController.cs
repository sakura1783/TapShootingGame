using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateBullet();
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    private void GenerateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);

        bullet.GetComponent<Bullet>().ShotBullet();
    }
}
