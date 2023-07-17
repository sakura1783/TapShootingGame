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

    public int maxGenerateCount;

    public bool isGenerateEnd;

    public bool isBossDestroyed;


    void Update()
    {
        if (isGenerateEnd)
        {
            return;
        }

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

            if (generateCount >= maxGenerateCount)
            {
                isGenerateEnd = true;

                Debug.Log("全ての敵を生成しました");

                //ボスの生成
                StartCoroutine(GenerateBoss());
            }
        }
    }

    /// <summary>
    /// 敵生成
    /// </summary>
    private void GenerateEnemy(bool isBoss = false)
    {
        GameObject enemy = Instantiate(enemyPrefab, transform, false);

        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        enemyController.SetUpEnemyController(isBoss);

        //上の処理を1行で記述
        //Instantiate(enemyPrefab, transform, false).GetComponent<EnemyController>().SetUpEnemyController(isBoss);

        if (isBoss)
        {
            enemyController.AdditionalSetUpEnemyController(this);
        }
    }

    /// <summary>
    /// ボスの生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateBoss()
    {
        //TODO ボス出現の警告演出

        yield return new WaitForSeconds(1);

        GenerateEnemy(true);

        //TODO ボス討伐
    }

    /// <summary>
    /// ボス討伐状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchBossDestroyed(bool isSwitch)
    {
        isBossDestroyed = isSwitch;

        Debug.Log("ボス討伐");

        gameManager.SwitchGameUp(isBossDestroyed);

        //TODO ゲームクリアの準備
    }
}
