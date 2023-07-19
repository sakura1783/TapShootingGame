using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;  //UnityActionを利用する際には追加する
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "MoveEventSO", menuName = "Create MoveEventSO")]
public class MoveEventSO : ScriptableObject
{
    private const float moveLimit = -3000;


    /// <summary>
    /// MoveTypeに応じた移動方法を決定してイベントとして登録
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public UnityAction<UnityEngine.Transform, float> GetMoveEvent(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.Straight:
                return MoveStraight;

            case MoveType.Meandering:
                return MoveMeandering;

            case MoveType.Boss_Horizontal:
                return MoveBossHorizontal;

            default:
                return Stop;
        }
    }

    /// <summary>
    /// 直進移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    private void MoveStraight(UnityEngine.Transform tran, float duration)
    {
        Debug.Log("直進");

        tran.DOLocalMoveY(moveLimit, duration);
    }

    /// <summary>
    /// 蛇行移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    private void MoveMeandering(UnityEngine.Transform tran, float duration)
    {
        Debug.Log("蛇行");

        tran.DOLocalMoveX(tran.position.x + 150, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        tran.DOLocalMoveY(moveLimit, duration);
    }

    /// <summary>
    /// ボス・水平移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    private void MoveBossHorizontal(UnityEngine.Transform tran, float duration)
    {
        tran.localPosition = new Vector3(0, tran.localPosition.y, tran.localPosition.z);

        tran.DOLocalMoveY(-500, 3f).OnComplete(() =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x + 400, 2.5f).SetEase(Ease.Linear));
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x - 400, 5f).SetEase(Ease.Linear));
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x, 2.5f).SetEase(Ease.Linear));
            sequence.AppendInterval(1).SetLoops(-1, LoopType.Restart);
        });
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="diration"></param>
    private void Stop(UnityEngine.Transform tran, float diration)
    {
        Debug.Log("停止");
    }
}
