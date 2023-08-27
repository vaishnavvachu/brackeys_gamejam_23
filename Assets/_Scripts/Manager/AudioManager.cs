using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioClip attackClip;
    [SerializeField] AudioClip playerHitClip;
    [SerializeField] AudioClip coinCollectedClip;
    [SerializeField] AudioClip enemyShotDownClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip collectibleCollectedClip;
    [SerializeField] AudioSource audioSource;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayAttackSound()
    {
        PlayAudioClip(attackClip);
    }
    public void PlayPlayerHitSound()
    {
        PlayAudioClip(playerHitClip);
    }

    public void PlayCoinCollectedSound()
    {
        PlayAudioClip(coinCollectedClip);
    }

    public void PlayEnemyShotDownSound()
    {
        PlayAudioClip(enemyShotDownClip);
    }

    public void PlayGameOverSound()
    {
        PlayAudioClip(gameOverClip);
    }
    
    public void PlayCollectibleCollectedSound()
    {
        PlayAudioClip(collectibleCollectedClip);
    }
    
    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
