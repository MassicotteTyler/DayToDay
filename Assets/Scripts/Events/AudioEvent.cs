using System;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Event for playing audio.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Game Events/Audio", order = 6)]
    public class AudioEvent : GameEvent 
    {
        /// <summary>
        /// Action to be invoked when audio clip is played.
        /// </summary>
        public static Action<AudioClip> OnPlayAudioClipEvent;
        
        /// <summary>
        /// Action to be invoked when audio clip is played at a position.
        /// </summary>
        public static Action<AudioClip, Vector3> OnPlayAudioClipAtPositionEvent;

        /// <summary>
        /// The audio clip to play.
        /// </summary>
        [SerializeField] public AudioClip clip;
        
        /// <summary>
        /// Whether to play the audio clip at a position. Will be played at the position of the invoker if true.
        /// </summary>
        [SerializeField] private bool playAtPosition;

        protected override void Execute(GameObject invoker = null)
        {
            base.Execute();
            if (playAtPosition && invoker)
            {
                OnPlayAudioClipAtPositionEvent?.Invoke(clip, invoker.transform.position);
                return;
            }
            OnPlayAudioClipEvent?.Invoke(clip);
        }
    }
}