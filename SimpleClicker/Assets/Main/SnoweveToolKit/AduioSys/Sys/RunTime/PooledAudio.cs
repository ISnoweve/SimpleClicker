using Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Interface;
using Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Manager;
using UnityEngine;

public class PooledAudio : MonoBehaviour, IPoolable
{
    private AudioSource _source;
    public AudioSource Source => _source ??= GetComponent<AudioSource>();

    public void OnSpawn() { }

    public void OnDespawn()
    {
        Source.Stop();
        Source.clip = null;
    }

    private void Update()
    {
        // 如果音效播放完畢，自動回收
        if (!Source.isPlaying && Source.clip != null)
        {
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}