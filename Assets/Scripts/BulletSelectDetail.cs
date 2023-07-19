using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField] private Button btnBulletSelect;

    private BulletSelectManager bulletSelectManager;


    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager)
    {
        this.bulletSelectManager = bulletSelectManager;

        btnBulletSelect.onClick.AddListener(OnClickButtonBulletSelect);
    }

    /// <summary>
    /// btnBulletSelectを押した際の処理
    /// </summary>
    public void OnClickButtonBulletSelect()
    {
        Debug.Log("弾選択");
    }
}
