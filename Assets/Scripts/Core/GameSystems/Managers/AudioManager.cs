using Core.Interfaces;
using UnityEngine;

namespace Core.GameSystems.Managers
{
    public class AudioManager : ISystem
    {
        private static AudioManager instance;

        private AudioSource musicSource;
        private AudioSource sfxSource;

        private bool isMuted = false;

        private AudioManager() { }

        public static AudioManager CreateInstance()
        {
            if (instance == null)
            {
                instance = new AudioManager();
            }
            return instance;
        }

        public void Initialize()
        {
            GameObject audioObj = new GameObject("AudioManager");
            Object.DontDestroyOnLoad(audioObj);

            musicSource = audioObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;

            sfxSource = audioObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        public void Shutdown()
        {
            if (musicSource != null)
                Object.Destroy(musicSource.gameObject);
        }
        
        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.Play();
        }

        public void StopMusic() => musicSource.Stop();

        public void Mute()
        {
            isMuted = true;
            AudioListener.volume = 0f;
        }

        public void Unmute()
        {
            isMuted = false;
            AudioListener.volume = 1f;
        }

        public bool IsMuted() => isMuted;
        
        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            sfxSource.PlayOneShot(clip);
        }
    }
}
