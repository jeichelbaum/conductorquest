using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenView : MonoBehaviour {

    public static string anim_show = "endscreen_show";
    public static string anim_hide = "endscreen_hide";

    Animator animator;

    public Text text_monster, text_accuracy, text_total;

    void Start ()
    {
        animator = GetComponent<Animator>();	
	}

    public void ShowGameOver(float monster, float accuracy)
    {
        text_monster.text = monster.ToString();
        text_accuracy.text = accuracy.ToString() + "%";

        var total = Mathf.Round(monster * accuracy * 100);
        text_total.text = total.ToString();

        animator.Play(anim_show);
    }
}
