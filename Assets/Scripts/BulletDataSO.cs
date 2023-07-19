using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "Create BulletDataSO")]
public class BulletDataSO : ScriptableObject
{
    public List<BulletData> bulletDataList = new();


    /// <summary>
    /// 弾の種類
    /// </summary>
    [Serializable]
    public enum BulletType
    {
        Player_Leaf,
        Player_Fire,
        Player_Ice,
        Player_Thunder,
        Enemy_Water,
        Enemy_Fire,
        Boss_Darkness,
    }

    /// <summary>
    /// 使用者の種類
    /// </summary>
    [Serializable]
    public enum UserType
    {
        Player,
        Enemy,
        Boss,
    }

    /// <summary>
    /// 弾のデータ
    /// </summary>
    [Serializable]
    public class BulletData
    {
        public int bulletNo;
        public float bulletSpeed;
        public int bulletPower;
        public float Interval;
        public BulletType bulletType;
        public Sprite btnSprite;
        public UserType userType;
        public int needExp;  //解放に必要な経験値
        public float duration;  //使用できる時間
        public Sprite bulletSprite;
        public string description;  //説明
    }
}
