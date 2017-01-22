using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour {

    string anim_monsterBounce = "monster_bounce";
    string anim_monsterFadeIn = "monster_fadein";
    string anim_monsterfadeOut = "monster_fadeout";
    string anim_monsterdeath = "monster_death";

    BodyData body;
    Transform armleft;
    Transform armright;

    Animator animator;
    List<List<float>> poses = new List<List<float>>();

    bool turnMonster = true;
    bool dead = false;
    float t_dead = 0.2f;
    bool hurtState = false;
    float startScale = 0f;


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

        armleft = body.GetBoneArmLeft().GetChild(0).transform;
        armright = body.GetBoneArmRight().GetChild(0).transform;

        startScale = transform.localScale.x;
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

    public void SetTurnActive(bool val, bool update = true)
    {
        if (dead) return;
        turnMonster = val;
        if(update) UpdateHurtState(update);
    }


    int lastPose = -1;
    void OnPatternTick()
    {
        if (!turnMonster || dead) return;

        int pose = lastPose;
        while (pose == lastPose)
        {
            pose = Random.Range(0, poses.Count);
        }
        lastPose = pose;

        armleft.rotation = Quaternion.Euler(0f, 0f, poses[pose][0]);
        armright.rotation = Quaternion.Euler(0f, 0f, poses[pose][1]);

        if(BeatController.instance.getPatternTicksLeft() == 0)
        {
            armleft.rotation = Quaternion.Euler(0f, 0f, 0f);
            armright.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
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

    public void OnAttacked()
    {
        UpdateHurtState(!hurtState);
    }

    public void OnDeath()
    {
        animator.Play(anim_monsterdeath);
    }

    void UpdateHurtState(bool state)
    {
        hurtState = state;
        armleft.rotation = Quaternion.Euler(0f, 0f, poses[0][0]);
        armright.rotation = Quaternion.Euler(0f, 0f, poses[0][0]);
        transform.localScale = new Vector3(hurtState ? startScale : -startScale, transform.localScale.y, transform.localScale.z);
        transform.localPosition = hurtState ? Vector3.zero : new Vector3(3f, 0f, 0f);
    }

    void OnDestroy()
    {
        BeatController.instance.OnBeatUpdate -= OnBeatUpdate;
        BeatController.instance.OnPatternTick -= OnPatternTick;
    }
}
