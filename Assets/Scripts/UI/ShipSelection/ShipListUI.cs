using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShipListUI : MonoBehaviour {

    [SerializeField] private GameObject shipUIPrefab;
    [SerializeField] private Vector2 UISize;
    [SerializeField] private ShipTypesUI shipTypes;

    private GameData gamedata;
    private Transform _transform;
    private RectTransform _rectTransform;
    private UIPosAnimator animator;
    private Vector2 UIHalfSize;
    private bool isTransitioning = false;
    private bool isDown = false;
    private int shipSelectedIndex;
    private int shipNb = 0;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        _rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<UIPosAnimator>();

        gamedata = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;

        UIHalfSize = UISize * 0.5f;
        shipNb = gamedata.shipsUIItems.Count;

        for (int i = 0; i < gamedata.shipsUIItems.Count; i++)  {
            ShipsUIItemData dataToSend = saveData.shipsInfo[gamedata.shipsUIItems[i].associatedShipIndex].isUnlocked ? gamedata.shipsUIItems[i] : null;
            GameObject obj = Instantiate(shipUIPrefab, _transform) as GameObject;
            obj.GetComponentInChildren<ShipUI>().Init(dataToSend, i, UIHalfSize);
        }

        _rectTransform.anchoredPosition = new Vector2(0.5f * Camera.main.pixelWidth, 0);

        shipSelectedIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(isDown) {
            return;
        }

	    if(isTransitioning) {
            if(Input.GetAxis("MoveX") > -0.5f && Input.GetAxis("MoveX") < 0.5f) {
                isTransitioning = false;
            }

            return;
        }

        if(Input.GetButtonDown("Submit")) {
            StartCoroutine(GoDown());
            isDown = true;
        }
        else if(shipSelectedIndex < shipNb - 1 && Input.GetAxis("MoveX") > 0.5f) {
            isTransitioning = true;
            StartCoroutine(animator.Animate("listSelectMove", new Vector2(_rectTransform.anchoredPosition.x - 0.5f * Camera.main.pixelWidth, 0)));
            shipSelectedIndex++;
            EventDispatcher.DispatchEvent(Events.SELECT_UI, null);
        }
        else if (shipSelectedIndex > 0 && Input.GetAxis("MoveX") < -0.5f) {
            isTransitioning = true;
            StartCoroutine(animator.Animate("listSelectMove", new Vector2(_rectTransform.anchoredPosition.x + 0.5f * Camera.main.pixelWidth, 0)));
            shipSelectedIndex--;
            EventDispatcher.DispatchEvent(Events.SELECT_UI, null);
        }
    }

    public void Open() {
        StartCoroutine(animator.Animate("listToggleVisibility", new Vector2(_rectTransform.anchoredPosition.x, 0)));
        isDown = false;
    }

    IEnumerator GoDown() {
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        isTransitioning = true;
        yield return StartCoroutine(animator.Animate("listToggleVisibility", new Vector2(_rectTransform.anchoredPosition.x, -Camera.main.pixelHeight)));
        shipTypes.PopDown(shipSelectedIndex);
    }
}
