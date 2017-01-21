using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float t_hitThreshold = 0.4f;

    float t_introCountdown = 1f;

    bool turnPlayer = true;
    bool failed = false;
    bool tickNextIgnore = false;
    bool tickWaiting = false;
    float t_waiting = 0f;

	void Start ()
    {
        BeatController.instance.OnTickUpdate += OnTickUpdate;
        BeatController.instance.OnPatternTick += OnPatternTick;
        BeatController.instance.OnBarUpdate += OnBarUpdate;
    }
	
	void Update ()
    {
        UpdateCountdown();
        UpdateInput();
	}

    void UpdateCountdown()
    {
        if (t_introCountdown > 0f)
        {
            t_introCountdown -= Time.deltaTime;
            if (t_introCountdown < 0f)
            {
                StartSound();
            }
        }
    }

    void StartSound()
    {
        BeatController.instance.StartPlaying();
    }

    void OnTickUpdate()
    {
        if (BeatController.instance.tick == 14)
        {
            if (!turnPlayer)
            {
                SoundManager.instance.PlaySwitchEnemy();
            }
            else
            {
                SoundManager.instance.PlaySwitchEnemy();
            }
        }
    }

    void OnPatternTick()
    {
        if (!turnPlayer)
        {
            OnMonsterTick();
        }
        else
        {
            if(!tickNextIgnore && !failed)
            {
                tickWaiting = true;
                t_waiting = 0f;
            }
            else
            {
                tickNextIgnore = false;
            }
        }
    }


    void OnBarUpdate()
    {
        turnPlayer = !turnPlayer;
        if (!turnPlayer)
        {
            SpawnNewMonster();
        }
    }


    void UpdateInput()
    {
        // only update during player turn
        if (!turnPlayer || failed) return;

        // check if input came at the right time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var dist = BeatController.instance.getDistanceClosestTick();
            if(dist < t_hitThreshold)
            {
                OnSlashCorrect();
            }
            else
            {
                OnSlashFail();
            }
        }

        // check if slash comes too late
        if (tickWaiting)
        {
            t_waiting += Time.deltaTime;
            if (t_waiting > t_hitThreshold)
            {
                OnSlashFail();
            }
        }
    }


    void SpawnNewMonster()
    {
        BeatController.instance.SelectPatterRandom();
        failed = tickNextIgnore = tickWaiting = false;
    }

    void OnMonsterTick()
    {
        SoundManager.instance.PlaySwordSlash(0);
    }

    void OnSlashFail()
    {
        SoundManager.instance.PlaySlashFail();
        failed = true;
        tickWaiting = tickNextIgnore = false;
    }

    void OnSlashCorrect()
    {
        SoundManager.instance.PlaySwordSlash(0);
        if (tickWaiting)
        {
            tickWaiting = false;
        }
        else
        {
            tickNextIgnore = true;
        }
    }
}
