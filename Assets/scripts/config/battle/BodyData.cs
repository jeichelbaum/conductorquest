using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyData : MonoBehaviour {

    public static string bone_head = "bone_head";
    public static string bone_armleft = "bone_armleft";
    public static string bone_armright = "bone_armright";
    public static string bone_legs = "bone_legs";
    
    public Vector3 head_offset;
    public Vector3 armleft_offset;
    public Vector3 armright_offset;
    public Vector3 legs_offset;
   
    Transform b_head;
    Transform b_armright;
    Transform b_armleft;
    Transform b_legs;

    void OnValidate()
    {
        // get bones
        if (b_head == null) b_head = GetBoneHead();
        if (b_armright == null) b_armright = GetBoneArmRight();
        if (b_armleft == null) b_armleft = GetBoneArmLeft();
        if (b_legs == null) b_legs = GetBoneLegs();
        
        // update pivot positions
        b_head.GetChild(0).localPosition = head_offset;
        b_armright.GetChild(0).localPosition = armright_offset;
        b_armleft.GetChild(0).localPosition = armleft_offset;
        b_legs.GetChild(0).localPosition = legs_offset;
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, .5f);
        Gizmos.DrawSphere(b_head.position + head_offset, 0.5f);

        Gizmos.color = new Color(1f, 0f, 0f, .5f);
        Gizmos.DrawSphere(b_armleft.position + armleft_offset, 0.5f);
        Gizmos.DrawSphere(b_armright.position + armright_offset, 0.5f);

        Gizmos.color = new Color(1f, 0f, 1f, .5f);
        Gizmos.DrawSphere(b_legs.position + legs_offset, 0.5f);
    }



    public Transform GetBoneHead()
    {
        return transform.parent.FindChild(bone_head);
    }
    public Transform GetBoneArmLeft()
    {
        return transform.parent.FindChild(bone_armleft);
    }
    public Transform GetBoneArmRight()
    {
        return transform.parent.FindChild(bone_armright);
    }
    public Transform GetBoneLegs()
    {
        return transform.parent.FindChild(bone_legs);
    }
}
