using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    public enum BGMType
    {
        Battle,
        Silence = 999,
    }

    [System.Serializable]
    public class BGMData
    {
        public int bgmNo;
        public BGMType bgmType;
        public float volume = 0.05f;
        public AudioClip bgmAudioClip;
    }

    public List<BGMData> bgmDataList = new();
}
