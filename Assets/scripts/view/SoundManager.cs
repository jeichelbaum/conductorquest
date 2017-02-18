using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public AudioClip switch_enemy;
    public AudioClip[] monster_sound;
    public AudioClip sword_draw;
    public AudioClip[] sword_slash;
    public AudioClip[] sword_clash;
    public AudioClip slash_fail;

    public AudioClip gameover;

    public AudioSource musicChannel;
    List<AudioSource> channels = new List<AudioSource>();
 
    void Awake()
    {
        instance = this;
    }

    void PlayClip(AudioClip clip, float volume = 1f)
    {
        AudioSource newChannel = null;
        foreach(var channel in channels)
        {
            if(!channel.isPlaying)
            {
                newChannel = channel;
                break;
            }
        }

        if (newChannel == null)
        {
            newChannel = gameObject.AddComponent<AudioSource>();
            channels.Add(newChannel);
        }


        newChannel.clip = clip;
        newChannel.volume = volume;
        newChannel.Play();
    }

    public void PlayMusic()
    {
        musicChannel.Play();
    }

    public void EndMusic()
    {
        musicChannel.Stop();
        BeatController.instance.StopPlaying();
    }

    public void PlaySwitchEnemy()
    {
        PlayClip(switch_enemy);
    }

    public void PlayRandomMonsterSound()
    {
        PlayMonsterSound(Random.Range(0, monster_sound.Length));
    }

    public void PlayMonsterSound(int index)
    {
        PlayClip(monster_sound[index]);
    }

    public void PlaySwordDraw()
    {
        PlayClip(sword_draw);
    }

    public void PlayRandomSwordSlash()
    {
        PlaySwordSlash(Random.Range(0, sword_slash.Length));
    }

    public void PlaySwordSlash(int index)
    {
        PlayClip(sword_slash[index], 0.8f);
        PlayClip(sword_clash[index]);
    }
    
    public void PlaySlashFail()
    {
        PlayClip(slash_fail);
    }

    public void PlayGameOver()
    {
        PlayClip(gameover);
    }
}
