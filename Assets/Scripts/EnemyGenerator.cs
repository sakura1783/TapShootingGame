using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float prepareTime;

    private int generateCount;

    private float timer;

    private GameManager gameManager;


    void Update()
    {
        if (!gameManager.isGameUp)
        {
            PrepareGenerateEnemy();
        }
    }

    public void SetUpEnemyGenerator(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    /// <summary>
    /// 敵生成の準備
    /// </summary>
    private void PrepareGenerateEnemy()
    {
        timer += Time.deltaTime;

        if (timer >= prepareTime)
        {
            timer = 0;

            GenerateEnemy();

            generateCount++;

            Debug.Log($"生成した敵の数：{generateCount}");
        }
    }

    /// <summary>
    /// 敵生成
    /// </summary>
    private void GenerateEnemy()
    {
        //GameObject enemy = Instantiate(enemyPrefab, transform, false);

        //EnemyController enemyController = enemy.GetComponent<EnemyController>();

        //enemyController.SetUpEnemyController();

        //上の処理を1行で記述
        Instantiate(enemyPrefab, transform, false).GetComponent<EnemyController>().SetUpEnemyController();
    }
}
