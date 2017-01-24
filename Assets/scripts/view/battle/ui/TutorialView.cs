using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialView : MonoBehaviour {

    public static string anim_idle = "tutorial_idle";
    public static string anim_instruction = "tutorial_instruction";

    public static string anim_go_idle = "tutorial_go_idle";
    public static string anim_go_fadein = "tutorial_go_fadein";
    public static string anim_go_fadeout = "tutorial_go_fadeout";

    public static string anim_press_idle = "tutorial_space_idle";
    public static string anim_press_bounce = "tutorial_space_press";


    public Animator animator;
    bool showBounces = true;
    bool instructions = false;

    void Start ()
    {

        BeatController.instance.OnPatternTick += OnPatternTick;
	}
	
    
    void OnPatternTick()
    {
        if (!showBounces) return;
        animator.Play(anim_press_bounce);
    }

    public void Hide()
    {
        animator.Play(anim_idle);
        showBounces = false;    
    }

    public void ShowInstructions()
    {
        animator.Play(anim_instruction);
        showBounces = false;
        instructions = true;
    }

    public void ShowSpace()
    {
        if (!instructions) animator.Play(anim_go_fadeout);
        else animator.Play(anim_press_idle);
        showBounces = true;
    }

    public void ShowGo()
    {
        animator.Play(anim_go_fadein);
        showBounces = false;
    }
}
