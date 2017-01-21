using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConfigs : MonoBehaviour {

    public static MonsterConfigs instance;

    public GameObject[] monsterPrefabs;

    public Sprite[] monsterHeads;
    public Sprite[] monsterArmsRight;
    public Sprite[] monsterArmsLeft;
    public Sprite[] monsterLegs;

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
        return monsterHeads[Random.Range(0, monsterHeads.Length)];
    }
    public Sprite GetRandomArmRight()
    {
        return monsterArmsRight[Random.Range(0, monsterArmsRight.Length)];
    }
    public Sprite GetRandomArmLeft()
    {
        return monsterArmsLeft[Random.Range(0, monsterArmsLeft.Length)];
    }
    public Sprite GetRandomLegs()
    {
        return monsterLegs[Random.Range(0, monsterLegs.Length)];
    }
}
