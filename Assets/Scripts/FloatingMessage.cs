using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingMessage : MonoBehaviour
{
    [SerializeField] private Text txtFloatingMessage;


    /// <summary>
    /// フロート表示の種類
    /// </summary>
    public enum FloatingMessageType
    {
        EnemyDamage,
        PlayerDamage,
        GetExp,
        BulletCost,  //弾のコスト支払い時
    }

    /// <summary>
    /// フロート表示の制御
    /// </summary>
    /// <param name="floatingValue"></param>
    /// <param name="floatingMessageType"></param>
    public void DisplayFloatingMessage(int floatingValue, FloatingMessageType floatingMessageType, bool isWeakness = false)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-20, 20), transform.localPosition.y + Random.Range(-10, 10), 0);

        txtFloatingMessage.text = floatingValue.ToString();

        txtFloatingMessage.color = GetMessageColor(floatingMessageType);

        if (isWeakness)
        {
            transform.localScale = Vector3.one * 1.2f;
        }

        transform.DOLocalMoveY(transform.localPosition.y + 50, 1f).OnComplete(() => { Destroy(gameObject); });
    }

    /// <summary>
    /// フロート表示の色を設定
    /// </summary>
    /// <param name="floatingMessageType"></param>
    /// <returns></returns>
    private Color GetMessageColor(FloatingMessageType floatingMessageType)
    {
        switch (floatingMessageType)
        {
            case FloatingMessageType.EnemyDamage:
            case FloatingMessageType.PlayerDamage:
                return Color.red;
            case FloatingMessageType.GetExp:
                return Color.yellow;
            case FloatingMessageType.BulletCost:
                return Color.blue;
            default:
                return Color.white;
        }
    }
}
