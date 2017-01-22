using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenView : MonoBehaviour {

    public static string anim_show = "endscreen_show";
    public static string anim_show_idle = "endscreen_idle";
    public static string anim_hide = "endscreen_hide";

    Animator animator;

    public Text text_monster, text_accuracy, text_total;

    void Start ()
    {
        animator = GetComponent<Animator>();	
	}

    public void ShowGameOver(float monster, float accuracy)
    {

        if(accuracy != -1)
        {
            accuracy = Mathf.Max(1f - accuracy, 0.2f);
        }
        else
        {
            accuracy = 0f;
        }
       

        text_monster.text = monster.ToString();
        text_accuracy.text = (Mathf.Round(accuracy * 100)).ToString() + "%";

        var total = Mathf.Round(monster * accuracy * 100);
        text_total.text = total.ToString();

        animator.Play(anim_show);
    }

    public void SkipResultAnimation()
    {
        animator.Play(anim_show_idle);
    }
}
