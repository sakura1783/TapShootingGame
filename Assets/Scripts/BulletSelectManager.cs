using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelectManager : MonoBehaviour
{
    [SerializeField] private BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField] private Transform bulletButtonTran;

    private const int maxBulletButtonCount = 4;

    public List<BulletSelectDetail> bulletSelectDetailList = new();


    void Start()
    {
        StartCoroutine(GenerateBulletSelectDetail());
    }

    /// <summary>
    /// 弾選択用ボタンの生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDetail()
    {
        for (int i = 0; i < maxBulletButtonCount; i++)
        {
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletButtonTran, false);

            bulletSelectDetail.SetUpBulletSelectDetail(this);

            bulletSelectDetailList.Add(bulletSelectDetail);

            //0.25秒だけ処理を中断(順番にボタンが生成されるようにする演出)
            yield return new WaitForSeconds(0.25f);
        }
    }
}
