using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCard : MonoBehaviour
{
    public Animator endAnim;
   public void HideEndCard()
    {
        endAnim.SetBool("Hide",true);
    }
}
