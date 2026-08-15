/*
7/5作成
作成者：ジャンウォンソク
サウンドプレイヤー

使用方法：
Assetフォルダ内にResourcesフォルダがあること
その中のsoundフォルダを生成し、soundの中にse, bgmフォルダを生成
このスクリプトはフォルダ内のファイル名をキーバリューにしてAudioClipを返却するDictinaryを初期化時作成します。

使う方法は
SoundPlayer.Instance.PlaySE("ファイル名");
BGMは
SoundPlayer.Instance.PlayBGM("ファイル名");

BGMの場合は、次のシーンに行く前にどこかでStopBGM()で再生を中止すること
*/
using UnityEngine;
using System.Collections.Generic;

public enum SoundType
{
    TitleBGM = 1,
    TitleSE = 2
}

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance; //static変数

    //再生用
    private AudioSource bgmSource;
    private AudioSource seSource;
    private AudioSource pitchedSeSource;

    //Dictionary
    private Dictionary<string, AudioClip> seDictionary;

    private Dictionary<SoundType, SoundData> dict = new();

    [SerializeField] private SoundDB database;

    //ファイル経路
    private const string SePath = "sound/se";
    //private const string BgmPath = "sound/bgm";

    void Awake()
    {
        //シングルトーン化
        if (Instance != null && Instance != this) { 
            Destroy(gameObject); 
            return; 
        }

        Instance = this;
        //BGM再生セットアップ
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = 0.5f;
        //SE再生セットアップ
        seSource = gameObject.AddComponent<AudioSource>();
        seSource.playOnAwake = false;
        pitchedSeSource = gameObject.AddComponent<AudioSource>();
        pitchedSeSource.playOnAwake = false;

        //フォルダからサウンド情報読み込み
        seDictionary = LoadSoundList(SePath);
        //bgmDictionary = LoadSoundList(BgmPath);

        foreach(var s in database.clips){
            if(dict.ContainsKey(s.type)){
                Debug.LogWarning("SoundPlayer:重複キーがあります"+s.type);
            }

            dict[s.type] = s;
        }

        DontDestroyOnLoad(gameObject);
    }

    private Dictionary<string, AudioClip> LoadSoundList(string path)
    {
        var dict = new Dictionary<string, AudioClip>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);
        //例外処理
        if(clips.Length==0){
            Debug.LogWarning("SoundPlayer :"+path+"にファイルがありません");

            return dict;
        }

        foreach(AudioClip clip in clips){
            if(dict.ContainsKey(clip.name)){
                Debug.LogWarning("SoundPlayer:"+clip.name+"が重複されています");
            }
            dict[clip.name] = clip;
        }

        return dict;
    }

    public void PlayBGM(SoundType soundType)
    {
        if(!dict.TryGetValue(soundType, out var s)){
            Debug.LogWarning("SoundPlayer:サウンドが存在しません"+soundType);
            return;
        }

        bgmSource.clip = s.clip;
        bgmSource.Play();

    }

    //nameに相当するSE再生
    public void PlaySE(string name, float volume = 1f, float pitchOffset = 0f)
    {
        //例外処理
        if (!seDictionary.TryGetValue(name, out AudioClip clip))
        {
            Debug.LogWarning("SoundPlayer : SE"+name+"が見つかりませんでした。");
            return;
        }

        AudioSource source;
        if(pitchOffset!=0){
            source = pitchedSeSource;
        }
        else{
            source = seSource;
        }
        source.PlayOneShot(clip, volume);   
    }

    public void PlaySE(SoundType soundType)
    {
        AudioSource source = seSource;
        if(!dict.TryGetValue(soundType, out var s)){
            Debug.LogWarning("SoundPlayer:サウンドが存在しません"+soundType);
            return;
        }

        source.pitch = 1f + Random.Range(-s.pitch, s.pitch);
        source.PlayOneShot(s.clip);
    }

    //BGM再生終了
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    
}
