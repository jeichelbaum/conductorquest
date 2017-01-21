using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float bpm = 120f;
    float beatInterval = 0f;
    float tickInterval = 0f;

    public bool selectRandomPattern = false;

    float time = 0f;
    public int tick = -1;
    public int beat = -1;
    public int bar = -1;
    public bool isPlaying = false;
    
    List<int> r1 = new List<int>(new int[] { 0, 4, 8, 12, 16, 20, 24 });
    List<int> r3 = new List<int>(new int[] { 0, 4, 6, 8, 12, 16, 18, 20, 24 });
    List<int> r2 = new List<int>(new int[] { 0, 4, 10, 14, 20, 24 });
    List<int> r4 = new List<int>(new int[] { 0, 4, 7, 10, 16, 20, 23, 26 });

    int patternIndex = -1;
    List<int>[] patterns;
    List<int> pattern;

    void Awake()
    {
        beatInterval = 60f / bpm;
        tickInterval = beatInterval / 4f;

        patterns = new List<int>[] { r1, r2, r3, r4 };

        instance = this;
    }

    void FixedUpdate()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        if (!isPlaying) return;
        time += Time.deltaTime;

        if (time > tickInterval) // TICK
        {
            time -= tickInterval;
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
        if(OnBarUpdate != null)
        {
            OnBarUpdate();
        }
    }

    public void StartPlaying()
    {
        isPlaying = true;
    }

    public void StopPlaying()
    {
        time = 0f;
        tick = bar = beat = -1;
        isPlaying = false;
    }

    public void SelectPattern(int index)
    {
        pattern = patterns[index];
    }

    public void SelectPatterRandom()
    {
        if (selectRandomPattern)
        {
            var p = Random.Range(0, patterns.Length);
            pattern = patterns[p];
        } 
        else
        {
            patternIndex = (patternIndex + 1) % patterns.Length;
            pattern = patterns[patternIndex];
        }
    }

    public float getDistanceClosestTick()
    {
        var closest = 0;
        var dist = 100;
        foreach(var t in pattern)
        {
            var newDist = Mathf.Abs(tick - t);
            if(newDist < dist)
            {
                closest = t;
                dist = newDist;
            }
        }
    
        return Mathf.Abs(closest * tickInterval - (tick * tickInterval + time));
    }
}
