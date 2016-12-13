using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class ShipStatsUpgrade : BaseUpgrade {

	[SerializeField] private string statSaveField;
    private FieldInfo field;

    void Start() {
        field = typeof(SaveData).GetField(statSaveField);
        if(field == null) {
            Debug.LogError("Save data doesn't contain field: " + statSaveField);
        }
    }

    public override void Equip(SaveData dataToModify) {
        modififyStatSave(1, dataToModify);
    }

    public override void UnEquip(SaveData dataToModify) {
        modififyStatSave(-1, dataToModify); 
    }

    private void modififyStatSave(int amount, SaveData data) {
        int newValue = ((int)field.GetValue(data)) + 1 * amount;
        field.SetValue(data, newValue);
    }

}
