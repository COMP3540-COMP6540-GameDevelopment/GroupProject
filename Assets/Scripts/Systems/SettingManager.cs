using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer; 
    // public static SettingManager instance;
    // public GameObject background; // 拖拽引用 Background 对象
    // public GameObject main;   
    public Slider volumeSlider;   

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        if (volumeSlider == null)
        {
            volumeSlider = GameObject.Find("MySlider").GetComponent<Slider>();
        }
     
    }

    // public void SetMainMenuUIActive()
    // {
    //     if (background != null)
    //         {
    //             background.SetActive(true);  // 激活 Background
    //         }
    //     if (main != null)
    //         {
    //             main.SetActive(true);        // 激活 Main 对象
    //         }
    // }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
    void Awake()
    {
        if (volumeSlider == null)
        {
            volumeSlider = GameObject.Find("MySlider").GetComponent<Slider>();
        }
        // if (instance != null && instance != this)
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        // instance = this;
        // DontDestroyOnLoad(gameObject); // 不在场景切换时销毁
    }
}
