using UnityEngine;

public static class TransformHelper
{
    private static Transform temporaryObjectContainerTran;

    /// <summary>
    /// temporaryObjectContainerTran変数のプロパティ
    /// </summary>
    public static Transform TemporaryObjectContainerTran
    {
        get { return temporaryObjectContainerTran; }
        set { temporaryObjectContainerTran = value; }
    }


    /// <summary>
    /// temporaryObjectContainerTranに情報をセット
    /// </summary>
    /// <param name="newTran"></param>
    public static void SetTemporaryObjectContainerTran(Transform newTran)
    {
        temporaryObjectContainerTran = newTran;

        Debug.Log($"temporaryObjectContainerTranに位置情報をセット完了：{temporaryObjectContainerTran}");
    }

    /// <summary>
    /// temporaryObjectContainerTranの情報を取得
    /// </summary>
    /// <returns></returns>
    public static Transform GetTemporaryObjectContainerTran()
    {
        return temporaryObjectContainerTran;
    }
}
