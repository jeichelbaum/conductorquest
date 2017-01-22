using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialView : MonoBehaviour {

    public static string anim_idle = "tutorial´_idle";

    public static string anim_go_idle = "tutorial_go_idle";
    public static string anim_go_fadein = "tutorial_go_fadein";
    public static string anim_go_fadeout = "tutorial_go_fadeout";

    public static string anim_press_idle = "tutorial_space_idle";
    public static string anim_press_bounce = "tutorial_space_press";


    Animator animator;
    bool showBounces = true;

    void Start ()
    {
        animator = GetComponent<Animator>();

        BeatController.instance.OnPatternTick += OnPatternTick;
	}
	
    
    void OnPatternTick()
    {
        if (!showBounces) return;
        animator.Play(anim_press_bounce);
    }


    public void ShowSpace()
    {
        animator.Play(anim_go_fadeout);
        showBounces = true;
    }

    public void ShowGo()
    {
        animator.Play(anim_go_fadein);
        showBounces = false;
    }
}
