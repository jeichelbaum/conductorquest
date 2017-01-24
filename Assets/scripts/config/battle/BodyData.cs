using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ----------------------------------------------------------
// THE WHOLE CLASS SHOULD ONLY BE USED FROM WITHIN THE EDITOR
// ----------------------------------------------------------


public class BodyData : MonoBehaviour
{
    static string name_bone_body = "bone_body";
    static string name_bone_head = "bone_head";
    static string name_bone_armleft = "bone_armleft";
    static string name_bone_armright = "bone_armright";
    static string name_bone_legs = "bone_legs";
    static string name_bone_shadow = "bone_shadow";
    static string name_sprite = "sprite";

    public Vector3 head_offset;
    public Vector3 armleft_offset;
    public Vector3 armright_offset;
    public Vector3 legs_offset;
    public Vector3 shadow_offset;

    Transform b_body;
    Transform b_head;
    Transform b_armleft;
    Transform b_armright;
    Transform b_legs;
    Transform b_shadow;

    void OnValidate()
    {
        ValidateSetup();
    }

    // should only be called from the editor
    void ValidateSetup()
    {
        // get and check bones
        b_body = GetBoneBody();
        if (b_body == null) Debug.LogError("Missing Bone: body");
        b_head = GetBoneHead();
        if (b_head == null) Debug.LogError("Missing Bone: head");
        b_armleft = GetBoneArmLeft();
        if (b_armleft == null) Debug.LogError("Missing Bone: arm-left");
        b_armright = GetBoneArmRight();
        if (b_armright == null) Debug.LogError("Missing Bone: arm-right");
        b_legs = GetBoneLegs();
        if (b_legs == null) Debug.LogError("Missing Bone: legs");
        b_shadow = GetBoneShadow();
        if (b_shadow == null) Debug.LogError("Missing Bone: shadow");

        // update pivot positions
        GetBoneSprite(b_head).localPosition = head_offset;
        GetBoneSprite(b_armleft).localPosition = armleft_offset;
        GetBoneSprite(b_armright).localPosition = armright_offset;
        GetBoneSprite(b_legs).localPosition = legs_offset;
        GetBoneSprite(b_shadow).localPosition = shadow_offset;
    }

    // used to display bone pivots in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0f, 1f, .5f);
        Gizmos.DrawSphere(GetBoneSprite(b_legs).position, 0.5f);
        Gizmos.DrawSphere(GetBoneSprite(b_head).position, 0.4f);
        Gizmos.DrawSphere(GetBoneSprite(b_shadow).position, 0.4f);

        Gizmos.color = new Color(1f, 0f, 1f, .5f);
        Gizmos.DrawSphere(GetBoneSprite(b_armleft).position, 0.4f);
        Gizmos.DrawSphere(GetBoneSprite(b_armright).position, 0.4f);
    }

    public Transform GetBoneBody()
    {
        return transform.FindChild(name_bone_body);
    }
    public Transform GetBoneHead()
    {
        return transform.FindChild(name_bone_head);
    }
    public Transform GetBoneArmLeft()
    {
        return transform.FindChild(name_bone_armleft);
    }
    public Transform GetBoneArmRight()
    {
        return transform.FindChild(name_bone_armright);
    }
    public Transform GetBoneLegs()
    {
        return transform.FindChild(name_bone_legs);
    }
    public Transform GetBoneShadow()
    {
        return GetBoneLegs().FindChild(name_bone_shadow);
    }

    public Transform GetBoneSprite(Transform bone)
    {
        return bone.FindChild(name_sprite);
    }

    public List<Transform> GetBoneSprites(Transform bone)
    {
        var sprites = new List<Transform>();
        for(var i = 0; i < bone.childCount; i++)
        {
            if (bone.GetChild(i).gameObject.name == name_sprite)
            {
                sprites.Add(bone.GetChild(i));
            }
        }
        return sprites;
    }
}
