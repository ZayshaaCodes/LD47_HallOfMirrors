using UnityEngine;

public class HitSounds : MonoBehaviour, IDestroyOnClone
{
    [SerializeField] private AudioClip hitClip;
    
    private AudioSource source;
    [SerializeField] private float _hitSoundScale = .2f;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        source.PlayOneShot(hitClip,  other.relativeVelocity.magnitude * _hitSoundScale);
    }
}
