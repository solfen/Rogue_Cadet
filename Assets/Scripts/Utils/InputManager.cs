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

    public enum GameAxisID {
        MOVE_X,
        MOVE_Y
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

    private static Dictionary<GameAxisID, string> defaultGamepadAxes = new Dictionary<GameAxisID, string> {
        { GameAxisID.MOVE_X, "LeftStickX" },
        { GameAxisID.MOVE_Y, "LeftStickY" }
    };

    private static Dictionary<GameAxisID, KeyCode[]> defaultKeyboardAxes = new Dictionary<GameAxisID, KeyCode[]> {
        { GameAxisID.MOVE_X, new KeyCode[2] { KeyCode.D, KeyCode.Q }  },
        { GameAxisID.MOVE_Y, new KeyCode[2] { KeyCode.Z, KeyCode.S }  },
    };

    private static Dictionary<GameButtonID, IGameButton> buttons = new Dictionary<GameButtonID, IGameButton>();
    private static Dictionary<GameAxisID, IGameAxis> axes = new Dictionary<GameAxisID, IGameAxis>();
    private static string savePref;

    // Use this for initialization
    void Awake () {
        UpdateInputDevice(Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "");
    }

    //Check to see if the player uses the gamepad or the keyboard.
    //If there's a switch, the input config is changed instantly.
    void Update () {
        bool newUseGamePadState;

        if (Input.GetAxisRaw("LeftStickX") != 0 || Input.GetAxisRaw("LeftStickY") != 0 || Input.GetAxisRaw("RightStickX") != 0 || Input.GetAxisRaw("RightStickY") != 0) {
            newUseGamePadState = true;
        }
        else if(Input.anyKeyDown) { // keydown includes most of gamepad buttons
            newUseGamePadState = SniffKeyPressed().ToString().Contains("Joystick");
        }
        else {
            return;
        }

        if(useGamedad != newUseGamePadState) {
            UpdateInputDevice(newUseGamePadState);
            EventDispatcher.DispatchEvent(Events.INPUT_DEVICE_CHANGED, useGamedad);
        }
    }

    private void UpdateInputDevice(bool isGamepad) {
        useGamedad = isGamepad;
        savePref = useGamedad ? "GameButton_JoyStick_" : "GameButton_KeyBoard_";
        BindSavedButtons();
        BindSavedAxes();
    }

    public static bool GetButton(GameButtonID id) {
        return buttons[id].IsPressed();
    }

    public static bool GetButtonDown(GameButtonID id) {
        return buttons[id].IsPressedDown();
    }

    public static float GetAxisRaw(GameAxisID id) {
        return axes[id].GetAxisRaw();
    }

    public static void BindButtonInput(GameButtonID id, string axisName) {
        buttons[id] = new AxisButton(axisName);
        PlayerPrefs.SetString(savePref + id, axisName);
    }

    public static void BindButtonInput(GameButtonID id, KeyCode key) {
        buttons[id] = new KeyButton(key);
        PlayerPrefs.SetString(savePref + id, ((int)key).ToString());
    }

    public static void BindAxisInput(GameAxisID id, string axisName) {
        axes[id] = new AxisAxis(axisName);
        PlayerPrefs.SetString(savePref + id, axisName);
    }

    public static void BindAxisInput(GameAxisID id, KeyCode positiveKey, KeyCode negativeKey) {
        axes[id] = new KeyAxis(positiveKey, negativeKey);
        PlayerPrefs.SetInt(savePref + id + "_Positive", (int)positiveKey);
        PlayerPrefs.SetInt(savePref + id + "_Negative", (int)negativeKey);
    }

    public static void BindSavedButtons() {
        Dictionary<GameButtonID, string> defaultKeys = useGamedad ? defaultGamepadBind : defaultKeyboardBind;

        foreach (GameButtonID id in System.Enum.GetValues(typeof(GameButtonID))) {
            string savedKey = PlayerPrefs.GetString(savePref + id, defaultKeys[id]);
            int tryParsed;
            bool isInt = int.TryParse(savedKey, out tryParsed);

            if (isInt) {
                BindButtonInput(id, (KeyCode)tryParsed);
            }
            else {
                BindButtonInput(id, savedKey);
            }
        }
    }

    public static void BindSavedAxes() {
        foreach (GameAxisID id in System.Enum.GetValues(typeof(GameAxisID))) {
            if (useGamedad) {
                BindAxisInput(id, PlayerPrefs.GetString(savePref + id, defaultGamepadAxes[id]));
            }
            else {
                BindAxisInput(id, (KeyCode)PlayerPrefs.GetInt(savePref + id + "_Positive", (int)defaultKeyboardAxes[id][0]), (KeyCode)PlayerPrefs.GetInt(savePref + id + "_Negative", (int)defaultKeyboardAxes[id][1]));
            }
        }
    }

    public static KeyCode SniffKeyPressed() {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKey(vKey)) {
                return vKey;
            }
        }

        return KeyCode.None;
    }
}

public interface IGameButton {
    bool IsPressed();
    bool IsPressedDown();
}

public interface IGameAxis {
    float GetAxisRaw();
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

public class KeyAxis : IGameAxis {

    private KeyCode postiveKey;
    private KeyCode negativeKey;

    public KeyAxis(KeyCode postive, KeyCode negative) {
        postiveKey = postive;
        negativeKey = negative;
    }

    public float GetAxisRaw() {
        float positive = Input.GetKey(postiveKey) ? 1 : 0;
        float negative = Input.GetKey(negativeKey) ? 1 : 0;
        return positive - negative;
    }
}

public class AxisAxis : IGameAxis {
    private string axisName;

    public AxisAxis(string _axisName) {
        axisName = _axisName;
    }

    public float GetAxisRaw() {
        return Input.GetAxisRaw(axisName);
    }
}
