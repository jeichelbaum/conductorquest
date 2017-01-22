using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarView : MonoBehaviour {

    public HeartView heart1;
    public HeartView heart2;
    public HeartView heart3;
    

    public void SetHealth(int health)
    {
        heart1.SetAlive(health >= 1);
        heart2.SetAlive(health >= 2);
        heart3.SetAlive(health >= 3);
    }
}
