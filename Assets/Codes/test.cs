using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    float time = 0.5f;
    Color cur;
    float lerpness = 0;
    SpriteRenderer sr;
    // Use this for initialization
    void Start()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        cur = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time<0)
        {
            time = 0.5f;
            lerpness += 0.1f;
            sr.color=Color.Lerp(cur, Color.red, lerpness);
        }
    }
}
