using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ElementType
{
    Leaf,
    Fire,
    Ice,
    Thunder,
    Water,
    Darkness,
}

[CreateAssetMenu(fileName = "ElementDataSO", menuName = "Create ElementDataSO")]
public class ElementDataSO : ScriptableObject
{
    public List<ElementData> elementDataList = new();


    [Serializable]
    public class ElementData
    {
        public int elementNo;
        public ElementType elementType;
    }
}
