using System;
using Events;
using UnityEngine;
using Utility;
using SceneManagement;

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

        /// <summary>
        /// A separate AudioSource for playing ambient audio. Allows for independent volume/settings control 
        /// </summary>
        private AudioSource _ambientAudioSource;
        
        /// <summary>
        /// The <see cref="AudioListener"/> attached to this GameObject. Used when there is no player audio listener.
        /// </summary>
        private AudioListener _audioListener;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _ambientAudioSource = gameObject.AddComponent<AudioSource>();
            _ambientAudioSource.loop = true;

            AudioEvent.OnPlayAudioClipEvent += PlaySound;
            AudioEvent.OnPlayAudioClipAtPositionEvent += PlaySoundAtPosition;
            AudioEvent.OnSetAmbientEvent += SetAmbientClip;
            SceneGroupManager.OnSceneGroupLoaded += HandleSceneGroupChange;
            
            _audioListener ??= gameObject.AddComponent<AudioListener>();
            _audioListener.enabled = false;
        }
        
        /// <summary>
        /// Enable the default <see cref="_audioListener"/>
        /// </summary>
        /// <param name="state">The state to set the AudioListener</param>
        public void EnableDefaultListener(bool state)
        {
            PlayAmbient(state);
            if (!_audioListener) return;
            _audioListener.enabled = state;
        }

        private void Start()
        {
            _audioListener.enabled = false;
        }

        private void OnDestroy()
        {
            AudioEvent.OnPlayAudioClipEvent -= PlaySound;
            AudioEvent.OnPlayAudioClipAtPositionEvent -= PlaySoundAtPosition;
            AudioEvent.OnSetAmbientEvent -= SetAmbientClip;
            SceneGroupManager.OnSceneGroupLoaded -= HandleSceneGroupChange;
        }

        /// <summary>
        /// To be triggered when a SceneGroup changes. Used to updated ambient audio.
        /// </summary>
        /// <param name="sceneGroup">The new <see cref="SceneGroup"/></param>
        private void HandleSceneGroupChange(SceneGroup sceneGroup)
        {
            if (!sceneGroup || sceneGroup.AudioEventsOnLoaded == null) return;

            foreach (AudioEvent audioEvent in sceneGroup.AudioEventsOnLoaded)
            {
                audioEvent?.Invoke();
            }
        }

        /// <summary>
        /// Sets the ambient clip and volume.
        /// Used for setting new clips.
        /// </summary>
        /// <remarks> Will stop playing if audioClip is null </remarks>
        /// <param name="ambientClip">Sound to play</param>
        /// <param name="volume">Volume from 0.0 - 1.0</param>
        public void SetAmbientClip(AudioClip audioClip, float volume)
        {
            if (!audioClip)
            {
                PlayAmbient(false);
                return;
            }

            _ambientAudioSource.clip = audioClip;
            _ambientAudioSource.volume = volume;
            PlayAmbient(true);
        }

        /// <summary>
        /// Method for Start/Stop ambient AudioSource.
        /// Used for pausing and resuming.
        /// </summary>
        /// <param name="state">start or stop</param>
        public void PlayAmbient(bool state)
        {
            if (_ambientAudioSource == null) return;

            if (state && _ambientAudioSource.clip)
            {
                _ambientAudioSource?.Play();
            }
            else
            {
                _ambientAudioSource?.Stop();
            }
        }

        /// <summary>
        /// Play the specified sound.
        /// </summary>
        /// <param name="clip">The sound to play.</param>
        public void PlaySound(AudioClip audioClip, float volume)
        {
            if (!audioClip) return;

            // Play the sound.
            _audioSource?.PlayOneShot(audioClip, volume);
        }
        
        /// <summary>
        /// Play the specified sound at the specified position.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="position"></param>
        public void PlaySoundAtPosition(AudioClip audioClip, Vector3 position, float volume)
        {
            if (!audioClip) return;

            // Play the sound at the specified position.
            AudioSource.PlayClipAtPoint(audioClip, position, volume); 
        }
    }
}