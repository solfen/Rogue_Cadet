using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour {

    [SerializeField] private string inputName = "Submit";
    [SerializeField] private GameObject hintBtn;

    private IInteractable interactableObject;

    void Start () {
        interactableObject = GetComponent<IInteractable>();
        if (interactableObject == null)
            Debug.LogError("No Interactable component on: " + gameObject.name);
    }

    void OnTriggerEnter2D() {
        StartCoroutine("WaitForInput");
        hintBtn.SetActive(true);
    }

    void OnTriggerExit2D() {
        StopCoroutine("WaitForInput");
        hintBtn.SetActive(false);
    }

    IEnumerator WaitForInput() {
        while (!Input.GetButtonDown(inputName)) {
            yield return null;
        }

        interactableObject.Activate();
    }
}
