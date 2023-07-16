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
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Vector3 direction = tapPos - transform.position;
            ////Vector3.Scaleで第一引数の各成分と第二引数の同じ成分を乗算する。この処理で、不要なZ成分の除去ができる
            //direction = Vector3.Scale(direction, new Vector3(1, 1, 0));
            //direction = direction.normalized;

            //上の処理を1行で記述
            Vector3 direction = Vector3.Scale(tapPos - transform.position, new Vector3(1, 1, 0)).normalized;

            GenerateBullet(direction);
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    private void GenerateBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);

        bullet.GetComponent<Bullet>().ShotBullet(direction);
    }
}
