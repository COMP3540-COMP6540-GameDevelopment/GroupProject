using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [System.Serializable]
   public class Sound
   {
    [Header("Audio clip")]
    public AudioClip clip;
    
    [Header("Audio grouping")]
    public AudioMixerGroup outputGroup;

    [Header("Audio volume")]
    [Range(0,1)]
    public float volume = 1;

    [Header("Opening Play")]
    public bool playOnAwake;

    [Header("Loop")]
    public bool loop;
   }


   public List<Sound> sounds;

   private Dictionary<string, AudioSource> audiosDic;
   private static AudioManager instance;

   private void Awake(){
        if (instance == null)
        {
            instance = this;
        
            transform.SetParent(null, false);

            DontDestroyOnLoad(gameObject);  

            audiosDic = new Dictionary<string, AudioSource>();
        }
        else
        {
            Destroy(gameObject);  
        }
   }

   private void Start(){
    foreach (var sound in sounds){
        GameObject obj = new GameObject(sound.clip.name);
        obj.transform.SetParent(transform);

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.playOnAwake = sound.playOnAwake;
        source.loop = sound.loop;
        source.volume = sound.volume;
        source.outputAudioMixerGroup = sound.outputGroup;

        if (sound.playOnAwake){
            source.Play();
        }

        audiosDic.Add(sound.clip.name, source);
    }
   }

   public static void PlayAudio(string name, bool isWait = false){
    if(instance.audiosDic.ContainsKey(name)){
        Debug.LogWarning($"{name} non-existent");
        return;
    }
    if(isWait){
        if(!instance.audiosDic[name].isPlaying){
            instance.audiosDic[name].Play();
        }
    }
    else{
        instance.audiosDic[name].Play();
    }
   }

   public static void StopAudio(string name){
    if(instance.audiosDic.ContainsKey(name)){
        Debug.LogWarning($"{name} non-existent");
        return;
    }
    instance.audiosDic[name].Stop();
   }


}
