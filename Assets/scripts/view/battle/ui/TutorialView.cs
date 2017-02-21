using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialView : MonoBehaviour {

    public static string anim_idle = "tutorial_idle";
    public static string anim_tap = "tutorial_tap";
    public Animator animator;


    bool isPlayerTurn = false;
    public GameObject prefabMark;
    public GameObject playbackmarker;
    public Vector2 barRange;

    List<KeyValuePair<int, GameObject>> marks = new List<KeyValuePair<int, GameObject>>();

    void Start ()
    {

        BeatController.instance.OnPatternTick += OnPatternTick;
	}


    void Update()
    {
        var prog = BeatController.instance.tick / 32f;


        var pos = playbackmarker.transform.position;
        pos.x = barRange.x + (barRange.y - barRange.x) * prog;
        playbackmarker.transform.position = pos;
    }

    public void SetPlayerTurn(bool val)
    {
        isPlayerTurn = val;
        if(!isPlayerTurn)
        {
            for (int i = marks.Count - 1; i >= 0; i--)
            {
                Destroy(marks[i].Value);
                marks.RemoveAt(i);
            }
        }
    }

    public void OnPlayerHit()
    {
        RemoveMark(BeatController.instance.getClosestTick());
    }

    public void OnPlayerMiss()
    {

    }



    void OnPatternTick()
    {
        animator.Play(anim_tap);

        if(!isPlayerTurn)
        {
            AddMark(BeatController.instance.tick);
        }
    }
    

    void ShowFinger(bool val)
    {

    }

    void AddMark(int index)
    {
        var indexMapped = index / 2;
        var obj = (GameObject)GameObject.Instantiate(prefabMark, new Vector3(barRange.x + (barRange.y - barRange.x) * (indexMapped / 15f), 4.08f, -1f), Quaternion.identity);
        obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        marks.Add(new KeyValuePair<int, GameObject>(index, obj));
    }

    void RemoveMark(int index)
    {
        for(int i = marks.Count-1; i >= 0; i--)
        {
            Debug.Log(index.ToString() + " , " + marks[i].Key.ToString());
            if(marks[i].Key == index)
            {
                Destroy(marks[i].Value);
                marks.RemoveAt(i);
                break;
            }
        }
    }
}
