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


    public enum SEType
    {
        BossAlert,
        Attack,  //敵に攻撃がヒットした際
        Damage,  //敵の攻撃を受けた際
    }

    [System.Serializable]
    public class SEData
    {
        public int seNo;
        public SEType seType;
        public float volume = 0.2f;
        public AudioClip seAudioClip;
    }

    public List<SEData> seDataList = new();
}
