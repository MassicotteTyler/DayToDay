using System;
using UnityEngine;
using Audio;

namespace Events
{
    public enum AudioEventType
    {
        PlayOnce,
        PlayAtPosition,
        SetAmbient
    }
    /// <summary>
    /// Event for playing audio.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Audio", order = 6)]
    public class AudioEvent : GameEvent 
    {
        /// <summary>
        /// Action to be invoked when audio clip is played.
        /// </summary>
        public static Action<AudioClip, float> OnPlayAudioClipEvent;
        
        /// <summary>
        /// Action to be invoked when audio clip is played at a position.
        /// </summary>
        public static Action<AudioClip, Vector3, float> OnPlayAudioClipAtPositionEvent;

        /// <summary>
        /// Action to be invoked when audio clip is played at a position.
        /// </summary>
        public static Action<AudioClip, float> OnSetAmbientEvent;

        /// <summary>
        /// The audio clips to play at random.
        /// </summary>
        [Tooltip("Audio clip sources")]
        [SerializeField] public AudioClip[] PossibleClips;

        /// <summary>
        /// The playback volume
        /// </summary>
        [Tooltip("Playback volume")]
        [Range(0.0f, 1.0f)]
        [SerializeField] public float Volume = 1.0f;

        /// <summary>
        /// Type of audio event
        /// </summary>
        [SerializeField] private AudioEventType _audioEventType;

        protected override void Execute(GameObject invoker = null)
        {
            base.Execute();

            switch (_audioEventType)
            {
                case AudioEventType.PlayAtPosition:
                    if(invoker)
                        OnPlayAudioClipAtPositionEvent?.Invoke(GetRandomClip(), invoker.transform.position, Volume);
                    break;
                case AudioEventType.SetAmbient:
                    OnSetAmbientEvent?.Invoke(GetRandomClip(), Volume);
                    break;
                default: //PlayOnce
                    OnPlayAudioClipEvent?.Invoke(GetRandomClip(), Volume);
                    break;
            }
        }

        /// <summary>
        /// Get a random clip using UnityEngine.Random
        /// </summary>
        AudioClip GetRandomClip()
        {
            if (PossibleClips == null || PossibleClips.Length <= 0) return null;

            return PossibleClips[UnityEngine.Random.Range(0, PossibleClips.Length)];
        }
    }
}