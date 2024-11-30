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
        /// The audio clip to play.
        /// </summary>
        [Tooltip("Audio clip source")]
        [SerializeField] public AudioClip Clip;

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
                        OnPlayAudioClipAtPositionEvent?.Invoke(Clip, invoker.transform.position, Volume);
                    break;
                case AudioEventType.SetAmbient:
                    OnSetAmbientEvent?.Invoke(Clip, Volume);
                    break;
                default: //PlayOnce
                    OnPlayAudioClipEvent?.Invoke(Clip, Volume);
                    break;
            }
        }
    }
}