using System.Collections.Generic;
using grid.utils;
using UnityEngine;

namespace grid.sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager: Singleton<SoundManager>
    {
        private AudioSource audioSource;
        [SerializeField] private List<AudioClip> sounds;
        
        private int lastRandomIndex = -1;

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomMarineSound()
        {
            var t = lastRandomIndex;
            while (t == lastRandomIndex)
            {
                lastRandomIndex = Random.Range(0, sounds.Count);    
            }
             
            var random = sounds[lastRandomIndex];
            audioSource.PlayOneShot(random);
        }
    }
}