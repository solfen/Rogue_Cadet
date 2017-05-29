using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CadetCadaver : MonoBehaviour, IInteractable {

    [SerializeField] private Animator dialogBox;
    [SerializeField] private GameObject firstDialogOption;
    [SerializeField] private List<GameObject> possibleRewards;

    private Animator explosionAnim;
    private bool destroyed = false;

    void Start() {
        explosionAnim = GetComponent<Animator>();
    }

    public void Activate() {
        if (destroyed)
            return;

        Time.timeScale = 0;
        dialogBox.SetTrigger("Open");
        StartCoroutine(SelectUIObject());
    }

    IEnumerator SelectUIObject() {
        yield return new WaitForSecondsRealtime(0.1f);
        EventSystem.current.SetSelectedGameObject(firstDialogOption);
    }

    public void ConfirmAction() {
        Time.timeScale = 1;
        destroyed = true;
        explosionAnim.SetTrigger("Explode");
        dialogBox.SetTrigger("Close");

        StartCoroutine("WaitForAnimToFinish");
    }

    IEnumerator WaitForAnimToFinish() {
        yield return new WaitForSecondsRealtime(0.6f);
        gameObject.SetActive(false);
        possibleRewards[Random.Range(0, possibleRewards.Count)].SetActive(true);
    }

    public void CancelAction() {
        Time.timeScale = 1;
        dialogBox.SetTrigger("Close");
    }
}
