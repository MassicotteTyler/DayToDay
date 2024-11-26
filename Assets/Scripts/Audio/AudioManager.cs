using System;
using Events;
using UnityEngine;
using Utility;

namespace Audio
{
    /// <summary>
    /// Manager for audio.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager> 
    {
        /// <summary>
        /// The <see cref="AudioSource"/> attached to this GameObject.
        /// </summary>
        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
           AudioEvent.OnPlayAudioClipEvent += PlaySound;
           AudioEvent.OnPlayAudioClipAtPositionEvent += PlaySoundAtPosition;
        }

        private void OnDestroy()
        {
            AudioEvent.OnPlayAudioClipEvent -= PlaySound;
            AudioEvent.OnPlayAudioClipAtPositionEvent -= PlaySoundAtPosition;
        }

        /// <summary>
        /// Play the specified sound.
        /// </summary>
        /// <param name="clip">The sound to play.</param>
        public void PlaySound(AudioClip clip)
        {
            // Play the sound.
            _audioSource.PlayOneShot(clip);
        }
        
        /// <summary>
        /// Play the specified sound at the specified position.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="position"></param>
        public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
        {
            // Play the sound at the specified position.
           AudioSource.PlayClipAtPoint(clip, position); 
        }
    }
}