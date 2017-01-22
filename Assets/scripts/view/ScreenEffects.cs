using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffects : MonoBehaviour {

    public static string anim_idle = "screen_idle";
    public static string anim_slash = "screen_slash";


    Animator animator;
    public GameObject slash;
    public ParticleSystem bloodsplash;

    Vector3 posStart = Vector3.zero;
    Vector3 offset = Vector3.zero;
    float t_screenshake = 0f;



	void Start ()
    {
        posStart = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        animator = GetComponent<Animator>();	
	}

    void Update()
    {
        t_screenshake = Mathf.Max(0f, t_screenshake - Time.deltaTime);

        offset = new Vector3(Random.Range(-1f, 1f) * t_screenshake, Random.Range(-1f, 1f) * t_screenshake, 0f);
        transform.position = posStart + offset;
    }

    public void ShowSlash(Vector3 start, Vector3 finish)
    {
        ScreenShake(1f);
        bloodsplash.Play();
        animator.Play(anim_slash, -1, 0f);

        var dir = finish - start;
        var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        slash.transform.eulerAngles = new Vector3(0f, 0f, angle);

        slash.transform.position = (start + finish) / 2f;
    }

    public void ScreenShake(float strength)
    {
        t_screenshake = 0.3f;
    }
}
