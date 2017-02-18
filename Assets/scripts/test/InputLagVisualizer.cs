using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputLagVisualizer : MonoBehaviour {

    public AudioSource audio;

    public Text output;

    float t_lastTick = 0f;
    bool waitingForInput = false;

    int delayIndex = 0;
    float[] delays = new float[5];

    void Start()
    {
        BeatController.instance.SelectPatterRandom();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
        BeatController.instance.OnBarUpdate += OnBarUpdate;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            var diff = (Time.time - t_lastTick);
            if(!waitingForInput) diff = Mathf.Max(BeatController.instance.beatInterval - diff, 0f);

            delays[delayIndex] = diff;
            delayIndex = (delayIndex + 1) % delays.Length;

            waitingForInput = false;
        }




        var avgDelay = 0f;
        foreach(var delay in delays)
        {
            avgDelay += delay;
        }
        avgDelay /= delays.Length;
        avgDelay = Mathf.Round(avgDelay * 1000f);


        output.text = avgDelay.ToString() + "ms";
    }


    void OnBeatUpdate()
    {
        t_lastTick = Time.time;
        waitingForInput = true;
        audio.Play();
    }

    void OnBarUpdate()
    {
        if (BeatController.instance.bar % 8 == 0)
        {
            SoundManager.instance.PlayMusic();
        }
    }
}
