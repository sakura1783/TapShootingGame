using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField] private Button btnBulletSelect;

    private BulletSelectManager bulletSelectManager;

    [SerializeField] private Image imgBulletButton;

    public BulletDataSO.BulletData bulletData;


    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager, BulletDataSO.BulletData bulletData)
    {
        this.bulletSelectManager = bulletSelectManager;
        this.bulletData = bulletData;

        imgBulletButton.sprite = this.bulletData.btnSprite;

        btnBulletSelect.onClick.AddListener(OnClickButtonBulletSelect);
    }

    /// <summary>
    /// btnBulletSelectを押した際の処理
    /// </summary>
    public void OnClickButtonBulletSelect()
    {
        Debug.Log("弾選択");

        //このバレット選択ボタンに設定されているBulletDataをGameDataのcurrentBulletData変数に登録し、現在使用中のバレットとする
        GameData.instance.SetBulletData(bulletData);
    }
}
