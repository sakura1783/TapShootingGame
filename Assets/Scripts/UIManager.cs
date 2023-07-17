using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameClearSetCanvasGroup;
    [SerializeField] private CanvasGroup gameOverSetCanvasGroup;

    [SerializeField] private Text txtGameOver;


    /// <summary>
    /// ゲームクリア表示を隠す
    /// </summary>
    public void HideGameClearSet()
    {
        gameClearSetCanvasGroup.alpha = 0;
    }

    /// <summary>
    /// ゲームクリア表示を行う
    /// </summary>
    public void DisplayGameClearSet()
    {
        gameClearSetCanvasGroup.DOFade(1, 0.25f);
    }

    /// <summary>
    /// ゲームオーバー表示を隠す
    /// </summary>
    public void HideGameOverSet()
    {
        gameOverSetCanvasGroup.alpha = 0;
    }

    /// <summary>
    /// ゲームオーバー表示を行う
    /// </summary>
    public void DisplayGameOverSet()
    {
        gameOverSetCanvasGroup.DOFade(1, 1);

        string text = "Game Over";

        txtGameOver.DOText(text, 1.5f).SetEase(Ease.Linear);
    }
}
