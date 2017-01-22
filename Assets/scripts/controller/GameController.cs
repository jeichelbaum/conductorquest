using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    public float t_hitThreshold = 0.4f;

    float t_introCountdown = 1f;

    bool turnPlayer = false;
    bool failed = false;
    bool tickNextIgnore = false;
    bool tickWaiting = false;
    float t_waiting = 0f;

    public ConductorView player;
    public Transform spawn_monster;
    MonsterView monster, monsterOld;
    public BackgroundView background;

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
                StartGame();
            }
        }
    }

    void StartGame()
    {
        BeatController.instance.StartPlaying();
        SpawnNewMonster();
    }

    void OnTickUpdate()
    {
        if (BeatController.instance.tick == 28)
        {
            OnTurnEnded();
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
            if(!tickNextIgnore)
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
        // start music again after 8 bars
        if (BeatController.instance.bar % 8 == 0)
        {
            SoundManager.instance.PlayMusic();
        }

    }


    void UpdateInput()
    {

        // only update during player turn
        if (!turnPlayer) return;

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
                OnMiss();
            }
        }
    }


    void SpawnNewMonster()
    {
        // fade old monster out
        if (monster != null)
        {
            background.PlaySwitchAnimation();
            monster.PlayAnimationFadeOut();
            monsterOld = monster;
        }

        // fade new monster

        var go = (GameObject)GameObject.Instantiate(MonsterConfigs.instance.GetRandomMonsterPrefab(), Vector3.zero, Quaternion.identity);
        go.transform.parent = spawn_monster;
        monster = go.GetComponent<MonsterView>();
        monster.PlayAnimationFadeIn();

        // update music
        BeatController.instance.SelectPatterRandom();
    }

    void OnMonsterDead()
    {
        SpawnNewMonster();
    }
    

    void OnMonsterTick()
    {
        SoundManager.instance.PlaySwordSlash(0);
    }

    void OnMiss()
    {
        failed = true;
        tickWaiting = tickNextIgnore = false;
        player.OnSlashFail();
    }

    void OnSlashFail()
    {
        SoundManager.instance.PlaySlashFail();
        player.OnSlashFail();
        failed = true;
        tickWaiting = tickNextIgnore = false;
    }

    void OnSlashCorrect()
    {
        SoundManager.instance.PlaySwordSlash(0);
        player.OnSlashCorrect();
        if (tickWaiting)
        {
            tickWaiting = false;
        }
        else
        {
            tickNextIgnore = true;
        }
    }

    void OnTurnEnded()
    {
        // switch turn
        turnPlayer = !turnPlayer;

        // play switching sound
        if (turnPlayer)
        {
            monster.SetArmRotationActive(false);
            SoundManager.instance.PlaySwitchEnemy();
        }
        else
        {
            player.OnTurnOver();
            SoundManager.instance.PlaySwitchEnemy();
            monster.SetArmRotationActive(true);
            if (!failed)
            {
                OnMonsterDead();
            }
        }

        // reset values
        failed = tickNextIgnore = tickWaiting = false;
    }
}
