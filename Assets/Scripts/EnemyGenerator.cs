using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;

    [SerializeField] private float prepareTime;

    //private int generateCount;

    //private float timer;

    private GameManager gameManager;

    //public int maxGenerateCount;

    //public bool isGenerateEnd;

    public bool isBossDestroyed;

    public EnemyDataSO enemyDataSO;

    //public List<EnemyDataSO.EnemyData> easyEnemyDatas = new List<EnemyDataSO.EnemyData>();
    //public List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();
    //public List<EnemyDataSO.EnemyData> eliteEnemyDatas = new List<EnemyDataSO.EnemyData>();
    //public List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();

    public MoveEventSO moveEventSO;

    [SerializeField] private List<EnemyController> enemiesList = new();


    //void Update()
    //{
    //    if (isGenerateEnd)
    //    {
    //        return;
    //    }

    //    if (!gameManager.isGameUp)
    //    {
    //        PrepareGenerateEnemy();
    //    }
    //}

    //public void SetUpEnemyGenerator(GameManager gameManager)
    //{
    //    this.gameManager = gameManager;

    //    //easyEnemyDatas = GetEnemyTypeList(EnemyType.Easy);
    //    //normalEnemyDatas = GetEnemyTypeList(EnemyType.Normal);
    //    //eliteEnemyDatas = GetEnemyTypeList(EnemyType.Elite);
    //    //bossEnemyDatas = GetEnemyTypeList(EnemyType.Boss);
    //}

    /// <summary>
    /// 敵生成の準備
    /// </summary>
    //private void PrepareGenerateEnemy()
    //{
    //    timer += Time.deltaTime;

    //    if (timer >= prepareTime)
    //    {
    //        timer = 0;

    //        SpawnEnemy();

    //        generateCount++;

    //        Debug.Log($"生成した敵の数：{generateCount}");

    //        if (generateCount >= maxGenerateCount)
    //        {
    //            isGenerateEnd = true;

    //            Debug.Log("全ての敵を生成しました");

    //            //ボスの生成
    //            StartCoroutine(GenerateBoss());
    //        }
    //    }
    //}

    /// <summary>
    /// 敵生成
    /// </summary>
    //private void GenerateEnemy(EnemyType enemyType = EnemyType.Normal)
    //{
    //    int randomEnemyNo;

    //    EnemyDataSO.EnemyData enemyData = null;

    //    switch (enemyType)
    //    {
    //        case EnemyType.Easy:
    //            randomEnemyNo = Random.Range(0, easyEnemyDatas.Count);
    //            enemyData = easyEnemyDatas[randomEnemyNo];
    //            break;
    //        case EnemyType.Normal:
    //            randomEnemyNo = Random.Range(0, normalEnemyDatas.Count);
    //            enemyData = normalEnemyDatas[randomEnemyNo];
    //            break;
    //        case EnemyType.Elite:
    //            randomEnemyNo = Random.Range(0, eliteEnemyDatas.Count);
    //            enemyData = eliteEnemyDatas[randomEnemyNo];
    //            break;
    //        case EnemyType.Boss:
    //            randomEnemyNo = Random.Range(0, bossEnemyDatas.Count);
    //            enemyData = bossEnemyDatas[randomEnemyNo];
    //            break;
    //    }

    //    GameObject enemy = Instantiate(enemyPrefab, transform, false);

    //    EnemyController enemyController = enemy.GetComponent<EnemyController>();

    //    enemyController.SetUpEnemyController(enemyData);

    //    //上の処理を1行で記述
    //    //Instantiate(enemyPrefab, transform, false).GetComponent<EnemyController>().SetUpEnemyController(isBoss);

    //    //enemyController.AdditionalSetUpEnemyController(this);
    //}

    /// <summary>
    /// ボスの生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateBoss()
    {
        //TODO ボス出現の警告演出

        yield return new WaitForSeconds(1);

        //GenerateEnemy(EnemyType.Boss);
        SpawnBoss();

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

        //ゲームクリアの準備
        gameManager.PrepareGameClear();
    }

    /// <summary>
    /// 引数で指定された敵の種類のListを作成し、作成した値を戻す
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    //private List<EnemyDataSO.EnemyData> GetEnemyTypeList(EnemyType enemyType)
    //{
    //    List<EnemyDataSO.EnemyData> enemyDatas = new List<EnemyDataSO.EnemyData>();

    //    for (int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
    //    {
    //        if (enemyDataSO.enemyDataList[i].enemyType == enemyType)
    //        {
    //            enemyDatas.Add(enemyDataSO.enemyDataList[i]);
    //        }
    //    }

    //    return enemyDatas;
    //}



    [System.Serializable]
    public class SpawnData
    {
        public int enemyNo;
        public int maxSpawnCount;
        public int spawnRate;
    }

    [SerializeField] private List<SpawnData> spawnDataList = new();  //インスペクターでボス以外の敵の各値をそれぞれ設定する


    [System.Serializable]
    public class SpawnedEnemy
    {
        public int enemyNo;  //Key
        public int spawnCount;  //Value
    }

    private Dictionary<int, int> spawnCountByEnemyType = new();

    //生成する全敵の合計数を算出したプロパティ
    public int MaxSpawnEnemyCount => spawnDataList.Sum(spawnData => spawnData.maxSpawnCount);

    private int totalSpawnRate = 0;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEnemyGenerator(GameManager gameManager)
    {
        this.gameManager = gameManager;

        InitializeSpawnCounts();

        CalculateTotalSpawnRate();

        StartCoroutine(SpawnTimer());
    }

    /// <summary>
    /// 各生成数の初期化
    /// </summary>
    private void InitializeSpawnCounts()
    {
        spawnCountByEnemyType.Clear();

        foreach (var spawnData in spawnDataList)
        {
            spawnCountByEnemyType[spawnData.enemyNo] = 0;
        }

        Debug.Log("生成数初期化");
    }

    /// <summary>
    /// 最大生成数を超えていない敵の生成確率の合計値を算出
    /// </summary>
    private void CalculateTotalSpawnRate()
    {
        totalSpawnRate = 0;

        foreach (var spawnData in spawnDataList)
        {
            //TryGetValueは第一引数には取得したいKey、第二引数には取得できた場合にValueの値を格納するための変数をoutキーワードを使用して指定
            if (spawnCountByEnemyType.TryGetValue(spawnData.enemyNo, out int spawnCount) && spawnCount < spawnData.maxSpawnCount)
            {
                totalSpawnRate += spawnData.spawnRate;
            }
        }
    }

    /// <summary>
    /// 生成時間の計測
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnTimer()
    {
        if (!gameManager.isGameUp)
        {
            int spawnCount = 0;

            while (MaxSpawnEnemyCount > spawnCount)
            {
                if (gameManager.isSetUpEnd)
                {
                    yield return new WaitForSeconds(prepareTime);

                    SpawnEnemy();

                    spawnCount = spawnCountByEnemyType.Values.Sum();

                    Debug.Log($"現在の総生成数：{spawnCount}");
                }

                //if(gameManager.isSetUpEnd)を追加したので追加(これを書かないと無限ループになり、Unityが動かなくなる)
                yield return null;
            }

            Debug.Log("全ての敵を生成しました");

            StartCoroutine(GenerateBoss());
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    private void SpawnEnemy()
    {
        EnemyController enemy = Instantiate(enemyPrefab, transform, false);

        CalculateTotalSpawnRate();

        int randomNo = Random.Range(0, totalSpawnRate);

        int spawnEnemyNo = GetRandomEnemyType(randomNo);

        if (spawnEnemyNo == -1)
        {
            Debug.Log("生成する敵がいません");
        }

        spawnCountByEnemyType[spawnEnemyNo]++;
        Debug.Log($"生成した敵の種類は{spawnEnemyNo}番です");

        enemy.SetUpEnemyController(enemyDataSO.enemyDataList[spawnEnemyNo], this);

        enemiesList.Add(enemy);
    }

    /// <summary>
    /// ランダムな敵の確定
    /// </summary>
    /// <param name="randomNo"></param>
    /// <returns></returns>
    private int GetRandomEnemyType(int randomNo)
    {
        int cumulativeChance = 0;  //重み付け用の値

        foreach (var spawnData in spawnDataList)
        {
            if (spawnCountByEnemyType[spawnData.enemyNo] < spawnData.maxSpawnCount)
            {
                //生成数を超えていない場合に限り、重み付け用の値を加算
                cumulativeChance += spawnData.spawnRate;

                if (randomNo < cumulativeChance)
                {
                    return spawnData.enemyNo;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// ボス生成
    /// </summary>
    private void SpawnBoss()
    {
        EnemyController boss = Instantiate(enemyPrefab, transform).GetComponent<EnemyController>();

        boss.SetUpEnemyController(enemyDataSO.enemyDataList[3], this);

        enemiesList.Add(boss);
    }

    /// <summary>
    /// TotalExpの表示更新準備
    /// </summary>
    /// <param name="exp"></param>
    public void PrepareDisplayTotalExp(int exp)
    {
        gameManager.uiManager.UpdateDisplayTotalExp(GameData.instance.GetTotalExp());

        gameManager.uiManager.CreateFloatingMessageToExp(exp, FloatingMessage.FloatingMessageType.GetExp);

        //使用可能バレット選択ボタンの確認と更新
        gameManager.bulletSelectManager.JudgeOpenBullets();
    }

    /// <summary>
    /// プレイヤーと敵との位置から方向を判定する準備
    /// </summary>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    public Vector3 PrepareGetPlayerDirection(Vector3 enemyPos)
    {
        return gameManager.GetPlayerDirection(enemyPos);
    }

    /// <summary>
    /// enemiesListに登録されている敵の内、シーンに残っている敵のゲームオブジェクトを破壊し、enemiesListをクリアする
    /// </summary>
    public void ClearEnemiesList()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            //要素が空ではない(プレイヤーに破壊されずにゲーム画面に残っている)なら
            if (enemiesList[i] != null)
            {
                Destroy(enemiesList[i].gameObject);
            }
        }

        enemiesList.Clear();
    }

    /// <summary>
    /// 一時的に存在しているオブジェクト(弾、エフェクトなど)を全て破棄
    /// </summary>
    public void DestroyTemporaryObjectContainer()
    {
        //プロパティを利用した場合
        Destroy(TransformHelper.TemporaryObjectContainerTran.gameObject);

        //プロパティを利用しない場合
        //Destroy(TransformHelper.GetTemporaryObjectContainerTran().gameObject);
    }
}
