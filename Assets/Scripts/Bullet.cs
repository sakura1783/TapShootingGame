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

    private bool isTarget;  //追尾弾用。追尾対象の敵がいるかどうか

    private Vector3 nearPos;  //追尾弾用。追尾対象の敵の位置情報


    void Update()
    {
        //追尾弾の対象がいない場合は処理しない
        if (!isTarget)
        {
            return;
        }

        Vector3 currentPos = transform.position;

        //MoveTowardsで第一引数(起点)から第二引数(目標点)の位置に、第三引数(1フレームの移動速度(移動距離))で指定されたスピードで移動する
        transform.position = Vector3.MoveTowards(currentPos, nearPos, Time.deltaTime / 10 * bulletData.bulletSpeed);
    }

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

        //追尾弾の場合は一番近い敵を追尾する
        if (bulletData.bulletType == BulletDataSO.BulletType.Player_Fire)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                nearPos = enemies[0].transform.position;

                for (int i = 0; i < enemies.Length; i++)
                {
                    Vector3 pos = enemies[i].transform.position;

                    //画面の下(プレイヤー)に近いものをnearPosとして更新する
                    if (nearPos.x > pos.x && nearPos.y > pos.y)
                    {
                        nearPos = pos;
                    }

                    //上の処理だとxの値のみを最初に比較するため処理にズレが生じることがあるので、2回ではなく1回の比較判定を行う方法にリファクタリング
                    if (Vector3.Scale(nearPos, new(1, 1, 0)).normalized.sqrMagnitude > Vector3.Scale(pos, new(1, 1, 0)).normalized.sqrMagnitude)
                    {
                        nearPos = pos;
                    }
                }

                isTarget = true;
            }
        }

        //追尾弾以外の処理
        if (bulletData.bulletType != BulletDataSO.BulletType.Player_Fire)
        {
            GetComponent<Rigidbody2D>().AddForce(direction * this.bulletData.bulletSpeed);
        }

        Destroy(gameObject, 5);
    }
}
