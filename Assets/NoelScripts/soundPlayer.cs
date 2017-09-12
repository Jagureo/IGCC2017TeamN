using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class soundPlayer : MonoBehaviour
{

    public static soundPlayer m_Instance;

    bool b_InitialLoad = false;
    public Dictionary<string, AudioClip> AudioList = new Dictionary<string, AudioClip>();

    string CurrentScene;

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!m_Instance)
        {
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_InitialLoad)
        {
            AudioList.Add("LockedDoor1", Resources.Load("12740__leady__locked-door2", typeof(AudioClip)) as AudioClip);
            AudioList.Add("ErrorSound1", Resources.Load("142608__autistic-lucario__error", typeof(AudioClip)) as AudioClip);
            AudioList.Add("MugDrop", Resources.Load("179997__mohomran__mug-2", typeof(AudioClip)) as AudioClip);
            AudioList.Add("BushesSound1", Resources.Load("190638__gus-man__rustling-foilage-4", typeof(AudioClip)) as AudioClip);
            AudioList.Add("BushesSound2", Resources.Load("190642__gus-man__rustling-foilage-5", typeof(AudioClip)) as AudioClip);
            AudioList.Add("MugPickup", Resources.Load("240783__f4ngy__ceramic-set-down-2", typeof(AudioClip)) as AudioClip);
            AudioList.Add("LockedDoor2", Resources.Load("321087__benagain__door-locked", typeof(AudioClip)) as AudioClip);
            AudioList.Add("ErrorSound2", Resources.Load("351500__thehorriblejoke__error-sound", typeof(AudioClip)) as AudioClip);
            AudioList.Add("DoorSlam", Resources.Load("361167__funwithsound__door-slam", typeof(AudioClip)) as AudioClip);
            AudioList.Add("ErrorSound3", Resources.Load("363920__samsterbirdies__8-bit-error", typeof(AudioClip)) as AudioClip);
            AudioList.Add("MugPlace", Resources.Load("368614__bumblefly__cup-01", typeof(AudioClip)) as AudioClip);
            AudioList.Add("PlasticRustle", Resources.Load("377534__13fpanska-machacova-petra__rustling", typeof(AudioClip)) as AudioClip);
            AudioList.Add("DoorLock", Resources.Load("397314__designdean__double-door-lock", typeof(AudioClip)) as AudioClip);
            b_InitialLoad = true;
        }

    }

    public void PlaySoundEffect(string name)
    {
        GameObject.Find("EffectSoundPlayer").GetComponent<AudioSource>().clip = AudioList[name];
        GameObject.Find("EffectSoundPlayer").GetComponent<AudioSource>().Play();
    }

    public void StopSound()
    {
        GameObject.Find("EffectSoundPlayer").GetComponent<AudioSource>().Stop();
    }
}
