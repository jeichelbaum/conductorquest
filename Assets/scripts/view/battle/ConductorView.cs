using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorView : MonoBehaviour {

    Animator animator;

    string anim_conductorBounce = "conductor_utzn";

    public PosesNavMesh poses;
    SpriteRenderer pose;

    bool playFailEffect = true;
    public ParticleSystem failEffect;


    public Sprite spriteFail;
    public List<Sprite> spritePoses;

    int nodeIndex = 7;

	void Start () {
        pose = transform.FindChild("pose").GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
	}

    void OnBeatUpdate()
    {
        animator.Play(anim_conductorBounce);
    }

    public void OnTurnOver()
    {
        ShowConductorStanding(true);
        nodeIndex = 7;
        playFailEffect = true;
    }

    public void OnSlashCorrect()
    {
        var choices = poses.GetPossibleNodes(nodeIndex);
        SlashToNode(choices[Random.Range(0, choices.Length)]);
        playFailEffect = true;
    }

    public void OnSlashFail()
    {
        if (!playFailEffect) return;

        ShowConductorStanding(false);
        nodeIndex = 7 + (nodeIndex - 1) % 3;
        pose.transform.position = poses.GetNode(nodeIndex).pose.transform.position;
        pose.sprite = spriteFail;


        failEffect.Play();
        playFailEffect = false;
    }

    void ShowConductorStanding(bool val)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            child.SetActive(val);

        }

        pose.gameObject.SetActive(!val);
    }

    public void SlashToNode(int index)
    {
        Camera.main.GetComponent<ScreenEffects>().ShowSlash(
            poses.GetNode(nodeIndex).pose.transform.position, 
            poses.GetNode(index).pose.transform.position
        );

        // display node positions
        pose.transform.position = poses.GetNode(index).pose.transform.position;
        pose.sprite = spritePoses[Random.Range(0, spritePoses.Count)];

        ShowConductorStanding(false);
        nodeIndex = index;
    }
}
