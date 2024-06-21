using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class Step : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Hide(){
        animator.SetBool("isShow", false);
        animator.SetBool("isHide", true);
    }
    public void Show(){
        animator.SetBool("isShow", true);
        animator.SetBool("isHide", false);
    }
    public void Clear(){
        animator.SetBool("isShow", false);
        animator.SetBool("isHide", false);
    }
}
