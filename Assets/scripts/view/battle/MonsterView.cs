using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour {

    string anim_monsterBounce = "monster_bounce";
    string anim_monsterFadeIn = "monster_fadein";
    string anim_monsterfadeOut = "monster_fadeout";

    BodyData body;

    Animator animator;
    List<List<float>> poses = new List<List<float>>();

    bool rotateArms = true;
    bool dead = false;
    float t_dead = 0.2f;

    void Awake()
    {
        animator = GetComponent<Animator>();

        poses.Add(new List<float>(new float[] { -45f, -65f }));
        poses.Add(new List<float>(new float[] { -45f, 10f }));
        poses.Add(new List<float>(new float[] { 10f, -65f }));
        poses.Add(new List<float>(new float[] { 10f, 10f }));

        // make sure to remove those events
        BeatController.instance.OnBeatUpdate += OnBeatUpdate;
        BeatController.instance.OnPatternTick += OnPatternTick;
    }

    void Start()
    {
        body = transform.GetComponentInChildren<BodyData>();

        body.GetBoneHead().GetComponentInChildren<SpriteRenderer>().sprite = MonsterConfigs.instance.GetRandomHead();
        body.GetBoneArmLeft().GetComponentInChildren<SpriteRenderer>().sprite = MonsterConfigs.instance.GetRandomArmLeft();
        body.GetBoneArmRight().GetComponentInChildren<SpriteRenderer>().sprite = MonsterConfigs.instance.GetRandomArmRight();
        body.GetBoneLegs().GetComponentInChildren<SpriteRenderer>().sprite = MonsterConfigs.instance.GetRandomLegs();
    }

    
	
	void Update ()
    {
		if (dead)
        {
            t_dead -= Time.deltaTime;
            if(t_dead < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetArmRotationActive(bool val)
    {
        rotateArms = val;
    }


    int lastPose = -1;
    void OnPatternTick()
    {
        if (!rotateArms) return;


        int pose = lastPose;
        while (pose == lastPose)
        {
            pose = Random.Range(0, poses.Count);
        }
        lastPose = pose;

        body.GetBoneArmLeft().GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, poses[pose][0]);
        body.GetBoneArmRight().GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, poses[pose][1]);
    }

    void OnBeatUpdate()
    {
        if (dead) return;
        animator.Play(anim_monsterBounce);
    }


    public void PlayAnimationFadeIn()
    {
        animator.Play(anim_monsterFadeIn);
    }

    public void PlayAnimationFadeOut()
    {
        dead = true;
        animator.Play(anim_monsterfadeOut);
    }

    void OnDestroy()
    {
        BeatController.instance.OnBeatUpdate -= OnBeatUpdate;
        BeatController.instance.OnPatternTick -= OnPatternTick;
    }
}
