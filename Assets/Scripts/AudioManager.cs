using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioSource> sourcesMusic = new List<AudioSource>();
    public List<AudioSource> sourcesMusicMiniGame = new List<AudioSource>();
    public List<AudioSource> sourcesSound = new List<AudioSource>();
    public List<AudioSource> sourcesVoice = new List<AudioSource>();
    public bool isSound, isMusic;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /*  private void Start()
      {
          PlayerPrefs.SetInt("REWARDTODAY", PlayerPrefs.GetInt("REWARDTODAY") + 1);
          Debug.Log(" Log in Day    :  " + PlayerPrefs.GetInt("REWARDTODAY"));
      }*/
    public void Clear()
    {
        sourcesSound.Clear();
        sourcesMusicMiniGame.Clear();
        sourcesMusic.Clear();
        sourcesVoice.Clear();
    }

    public void PlayBGMSound(string str)
    {
        //  if (ManagerAsset.IsExist(str, typeof(AudioClip)))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Audio/{str}");
            for (int i = 0; i < sourcesMusic.Count; i++)
            {
                if (!sourcesMusic[i].isPlaying)
                {
                    sourcesMusic[i].clip = clip;
                    if (PlayerPrefs.GetInt("MUSIC") != 1)
                    {
                        sourcesMusic[i].Play();
                    }

                    break;
                }
            }

            GameObject obj = new GameObject();
            AudioSource aus = obj.AddComponent<AudioSource>();
            sourcesMusic.Add(aus);
            aus.transform.SetParent(transform) ;
            aus.clip = clip;
            aus.volume = 1f;
            aus.loop = true;
            if (PlayerPrefs.GetInt("MUSIC") != 1)
            {
                aus.Play();
            }
        }
    }

    public void PlaySound(string str)
    {
        //  if (ManagerAsset.IsExist(str, typeof(AudioClip)))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Audio/{str}");
            for (int i = 0; i < sourcesSound.Count; i++)
            {
                if (!sourcesSound[i].isPlaying)
                {
                    sourcesSound[i].clip = clip;
                    if (PlayerPrefs.GetInt("SOUND") != 1)
                    {
                        sourcesSound[i].PlayOneShot(clip);
                    }

                    return;
                }
            }

            GameObject obj = new GameObject();
            AudioSource aus = obj.AddComponent<AudioSource>();
            aus.transform.SetParent(transform) ;
            sourcesSound.Add(aus);
            aus.clip = clip;
            aus.volume = 1f;
            if (PlayerPrefs.GetInt("SOUND") != 1)
            {
                aus.PlayOneShot(clip);
            }
        }
    }

    public void PlayVoice(string str)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{str}");
        for (int i = 0; i < sourcesVoice.Count; i++)
        {
            if (!sourcesVoice[i].isPlaying)
            {
                sourcesVoice[i].clip = clip;
                if (PlayerPrefs.GetInt("SOUND") != 1)
                {
                    sourcesVoice[i].PlayOneShot(clip);
                }

                return;
            }
        }

        GameObject obj = new GameObject();
        AudioSource aus = obj.AddComponent<AudioSource>();
        sourcesVoice.Add(aus);
        aus.clip = clip;
        if (PlayerPrefs.GetInt("SOUND") != 1)
        {
            aus.PlayOneShot(clip);
        }
    }

    public void StopPlaySound()
    {
        for (int i = 0; i < sourcesSound.Count; i++)
        {
            //if (sourcesSound[i].isPlaying)
            {
                sourcesSound[i].enabled = false;
            }
        }
    }

    public void StopPlayVoice()
    {
        for (int i = 0; i < sourcesVoice.Count; i++)
        {
            //if (sourcesSound[i].isPlaying)
            {
                sourcesVoice[i].enabled = false;
            }
        }
    }

    public void StopPlayMusic()
    {
        for (int i = 0; i < sourcesMusic.Count; i++)
        {
            if (sourcesMusic[i].isPlaying)
            {
                sourcesMusic[i].enabled = false;
                sourcesMusic[i].Pause();
            }
        }
    }

    public void ContinuePlaySound()
    {
        if (sourcesMusic.Count > 0)
        {
            for (int i = 0; i < sourcesSound.Count; i++)
            {
                if (!sourcesSound[i].enabled)
                {
                    sourcesSound[i].enabled = true;
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void ContinuePlayMusic()
    {
        if (sourcesMusic.Count > 0)
        {
            for (int i = 0; i < sourcesMusic.Count; i++)
            {
                // if (!sourcesMusic[i].isPlaying)
                {
                    sourcesMusic[i].enabled = true;
                    sourcesMusic[i].Play();
                }
            }
        }
    }

    public void PlayBGMMiniGame(string str)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{str}");
        for (int i = 0; i < sourcesMusicMiniGame.Count; i++)
        {
            if (!sourcesMusicMiniGame[i].isPlaying)
            {
                sourcesMusicMiniGame[i].clip = clip;
                if (PlayerPrefs.GetInt("MUSIC") != 1)
                {
                    sourcesMusicMiniGame[i].Play();
                }

                break;
            }
        }

        GameObject obj = new GameObject();
        AudioSource aus = obj.AddComponent<AudioSource>();
        sourcesMusicMiniGame.Add(aus);
        aus.clip = clip;
        aus.volume = 0.15f;
        aus.loop = true;
        if (PlayerPrefs.GetInt("MUSIC") != 1)
        {
            aus.Play();
        }
    }

    public void ContinuePlayBGMMiniGame()
    {
        if (sourcesMusicMiniGame.Count > 0)
        {
            for (int i = 0; i < sourcesMusicMiniGame.Count; i++)
            {
                if (PlayerPrefs.GetInt("MUSIC") != 1)
                {
                    sourcesMusicMiniGame[i].Play();
                }
            }
        }
    }

    public void StopPlayBGMMiniGame()
    {
        for (int i = 0; i < sourcesMusicMiniGame.Count; i++)
        {
            sourcesMusicMiniGame[i].Stop();
        }
    }

    public void StopPlayBGMSound()
    {
        for (int i = 0; i < sourcesMusic.Count; i++)
        {
            sourcesMusic[i].Stop();
        }
    }
}