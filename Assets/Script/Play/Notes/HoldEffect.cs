using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEffect : MonoBehaviour
{
    private Animator animator;
    [NonSerialized]
    public float duration;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        StartCoroutine(Destroy());
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(duration);
        Finish();
        yield return new WaitForSeconds(0.237f);
        Destroy(gameObject);
        yield break;
    }
    public void Finish()
    {
        animator.SetBool("isFinished", true);
    }
}
