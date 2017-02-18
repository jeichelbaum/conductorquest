using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BeatController : MonoBehaviour
{
    public static BeatController instance;

    public delegate void PatternTick();
    public event PatternTick OnPatternTick;

    public delegate void TickUpdate();
    public event TickUpdate OnTickUpdate;
    public delegate void BeatUpdate();
    public event BeatUpdate OnBeatUpdate;
    public delegate void BarUpdate();
    public event BarUpdate OnBarUpdate;


    public delegate void LastPatternTick();
    public event LastPatternTick OnLastPatternTick;

    public float bpm = 120f;
    [HideInInspector]
    public float beatInterval = 0f;
    float tickInterval = 0f;

    float time = 0f;
    [HideInInspector]
    public int tick = -1;
    [HideInInspector]
    public int beat = -1;
    [HideInInspector]
    public int bar = -1;
    [HideInInspector]

    public bool isPlaying
    {
        get
        {
            return soundManager.musicChannel.isPlaying;
        }
    }

    // only 4ths
    List<int> r1_2 = new List<int>(new int[] { 0, 4, 8, 16, 20, 24 });
    /*List<int> r1_1 = new List<int>(new int[] { 0, 4, 8, 16, 20, 24 });
    List<int> r1_2 = new List<int>(new int[] { 0, 4, 8, 12, 16, 20, 24 });
    List<int> r1_3 = new List<int>(new int[] { 0, 4, 8, 12, 16, 20, 24 });
    List<int> r1_4 = new List<int>(new int[] { 0, 4, 12, 16, 20, 24 });
    List<int> r1_5 = new List<int>(new int[] { 0, 8, 12, 16, 24 });

    // only 4th double time
    List<int> r2_1 = new List<int>(new int[] { 0, 4, 6, 8, 12, 16, 18, 20, 24 });
    List<int> r2_2 = new List<int>(new int[] { 0, 2, 4, 8, 10, 12, 16, 18, 20, 24 });
    List<int> r2_3 = new List<int>(new int[] { 0, 4, 6, 8, 10, 12, 16, 18, 20, 22, 24 });
    
    // offbeat #1
    List<int> r3_1 = new List<int>(new int[] { 0, 4, 10, 14, 20, 24 });
    List<int> r3_2 = new List<int>(new int[] { 0, 4, 10, 14, 16, 20, 24 });
    List<int> r3_3 = new List<int>(new int[] { 0, 4, 10, 14, 20, 24 });
    List<int> r3_4 = new List<int>(new int[] { 0, 4, 10, 14, 20, 24 });

    // offbeat #2
    List<int> r4_1 = new List<int>(new int[] { 0, 4, 7, 10, 16, 20, 23, 26 });
    List<int> r4_2 = new List<int>(new int[] { 0, 4, 7, 10, 15, 18, 21, 23 });

    // offbeat #3
    List<int> r5_1 = new List<int>(new int[] { 0, 3, 4, 10, 14, 16, 19, 20, 26});
    List<int> r5_2 = new List<int>(new int[] { 0, 3, 4, 8, 12, 14, 19, 20, 26});

    List<int> r6_1 = new List<int>(new int[] { 0, 6, 12, 18, 24 });
    List<int> r6_2 = new List<int>(new int[] { 0, 6, 20, 24 });
    List<int> r6_3 = new List<int>(new int[] { 0, 9, 15, 24 });
    List<int> r6_4 = new List<int>(new int[] { 0, 2, 19 });*/

    int patternIndex = -1;
    public List<List<int>> patterns;
    List<int> pattern;

    SoundManager soundManager;
    int numBars = 0;

    void Awake()
    {
        soundManager = GetComponent<SoundManager>();

        beatInterval = 60f / bpm;
        tickInterval = beatInterval / 4f;

        float musicLength = soundManager.musicChannel.clip.length;
        numBars = (int)Mathf.Round(musicLength / (8f * beatInterval));

        pattern = r1_2;
        /*patterns = new List<int>[] {
            r1_1, r1_2, r1_3, r1_4, r1_5,
            r2_1, r2_2, r2_3,
            r3_1, r3_2, r3_3, r3_4,
            r4_1, r4_2,
            r5_1, r5_2,
            r6_1, r6_2, r6_3, r6_4
        };

        patterns = new List<int>[] {
            r1_1, r1_2, r1_3, r1_4, r1_5,
            r2_1, r2_2, r2_3
        };*/

        instance = this;
    }

    void FixedUpdate()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        if (!isPlaying) return;
        int t = (int) Mathf.Floor(soundManager.musicChannel.time / (tickInterval));
        
        if (t % 32 > tick || (t % 32 == 0 && tick > 0)) // TICK
        {
            tick = (tick + 1) % 32;

            if (tick % 4 == 0) // BEAT
            {
                beat = (beat + 1) % 8;

                if (beat == 0) // BAR
                {
                    bar++;
                    OnBar();
                }

                OnBeat();
            }

            OnTick();
        }

        
        time = soundManager.musicChannel.time - ((bar % numBars) * 32 + tick) * tickInterval;
    }

    void OnTick()
    {
        if (OnTickUpdate != null)
        {
            OnTickUpdate();
        }

        
        if (pattern.Contains(tick))
        {
            if (OnPatternTick != null)
            {
                OnPatternTick();
            }

            if(tick == pattern[pattern.Count-1] && OnLastPatternTick != null)
            {
                OnLastPatternTick();
            }
        }
    }

    void OnBeat()
    {
        if (OnBeatUpdate != null)
        {
            OnBeatUpdate();
        }
    }

    void OnBar()
    {
        if (OnBarUpdate != null)
        {
            OnBarUpdate();
        }
    }
    

    public void StopPlaying()
    {
        time = 0f;
        tick = bar = beat = -1;
    }

    public void SelectPattern(int index)
    {
        pattern = patterns[index];
    }

    public void SelectPatterRandom()
    {
        pattern = r1_2;
        //patternIndex = (patternIndex + 1) % patterns.Count;
        //pattern = patterns[patternIndex];
    }

    public float getDistanceClosestTick()
    {
        var closest = getClosestTick();
        return Mathf.Abs(closest * tickInterval - (tick * tickInterval + time));
    }

    public int getClosestTick()
    {
        var closest = 0;
        var dist = 100;
        foreach (var t in pattern)
        {
            var newDist = Mathf.Abs(tick - t);
            if (newDist < dist)
            {
                closest = t;
                dist = newDist;
            }
        }
        return closest;
    }

    public int getPatternTicksLeft()
    {
        int lastTick = 0;
        for(var i = 0; i < pattern.Count; i++)
        {
            if (Mathf.Round((tick * tickInterval + time) / tickInterval) >= pattern[i]) lastTick = i;
        }

        return pattern.Count - 1 - lastTick;
    }

    public float getBarTime()
    {
        return (float)tick * tickInterval + time;
    }

    public float getRoundedTick()
    {
        return Mathf.Round((tick * tickInterval + time) / tickInterval);
    }

    public float getRoundedBeat()
    {
        return Mathf.Round((tick * tickInterval + time) / beatInterval);
    }
}
