using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MonsterBeacon : MonoBehaviour, IInteractable {

    [SerializeField] private Animator dialogBox;
    [SerializeField] private GameObject firstDialogOption;
    [SerializeField] private GameObject exitBlock;
    [SerializeField] private GameObject reward;
    [SerializeField] private GameObject enemies;

    private float enemiesNb;
    private Animator explosionAnim;

    void Start() {
        explosionAnim = GetComponent<Animator>();
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    public void Activate() {
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
        explosionAnim.SetTrigger("Explode");
        dialogBox.SetTrigger("Close");

        StartCoroutine("WaitForAnimToFinish");
    }

    IEnumerator WaitForAnimToFinish() {
        yield return new WaitForSeconds(0.6f);

        gameObject.SetActive(false);
        exitBlock.SetActive(true);
        enemies.SetActive(true);

        enemiesNb = enemies.transform.childCount;
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    public void CancelAction() {
        Time.timeScale = 1;
        dialogBox.SetTrigger("Close");
    }

    private void OnEnemyDied(object useless) {
        enemiesNb--;
        if(enemiesNb <= 0) {
            exitBlock.SetActive(false);
            reward.SetActive(true);
            EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
        }
    }
}
