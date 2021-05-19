using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    
    void OnEnable()
    {
        EventManager.OnGameFinish += SetAnimation;
    }

    private void OnDisable()
    {
        EventManager.OnGameFinish -= SetAnimation;
    }

    void SetAnimation()
    {
        GetComponent<Animator>().SetTrigger("Finish");
    }

    
}
