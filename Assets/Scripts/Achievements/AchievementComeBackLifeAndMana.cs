using UnityEngine;
using System.Collections;

public class AchievementComeBackLifeAndMana : BaseAchievement {

    [SerializeField] private float minLife;
    [SerializeField] private float minMana;
    [SerializeField] private float maxLife;
    [SerializeField] private float maxMana;

    private Player player;
    private BaseSpecialPower specialPower;
    private bool hasGoneLow = false;
    
    void Awake() {
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_CREATED, OnSpecialPowerCreated);
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_CREATED, OnSpecialPowerCreated);
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
    }

    private void OnSpecialPowerCreated(object powerObj) {
        specialPower = (BaseSpecialPower)powerObj;

        if(player != null) {
            StartCoroutine(CheckConditions());
        }
    }

    private void OnPlayerCreated(object playerObj) {
        player = (Player)playerObj;

        if(specialPower != null) {
            StartCoroutine(CheckConditions());
        }
    }
	
    IEnumerator CheckConditions() {
        while(true) {
            yield return new WaitForSeconds(1);

            if (player != null && specialPower != null) {
                if(player.currentLife <= player.maxLife * minLife && specialPower.mana <= specialPower.maxMana * minMana) {
                    hasGoneLow = true;
                }
                else if(hasGoneLow && player.currentLife >= player.maxLife * maxLife && specialPower.mana >= specialPower.maxMana * maxMana) {
                    Unlock();
                }
            }
        }
 
    }
}
