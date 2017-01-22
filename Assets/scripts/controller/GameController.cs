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

    public TutorialView tutorial;

    int health = 3;
    public HealthbarView healthbar;

    public ConductorView player;
    public Transform spawn_monster;
    MonsterView monster, monsterOld;
    public BackgroundView background;

    public int level = 0;

    void Start ()
    {
        BeatController.instance.OnTickUpdate += OnTickUpdate;
        BeatController.instance.OnPatternTick += OnPatternTick;
        BeatController.instance.OnBarUpdate += OnBarUpdate;
        BeatController.instance.OnLastPatternTick += OnLastPatternTick;
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

    void OnLastPatternTick()
    {
        if(turnPlayer && !failed)
        {
            monster.OnDeath();
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

    void OnLevelWon()
    {
        SpawnNewMonster();
        level++;
    }
    

    void OnMonsterTick()
    {
        SoundManager.instance.PlayRandomMonsterSound();
    }

    void OnMiss()
    {
        failed = true;
        tickWaiting = tickNextIgnore = false;
        player.OnSlashFail();
    }

    void OnSlashFail()
    {
        player.OnSlashFail();
        failed = true;
        tickWaiting = tickNextIgnore = false;
    }

    void OnSlashCorrect()
    {
        SoundManager.instance.PlayRandomSwordSlash();
        player.OnSlashCorrect();
        monster.OnAttacked();
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
            monster.SetTurnActive(false);
            SoundManager.instance.PlaySwordDraw();
            tutorial.ShowGo();
        }
        else
        {
            player.OnTurnOver();
            SoundManager.instance.PlaySwitchEnemy();
            tutorial.ShowSpace();
            if (!failed)
            {
                OnLevelWon();
            }
            else
            {
                health--;
                monster.SetTurnActive(true);
            }

            healthbar.SetHealth(health);
        }

        // reset values
        failed = tickNextIgnore = tickWaiting = false;
    }
}
