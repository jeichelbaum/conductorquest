﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorView : MonoBehaviour {

    Animator animator;

    string anim_conductorBounce = "conductor_utzn";

    public PosesNavMesh poses;
    SpriteRenderer pose;

    bool playFailEffect = true;
    public ParticleSystem failEffect;
    public ParticleSystem sparkleEffect;


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

        SoundManager.instance.PlaySlashFail();
        failEffect.Play();
        playFailEffect = false;
    }

    public void HideForIntro()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            child.SetActive(false);
        }
    }

    public void ShowConductorStanding(bool val)
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
        var nodepose = poses.GetNode(index).pose;
        pose.transform.position = nodepose.transform.position;
        pose.transform.localScale = nodepose.transform.localScale;
        pose.transform.localRotation = nodepose.transform.localRotation;
        pose.sprite = nodepose.GetComponent<SpriteRenderer>().sprite;

        ShowConductorStanding(false);
        nodeIndex = index;

        sparkleEffect.Play();
    }
}
