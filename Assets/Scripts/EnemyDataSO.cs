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
    }
}
