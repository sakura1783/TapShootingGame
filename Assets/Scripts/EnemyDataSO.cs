using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
    Easy,
    Normal,
    Elite,
    Boss,
}

public enum MoveType
{
    Straight,  //直進
    Meandering,  //蛇行
    Boss_Horizontal,  //ボス・水平移動
}

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();


    [Serializable]
    public class EnemyData
    {
        public int enemyNo;
        public int hp;
        public int attackPoint;
        public Sprite enemySprite;
        public EnemyType enemyType;
        public int exp;
        public float moveDuration;  //拠点までの移動時間
        public MoveType moveType;
    }
}
