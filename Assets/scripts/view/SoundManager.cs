using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;


    public AudioClip music;

    public AudioClip switch_enemy;
    public AudioClip[] monster_sound;
    public AudioClip sword_draw;
    public AudioClip[] sword_slash;
    public AudioClip slash_fail;


    void Awake()
    {
        instance = this;
    }

    void PlayClip(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    public void PlayMusic()
    {
        PlayClip(music, 0.3f);
    }

    public void PlaySwitchEnemy()
    {
        PlayClip(switch_enemy);
    }

    public void PlayMonsterSound(int index)
    {
        PlayClip(monster_sound[index]);
    }

    public void PlaySwordDraw()
    {
        PlayClip(sword_draw);
    }

    public void PlaySwordSlash(int index)
    {
        PlayClip(sword_slash[index]);
    }
    
    public void PlaySlashFail()
    {
        PlayClip(slash_fail);
    }
}
