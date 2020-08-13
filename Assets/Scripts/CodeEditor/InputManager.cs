using System.Collections.Generic;
using UnityEngine;

class InputManager : MonoBehaviour
{
    public float initialDelay = 0.5f;
    public float repeatKeyDelay = 0.05f;

    readonly Dictionary<KeyCode, KeyState> keys = new Dictionary<KeyCode, KeyState>()
    {
        { KeyCode.UpArrow, new KeyState() },
        { KeyCode.DownArrow, new KeyState() },
        { KeyCode.LeftArrow, new KeyState() },
        { KeyCode.RightArrow, new KeyState() },
        { KeyCode.Delete, new KeyState() },
        { KeyCode.Tab, new KeyState() },
    };

    public bool CheckKey(KeyCode key)
    {
        if (!keys.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Key {key} is not registered.");
        }

        if (!Input.GetKey(key))
        {
            return false;
        }

        if (Input.GetKeyDown(key))
        {
            keys[key].lastPhysicalPressTime = Time.time;
            return true;
        }

        if (Time.time - keys[key].lastPhysicalPressTime > initialDelay)
        {
            if (Time.time - keys[key].lastVirtualPressTime > repeatKeyDelay)
            {
                keys[key].lastVirtualPressTime = Time.time;
                return true;
            }
        }

        return false;
    }

    class KeyState
    {
        public float lastPhysicalPressTime;
        public float lastVirtualPressTime;
    }
}
