using UnityEngine;

namespace StuckInALoop
{
    [RequireComponent(typeof(AudioSource))]
    public class HitSounds : MonoBehaviour, IDestroyOnClone
    {
        [SerializeField] private AudioClip hitClip;
        [SerializeField] private float     hitSoundScale = .2f;

        private AudioSource _source;

        // Start is called before the first frame update
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            _source.PlayOneShot(hitClip, other.relativeVelocity.magnitude * hitSoundScale);
        }
    }
}