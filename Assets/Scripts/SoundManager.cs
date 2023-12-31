using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// 音源管理クラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public SoundDataSO soundDataSO;

    public const float crossFadeTime = 1f;  //クロスフェード時間

    //ボリューム関係
    public float bgmVolume = 0.1f;
    public float seVolume = 0.2f;
    public float voiceVolume = 0.2f;

    public bool isMute = false;

    private AudioSource[] bgmSources = new AudioSource[1];  //各オーティオファイル再生用のAudioSource
    private AudioSource[] seSources = new AudioSource[15];  //SE用のAudioSourceを代入するための変数(複数用意しているのは重複して鳴ることを想定)
    private AudioSource voiceSource = new AudioSource();

    private bool isCrossFading;

    //ボイスを分類ごとに仕分け
    private List<SoundDataSO.VoiceData> startVoiceList = new();
    private List<SoundDataSO.VoiceData> looseVoiceList = new();
    private List<SoundDataSO.VoiceData> winVoiceList = new();
    private List<SoundDataSO.VoiceData> attackVoiceList = new();
    private List<SoundDataSO.VoiceData> damageVoiceList = new();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //BGM用のAudioSource追加
        bgmSources[0] = gameObject.AddComponent<AudioSource>();

        //SE用のAudioSource追加
        for (int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        //ボイス用のAudioSource追加
        voiceSource = gameObject.AddComponent<AudioSource>();

        //ボイスを種類ごとに仕分けして各Listに追加
        AssortmentListsToVoices();
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="newBgmType"></param>
    /// <param name="loopFlg"></param>
    //public void PlayBGM(SoundDataSO.BGMType newBgmType, bool loopFlg = true)
    public void PlayBGM(SoundDataSO.BGMType newBgmType)
    {
        //BGMをSilenceの状態にする場合
        if ((int)newBgmType == 999)
        {
            StopBGM();

            return;  //returnでこのPlayBGMメソッドから出る
        }

        ////再生するBGMのBGMDataを取得
        SoundDataSO.BGMData newBgmData = null;

        foreach (SoundDataSO.BGMData bgmData in soundDataSO.bgmDataList.Where(x => x.bgmType == newBgmType))
        {
            newBgmData = bgmData;

            break;  //breakでこのforeach文から出る
        }

        //対象となるデータがなければ処理しない
        if (newBgmData == null)
        {
            return;
        }

        ////同じBGMの場合は何もしない
        //if (bgmSources[0].clip != null && bgmSources[0].clip == newBgmData.bgmAudioClip)
        //{
        //    return;
        //}
        //else if (bgmSources[1].clip != null && bgmSources[1].clip == newBgmData.bgmAudioClip)
        //{
        //    return;
        //}

        ////フェードでBGM開始
        //if (bgmSources[0].clip == null && bgmSources[1].clip == null)
        //{
        //    bgmSources[0].loop = loopFlg;
        //    bgmSources[0].clip = newBgmData.bgmAudioClip;
        //    bgmSources[0].volume = newBgmData.volume;
        //    bgmSources[0].Play();
        //}
        //else
        //{
        //    StartCoroutine(CrossFadeChangeBGM(newBgmData, loopFlg));
        //}

        bgmSources[0].clip = newBgmData.bgmAudioClip;
        bgmSources[0].volume = newBgmData.volume;
        bgmSources[0].Play();
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <param name="bgmData"></param>
    /// <param name="loopFlg"></param>
    /// <returns></returns>
    //private IEnumerator CrossFadeChangeBGM(SoundDataSO.BGMData bgmData, bool loopFlg)
    //{
    //    isCrossFading = true;

    //    //[0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
    //    if (bgmSources[0].clip != null)
    //    {
    //        bgmSources[1].DOFade(bgmData.volume, crossFadeTime);
    //        bgmSources[1].clip = bgmData.bgmAudioClip;
    //        bgmSources[1].loop = loopFlg;
    //        bgmSources[1].Play();
    //        bgmSources[0].DOFade(0, crossFadeTime);

    //        yield return new WaitForSeconds(crossFadeTime);
    //        bgmSources[0].Stop();
    //        bgmSources[0].clip = null;
    //    }
    //    //[1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
    //    else
    //    {
    //        bgmSources[0].DOFade(bgmData.volume, crossFadeTime);
    //        bgmSources[0].clip = bgmData.bgmAudioClip;
    //        bgmSources[0].loop = loopFlg;
    //        bgmSources[0].Play();
    //        bgmSources[1].DOFade(0, crossFadeTime);

    //        yield return new WaitForSeconds(crossFadeTime);
    //        bgmSources[1].Stop();
    //        bgmSources[1].clip = null;
    //    }

    //    isCrossFading = false;
    //}

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        bgmSources[0].Stop();
        //bgmSources[1].Stop();

        bgmSources[0].clip = null;
        //bgmSources[1].clip = null;
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        bgmSources[0].Stop();
        //bgmSources[1].Stop();
    }

    /// <summary>
    /// 一時停止した同じBGMを再生(再開)
    /// </summary>
    public void ResumeBGM()
    {
        bgmSources[0].Play();
        //bgmSources[1].Play();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="newSEType"></param>
    public void PlaySE(SoundDataSO.SEType newSEType)
    {
        SoundDataSO.SEData newSEData = null;

        foreach (SoundDataSO.SEData seData in soundDataSO.seDataList.Where(x => x.seType == newSEType))
        {
            newSEData = seData;

            break;
        }

        //再生中ではないAudioSourceを使ってSEをならす
        foreach (AudioSource source in seSources)
        {
            if (!source.isPlaying)
            {
                source.clip = newSEData.seAudioClip;
                source.volume = newSEData.volume;
                source.Play();

                return;
            }
        }
    }

    /// <summary>
    /// SE停止。全てのSE用のAudioSourceを停止する
    /// </summary>
    public void StopSE()
    {
        foreach (AudioSource source in seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    /// <summary>
    /// ボイスの種類ごとにListに仕分け
    /// </summary>
    private void AssortmentListsToVoices()
    {
        foreach (SoundDataSO.VoiceData voiceData in soundDataSO.voiceDataList)
        {
            switch (voiceData.voiceType)
            {
                case SoundDataSO.VoiceType.Start:
                    startVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Loose:
                    looseVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Win:
                    winVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Attack:
                    attackVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Damage:
                    damageVoiceList.Add(voiceData);
                    break;
            }
        }
    }

    /// <summary>
    /// 指定された種類のボイス再生。種類内に複数のボイスがある場合には、その中からランダムな一つを選んで再生
    /// </summary>
    /// <param name="voiceType"></param>
    public void PlayVoice(SoundDataSO.VoiceType voiceType)
    {
        //ボイス再生中は重複して再生をしない
        if (voiceSource.isPlaying)
        {
            return;
        }

        SoundDataSO.VoiceData voiceData = GetVoice(voiceType);

        voiceSource.clip = voiceData.voiceAudioClip;
        voiceSource.volume = voiceData.volume;
        voiceSource.Play();
    }

    /// <summary>
    /// 指定されたVoiceTypeよりListを設定し、List内にあるVoiceDataをランダムで一つ取得
    /// </summary>
    /// <param name="voiceType"></param>
    /// <returns></returns>
    private SoundDataSO.VoiceData GetVoice(SoundDataSO.VoiceType voiceType)
    {
        List<SoundDataSO.VoiceData> list = new();

        switch (voiceType)
        {
            case SoundDataSO.VoiceType.Start:
                list = startVoiceList;
                break;

            case SoundDataSO.VoiceType.Loose:
                list = looseVoiceList;
                break;

            case SoundDataSO.VoiceType.Win:
                list = winVoiceList;
                break;

            case SoundDataSO.VoiceType.Attack:
                list = attackVoiceList;
                break;

            case SoundDataSO.VoiceType.Damage:
                list = damageVoiceList;
                break;
        }

        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// ボイス停止
    /// </summary>
    public void StopVoice()
    {
        voiceSource.Stop();

        voiceSource.clip = null;
    }

    /// <summary>
    /// サウンドを初期化
    /// </summary>
    public void InitializeSound()
    {
        StopBGM();
        StopSE();
        StopVoice();
    }
}
