using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using _Main.ToolKit.SingletonFeature;
using Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Manager;
using Sirenix.OdinInspector;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Mixer & Groups")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Prefabs")]
    [SerializeField] private GameObject audioSourcePrefab;

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 1.5f;

    [Header("ReadOnly")]
    [SerializeField] private AudioSource activeBGMSource;
    [SerializeField] private AudioSource inactiveBGMSource;
    private Coroutine _fadeCoroutine;

    #region Life Cycle

    protected override void Awake()
    {
        base.Awake();
        InitializeBGMPlayers();
    }

    private void InitializeBGMPlayers()
    {
        // 建立兩個 BGM 播放器用於淡入淡出
        activeBGMSource = gameObject.AddComponent<AudioSource>();
        inactiveBGMSource = gameObject.AddComponent<AudioSource>();

        activeBGMSource.outputAudioMixerGroup = bgmGroup;
        inactiveBGMSource.outputAudioMixerGroup = bgmGroup;
        
        activeBGMSource.loop = true;
        inactiveBGMSource.loop = true;
    }

    #endregion

    #region Volume Control
    // 音量設定 (volume 範圍 0.0001 到 1)
    public void SetBGMVolume(float volume) => SetMixerVolume("BGMVol", volume);
    public void SetSFXVolume(float volume) => SetMixerVolume("SFXVol", volume);

    private void SetMixerVolume(string parameterName, float volume)
    {
        // 將線性 0~1 轉換為分貝 -80~0
        float db = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20f;
        mixer.SetFloat(parameterName, db);
    }
    #endregion

    #region BGM Playback
    public void PlayBGM(AudioClip clip)
    {
        if (activeBGMSource.clip == clip) return;

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(CrossfadeBGM(clip));
    }

    private IEnumerator CrossfadeBGM(AudioClip newClip)
    {
        inactiveBGMSource.clip = newClip;
        inactiveBGMSource.volume = 0;
        inactiveBGMSource.Play();

        float timer = 0;
        float startVol = activeBGMSource.volume;

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            activeBGMSource.volume = Mathf.Lerp(startVol, 0, t);
            inactiveBGMSource.volume = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        activeBGMSource.Stop();
        
        // 交換角色
        var temp = activeBGMSource;
        activeBGMSource = inactiveBGMSource;
        inactiveBGMSource = temp;
    }
    #endregion

    #region SFX Playback
    // 2D 音效
    public void PlaySFX2D(AudioClip clip)
    {
        GameObject obj = PoolManager.Instance.Spawn(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        AudioSource s = obj.GetComponent<PooledAudio>().Source;
        
        s.outputAudioMixerGroup = sfxGroup;
        s.spatialBlend = 0f; // 2D
        s.clip = clip;
        s.Play();
    }

    // 3D 音效
    public void PlaySFX3D(AudioClip clip, Vector3 position)
    {
        GameObject obj = PoolManager.Instance.Spawn(audioSourcePrefab, position, Quaternion.identity);
        AudioSource s = obj.GetComponent<PooledAudio>().Source;

        s.outputAudioMixerGroup = sfxGroup;
        s.spatialBlend = 1f; // 3D
        s.minDistance = 1f;
        s.maxDistance = 20f; // 可根據需求調整或傳入參數
        s.clip = clip;
        s.Play();
    }
    
    public void PlaySFX3D(AudioClip clip, Vector3 position, float minDistance, float maxDistance)
    {
        GameObject obj = PoolManager.Instance.Spawn(audioSourcePrefab, position, Quaternion.identity);
        AudioSource s = obj.GetComponent<PooledAudio>().Source;

        s.outputAudioMixerGroup = sfxGroup;
        s.spatialBlend = 1f; // 3D
        s.minDistance = minDistance;
        s.maxDistance = maxDistance; // 可根據需求調整或傳入參數
        s.clip = clip;
        s.Play();
    }
    #endregion
}