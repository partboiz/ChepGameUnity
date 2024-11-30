using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float hp;
    public int level;
    public string name;

    public test()
    {

    }
    public test(float hb, int level, string name)
    {
        this.hp = hb;
        this.level = level; 
        this.name = name;
    }
    
}
