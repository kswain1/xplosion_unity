using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class SwingManager : MonoBehaviour {

    public Animation anim;
    void Start()
    {
        anim = GetComponent<Animation>();
        AnimationCurve curve = AnimationCurve.Linear(0.0F, 1.0F, 2.0F, 0.0F);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        anim.AddClip(clip, "test");
        anim.Play("test");
    }
}
