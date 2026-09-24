using UnityEngine;


namespace Ach.Sfx
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sourcePrefab;
        [SerializeField] private int poolSize = 24;
        [SerializeField] private AudioClipsDataSO clips;
        
        private AudioSource[] _sources;
        private int _next;

        private void Awake()
        {
            _sources = new AudioSource[poolSize];

            for (int i = 0; i < poolSize; i++)
                _sources[i] = Instantiate(sourcePrefab, transform);
            
        }

        public void PlayAt(SoundType type, Vector3 position, float volumeScale = .6f)
        {
            AudioClip clip = clips.GetClip(type);
            if(clip == null)
                return;
            
            AudioSource source = _sources[_next];
            _next = (_next + 1) % _sources.Length;
            
            source.transform.position = position;
            source.clip = clip;
            source.volume = volumeScale;
            source.pitch = Random.Range(0.95f, 1.05f);
            source.Play();
        }
        
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        
    }

}
