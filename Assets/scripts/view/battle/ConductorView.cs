using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorView : MonoBehaviour {

    Animator animator;

    string anim_conductorBounce = "conductor_utzn";

    public PosesNavMesh poses;

    int nodeIndex = 7;

	void Start () {

        animator = GetComponent<Animator>();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
	}

    void OnBeatUpdate()
    {
        animator.Play(anim_conductorBounce);
    }

    public void OnSlashCorrect()
    {
        // get possible moves and shuffle
        var choices = poses.GetPossibleNodes(nodeIndex);

        for (int i = 0; i < choices.Length; i++)
        {
            var temp = choices[i];
            int randomIndex = Random.Range(i, choices.Length);
            choices[i] = choices[randomIndex];
            choices[randomIndex] = temp;
        }

        var ticksLeft = BeatController.instance.getPatternTicksLeft();
        var shortestFinisher = poses.GetDistanceToFinisher(nodeIndex, ticksLeft);
        
        
        foreach (var c in choices)
        {
            if(ticksLeft == 0)
            {
                SlashToNode(8);
                break;
            }
            else if(ticksLeft <= shortestFinisher)
            {
                // follow the correct path to finishing
                if (poses.GetDistanceToFinisher(c, ticksLeft) == ticksLeft -1)
                {
                    SlashToNode(c);
                    break;
                }
            }
            else
            {
                // go to whatever node as long as its not number 2
                if (poses.GetDistanceToFinisher(c, ticksLeft) != -1 && c != 2)
                {
                    SlashToNode(c);
                    break;
                }
            }
        }









    }

    public void OnSlashFail()
    {

    }

    void ShowConductorStanding(bool val)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(val);
        }
    }

    public void SlashToNode(int index)
    {
        Camera.main.GetComponent<ScreenEffects>().ShowSlash(poses.GetNode(nodeIndex).pose.transform.position, poses.GetNode(index).pose.transform.position);


        // display node positions
        poses.GetNode(nodeIndex).pose.SetActive(false);
        poses.GetNode(index).pose.SetActive(true);

        ShowConductorStanding(false);



        nodeIndex = index != 8 ? index : 7;
    }
}
