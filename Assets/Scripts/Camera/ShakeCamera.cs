using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    Animator camAnim;
    public void camShake()
    {
        camAnim.SetTrigger("shake");
    }
}
