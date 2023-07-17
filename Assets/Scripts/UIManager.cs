using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameClearSetCanvasGroup;


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
}
