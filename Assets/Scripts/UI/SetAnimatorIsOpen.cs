using UnityEngine;
using System.Collections;

public class SetAnimatorIsOpen : MonoBehaviour {

    public void SetIsOpen() {
        GetComponent<Animator>().SetBool("IsOpen", true);
    }
}
