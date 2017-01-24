using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundView : MonoBehaviour {

    static string anim_idle = "background_idle";
    static string anim_switch = "background_switch";

    Animator animator;
    bool switching = false;
    float t_switching = 0f;

    public Sprite[] backgrounds;
    public SpriteRenderer bg_left;
    public SpriteRenderer bg_right;


    void Start ()
    {
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        if(switching && t_switching + 0.1f < Time.time)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(anim_idle))
            {
                bg_left.sprite = bg_right.sprite;
                switching = false;
            }
        }
    }
	
    public void PlaySwitchAnimation()
    {
        animator.Play(anim_switch);
        switching = true;
        t_switching = Time.time;

        bg_right.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
    }
}
