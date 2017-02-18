using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    const string STATE_START = "STATE_START";
    const string STATE_INTRO = "STATE_INTRO";
    const string STATE_TUTORIAL = "STATE_TUTORIAL";
    const string STATE_GAME = "STATE_GAME";
    const string STATE_END = "STATE_END";
    string state = "";

    // ------ views controlled by game controller
    public IntroView intro;
    public TutorialView tutorial;
    public BackgroundView background;
    public EndScreenView endscreen;

    public HealthbarView healthbar;
    public ConductorView player;
    public Transform spawn_monster;
    MonsterView monster;

    // specifies acceptable delay for audio cue triggers
    public float t_hitThreshold = 0.4f;
    
    bool turnPlayer = false;        // toggles on each bar
    bool failed = false;            // active for a whole bar if failed
    bool tickNextIgnore = false;    // if input came just before tick
    bool tickWaiting = false;       // if waiting for input shortly after tick
    float t_waiting = 0f;           // keeping track of how long 

    // in game stats
    int health = 3;
    public int level = 0;
    float numSlashes = 0f;
    float totalDelay = 0f;
    
    // used to skip end screen
    float t_stateEnd = 0;

    
    void Start ()
    {
        EnterStateStart();
    }

    void Reset()
    {
        health = 3;
    }
    
    void Update()
    {
        switch (state)
        {
            case STATE_START:
                UpdateStateStart();
                break;
            case STATE_TUTORIAL:
                UpdateGameInput();
                break;
            case STATE_GAME:
                UpdateGameInput();
                break;
            case STATE_END:
                UpdateStateEnd();
                break;
            default:
                break;
        }
    }


    // ------------ 1. Start Screen
    // - wait for tap

    void EnterStateStart()
    {
        state = STATE_START;
        SpawnNewMonster();
        player.HideForIntro();
        tutorial.Hide();
        healthbar.Hide(true);
    }

    void UpdateStateStart()
    {
        if(InputButtonPressed())
        {
            EnterStateIntro();
        }
    }


    // ------------ 2. Intro
    // - play intro uninterupted

    void EnterStateIntro()
    {
        state = STATE_INTRO;
        intro.PlayIntro(EnterStateTutorial);
    }
    
    // ------------ 3. Tutorial
    // - core gameplay, immediate start
    // - show + update tutorial ui

    void EnterStateTutorial()
    {
        state = STATE_TUTORIAL;
        StartGame();
    }

    // ------------ 4. game
    // - 

    void EnterStateGame()
    {
        state = STATE_GAME;
        healthbar.Hide(false);
        StartGame();
    }
    

    void StartGame()
    {
        monster.SetTurnActive(false, false);
        player.ShowConductorStanding(true);
        tutorial.ShowInstructions();

        AddGameBeatListeners();
        SoundManager.instance.PlayMusic();
    }

    void AddGameBeatListeners()
    {
        BeatController.instance.OnTickUpdate += OnTickUpdate;
        BeatController.instance.OnPatternTick += OnPatternTick;
        BeatController.instance.OnBarUpdate += OnBarUpdate;
        BeatController.instance.OnLastPatternTick += OnLastPatternTick;
    }

    void RemoveGameBeatListeners()
    {
        BeatController.instance.OnTickUpdate -= OnTickUpdate;
        BeatController.instance.OnPatternTick -= OnPatternTick;
        BeatController.instance.OnBarUpdate -= OnBarUpdate;
        BeatController.instance.OnLastPatternTick -= OnLastPatternTick;
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
        if (BeatController.instance.bar % 4 == 0)
        {
            SoundManager.instance.PlayMusic();
        }

    }


    void UpdateGameInput()
    {

        // only update during player turn
        if (!turnPlayer) return;

        // check if input came at the right time
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
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
        if(!failed)
        {
            health--;
            healthbar.SetHealth(health);
        }

        failed = true;
        tickWaiting = tickNextIgnore = false;
        player.OnSlashFail();
    }

    void OnSlashFail()
    {   
        if (!failed)
        {
            health--;
            healthbar.SetHealth(health);
        }

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

        numSlashes++;
        var tickDistance = BeatController.instance.getDistanceClosestTick();
        tickDistance = tickDistance < t_hitThreshold * 0.1f ? 0f : tickDistance;
        totalDelay += tickDistance;
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
                if (health <= 0)
                {
                    OnGameOver();
                }
                monster.SetTurnActive(true);
            }
        }

        // reset values
        failed = tickNextIgnore = tickWaiting = false;
    }

    void OnGameOver()
    {
        healthbar.gameObject.SetActive(false);

        SoundManager.instance.EndMusic();
        BeatController.instance.StopPlaying();
        player.OnSlashFail();
        SoundManager.instance.PlayGameOver();
        tutorial.Hide();

        var accuracy = numSlashes > 0 ? totalDelay / numSlashes : -1f;
        endscreen.ShowGameOver(level, accuracy);
        
        t_stateEnd = 2f;
    }

    void UpdateStateEnd()
    {
        t_stateEnd -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            TryRestart();
        }
    }

    void TryRestart()
    {
        if(t_stateEnd > 0f)
        {
            t_stateEnd = 0f;
            endscreen.SkipResultAnimation();
            return;
        }

        // TODO : restart
    }

    bool InputButtonPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }

}
