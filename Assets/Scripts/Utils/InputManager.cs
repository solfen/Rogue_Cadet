using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static bool useGamedad;

    public enum GameButtonID {
        SHOOT,
        SWITCH_WEAPONS,
        SPECIAL,
        BOMB,
        SHOW_HITBOX
    }

    private static Dictionary<GameButtonID, string> defaultGamepadBind = new Dictionary<GameButtonID, string> {
        { GameButtonID.SHOOT, "RightTrigger" },
        { GameButtonID.SWITCH_WEAPONS, ((int)KeyCode.Joystick1Button2).ToString() },
        { GameButtonID.SPECIAL, ((int)KeyCode.Joystick1Button4).ToString() },
        { GameButtonID.BOMB, ((int)KeyCode.Joystick1Button8).ToString() },
        { GameButtonID.SHOW_HITBOX, ((int)KeyCode.Joystick1Button3).ToString() }
    };

    private static Dictionary<GameButtonID, string> defaultKeyboardBind = new Dictionary<GameButtonID, string> {
        { GameButtonID.SHOOT, ((int)KeyCode.Mouse0).ToString() },
        { GameButtonID.SWITCH_WEAPONS, ((int)KeyCode.LeftControl).ToString() },
        { GameButtonID.SPECIAL, ((int)KeyCode.LeftShift).ToString() },
        { GameButtonID.BOMB, ((int)KeyCode.Space).ToString() },
        { GameButtonID.SHOW_HITBOX, ((int)KeyCode.E).ToString() }
    };

    private static Dictionary<GameButtonID, IGameButton> buttons = new Dictionary<GameButtonID, IGameButton>();
    private static string savePref;

    // Use this for initialization
    void Awake () {
        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "") {
            useGamedad = true;
        }

        savePref = useGamedad ? "GameButton_JoyStick_" : "GameButton_KeyBoard_";
        Dictionary<GameButtonID, string> defaultKeys = useGamedad ? defaultGamepadBind : defaultKeyboardBind;

        foreach (GameButtonID id in System.Enum.GetValues(typeof(GameButtonID))) {
            string savedKey = PlayerPrefs.GetString(savePref + id, defaultKeys[id]);
            int tryParsed;
            bool isInt = int.TryParse(savedKey, out tryParsed);

            if(isInt) {
                BindButtonInput(id, (KeyCode)tryParsed);
            }
            else {
                BindButtonInput(id, savedKey);
            }
        }
    }

    public static bool GetButton(GameButtonID id) {
        return buttons[id].IsPressed();
    }

    public static bool GetButtonDown(GameButtonID id) {
        return buttons[id].IsPressedDown();
    }

    public static void BindButtonInput(GameButtonID id, string axisName) {
        buttons[id] = new AxisButton(axisName);
        PlayerPrefs.SetString(savePref + id, axisName);
    }

    public static void BindButtonInput(GameButtonID id, KeyCode key) {
        buttons[id] = new KeyButton(key);
        PlayerPrefs.SetString(savePref + id, ((int)key).ToString());
    }
}

public interface IGameButton {
    bool IsPressed();
    bool IsPressedDown();
}

public class KeyButton : IGameButton {

    private KeyCode key;

    public KeyButton(KeyCode _key) {
        key = _key;
    }

    public bool IsPressed() {
        return Input.GetKey(key);
    }

    public bool IsPressedDown() {
        return Input.GetKeyDown(key);
    }
}

public class AxisButton : IGameButton {
    private string axisName;
    private bool wasPressedLastTime = false;

    public AxisButton(string _axisName) {
        axisName = _axisName;
    }

    public bool IsPressed() {
        wasPressedLastTime = Input.GetAxisRaw(axisName) >= 0.5;
        return wasPressedLastTime;
    }

    //it's more a hack than a robust way. If it's called two times in the same frame, the second will retrurn false
    //Like wise if the last call was true, and then you stop asking, and you unpress it, and then you call again, it'll retrurn false. 
    //But I don't really want to call it each frame to be sure. It should suffice like that. Future me if you have problems, sorry... 
    public bool IsPressedDown() {
        bool isPressed = Input.GetAxisRaw(axisName) >= 0.5;
        bool isDown = !wasPressedLastTime && isPressed;
        wasPressedLastTime = isPressed;

        return isDown;
    }
}
