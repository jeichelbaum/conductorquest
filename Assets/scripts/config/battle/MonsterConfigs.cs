using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConfigs : MonoBehaviour {

    public static MonsterConfigs instance;

    public Material[] colorPatterns;
    public GameObject[] monsterPrefabs;

    [HideInInspector]
    public List<Sprite> monsterHeads;
    [HideInInspector]
    public List<Sprite> monsterArmsLeft;
    [HideInInspector]
    public List<Sprite> monsterArmsRight;
    [HideInInspector]
    public List<Sprite> monsterLegs;

    public void UpdatePrefabs()
    {
        // clean init spritearrays
        monsterHeads = new List<Sprite>();
        monsterArmsLeft = new List<Sprite>();
        monsterArmsRight = new List<Sprite>();
        monsterLegs = new List<Sprite>();

        foreach (var prefab in monsterPrefabs)
        {
            // instantiate monster prefab
            var config = ((GameObject)GameObject.Instantiate(prefab)).GetComponent<BodyData>();
            config.gameObject.SetActive(false);

            // extract all sprites
            AddSpritesToList(config, config.GetBoneHead(), monsterHeads);
            AddSpritesToList(config, config.GetBoneArmLeft(), monsterArmsLeft);
            AddSpritesToList(config, config.GetBoneArmRight(), monsterArmsRight);
            AddSpritesToList(config, config.GetBoneLegs(), monsterLegs);
            
            DestroyImmediate(config.gameObject);
        }

        Debug.Log("Updated Monster Prefabs, numHeads:" + monsterHeads.Count.ToString() +
            ", numArmL: " + monsterArmsLeft.Count.ToString() +
            ", numArmR: " + monsterArmsRight.Count.ToString() +
            ", numLegs: " + monsterLegs.Count.ToString()   
        );
    }

    void AddSpritesToList(BodyData config, Transform bone, List<Sprite> sprites)
    {
        foreach (var child in config.GetBoneSprites(bone))
        {
            sprites.Add(child.GetComponent<SpriteRenderer>().sprite);
        }
    }

    void Awake()
    {
        instance = this;
    }

    public GameObject GetRandomMonsterPrefab()
    {
        return monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
    }

    public Sprite GetRandomHead()
    {
        return monsterHeads[Random.Range(0, monsterHeads.Count)];
    }
    public Sprite GetRandomArmRight()
    {
        return monsterArmsRight[Random.Range(0, monsterArmsRight.Count)];
    }
    public Sprite GetRandomArmLeft()
    {
        return monsterArmsLeft[Random.Range(0, monsterArmsLeft.Count)];
    }
    public Sprite GetRandomLegs()
    {
        return monsterLegs[Random.Range(0, monsterLegs.Count)];
    }
}
