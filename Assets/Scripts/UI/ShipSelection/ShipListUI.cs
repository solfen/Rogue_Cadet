using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShipListUI : MonoBehaviour {

    [SerializeField] private GameData gamedata;
    [SerializeField] private GameObject shipUIPrefab;
    [SerializeField] private Vector2 UISize;
    [SerializeField] private ShipTypesUI shipTypes;

    private Transform _transform;
    private RectTransform _rectTransform;
    private UIPosAnimator animator;
    private Vector2 UIHalfSize;
    private bool isTransitioning = false;
    private bool isDown = false;
    private int shipSelectedIndex;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        _rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<UIPosAnimator>();

        UIHalfSize = UISize * 0.5f;

        for (int i = 0; i < gamedata.shipsUIItems.Count; i++)  {
            //TODO: if is unlock only
            GameObject obj = Instantiate(shipUIPrefab, _transform) as GameObject;
            obj.GetComponentInChildren<ShipUI>().Init(gamedata.shipsUIItems[i], i, UIHalfSize);
        }

        _rectTransform.anchoredPosition = new Vector2(-0.5f * ((gamedata.shipsUIItems.Count - 3) / 2) * Camera.main.pixelWidth, 0);

        shipSelectedIndex = (gamedata.shipsUIItems.Count - 1) / 2;
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
        else if(shipSelectedIndex < gamedata.shipsUIItems.Count - 1 && Input.GetAxis("MoveX") > 0.5f) {
            isTransitioning = true;
            StartCoroutine(animator.Animate("listSelectMove", new Vector2(_rectTransform.anchoredPosition.x - 0.5f * Camera.main.pixelWidth, 0)));
            shipSelectedIndex++;
        }
        else if (shipSelectedIndex > 0 && Input.GetAxis("MoveX") < -0.5f) {
            isTransitioning = true;
            StartCoroutine(animator.Animate("listSelectMove", new Vector2(_rectTransform.anchoredPosition.x + 0.5f * Camera.main.pixelWidth, 0)));
            shipSelectedIndex--;
        }
    }

    public void Open() {
        StartCoroutine(animator.Animate("listToggleVisibility", new Vector2(_rectTransform.anchoredPosition.x, 0)));
        isDown = false;
    }

    IEnumerator GoDown() {
        isTransitioning = true;
        yield return StartCoroutine(animator.Animate("listToggleVisibility", new Vector2(_rectTransform.anchoredPosition.x, -Camera.main.pixelHeight)));
        shipTypes.PopDown(shipSelectedIndex);
    }
}
