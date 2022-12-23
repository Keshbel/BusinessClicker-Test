using UnityEngine;

public class AudioBase : MonoBehaviour
{
    public AudioSource audioSource;
    
    public AudioClip clickButton;
    public AudioClip coinCollection;

    public void ClickButton()
    {
        audioSource.PlayOneShot(clickButton);
    }

    public void CoinCollection()
    {
        audioSource.PlayOneShot(coinCollection);
    }
    
    #region Singleton

    public static AudioBase Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    #endregion
}
