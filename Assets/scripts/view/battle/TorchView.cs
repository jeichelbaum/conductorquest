using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchView : MonoBehaviour {

    public static string anim_bounce = "torch_bounce";

    static float t_lastUpdate = 0f;

    static int randomIndex = -1;
    static bool state = false;

    Animator animator;
    public SpriteRenderer renderer;
    public Sprite[] torchSprites;

	void Start ()
    {
        animator = GetComponent<Animator>();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;	
	}
	
    void OnBeatUpdate()
    {
        if(Mathf.Abs(t_lastUpdate - Time.time) > 0.1f)
        {
            t_lastUpdate = Time.time;

            randomIndex = Random.Range(0, 2);
            state = !state;
        }

        if (BeatController.instance.tick == 28)
        {
            renderer.sprite = torchSprites[4];
        }
        else
        {
            renderer.sprite = torchSprites[randomIndex + (state ? 0 : 2)];
        }

        animator.Play(anim_bounce);
    }
}
