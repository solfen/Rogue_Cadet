using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUI : MonoBehaviour {

	[SerializeField] private Text moneyText;

	void Start () {
        UpdateMoney();
    }

	public void UpdateMoney () {
        moneyText.text = "Money: " + GlobalData.instance.saveData.money + " $";
    }
}
