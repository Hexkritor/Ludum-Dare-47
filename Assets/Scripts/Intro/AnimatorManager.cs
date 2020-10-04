using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{

    public List<Animator> animators;
    // Start is called before the first frame update
    public void BonfirePlay(string animation)
    {
        animators[0].Play(animation);
    }

    public void MagePlay(string animation)
    {
        animators[1].Play(animation);
    }
}
