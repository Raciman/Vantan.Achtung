#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ach.Input;
using Ach.Units.Player;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace Ach.UI.Editor
{
    // Attach only in Play Mode through Run(). Uses real UI pointer events and Input System devices.
    public sealed class MenuFlowSmokeTest : MonoBehaviour
    {
        private Keyboard _keyboard;
        private Mouse _mouse;
        private int _fireEvents;
        private IInputService _input;
        private bool _previousRunInBackground;
        private readonly List<string> _passed = new List<string>();
        private readonly Stack<IEnumerator> _steps = new Stack<IEnumerator>();
        private readonly Dictionary<InputActionAsset, InputDevice[]> _originalDevices = new Dictionary<InputActionAsset, InputDevice[]>();

        public static void Run()
        {
            if (!Application.isPlaying || SceneManager.GetActiveScene().name != "MenuScene")
                throw new InvalidOperationException("Enter Play Mode in MenuScene first.");
            new GameObject("MenuFlowSmokeTest").AddComponent<MenuFlowSmokeTest>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _previousRunInBackground = Application.runInBackground;
            Application.runInBackground = true;
            _keyboard = InputSystem.AddDevice<Keyboard>("MenuTestKeyboard");
            _mouse = InputSystem.AddDevice<Mouse>("MenuTestMouse");
            SessionState.SetString("Achtung.MenuVerification", "RUNNING");
            _steps.Push(Verify());
        }

        // Advance nested enumerators in Update so assertions observe the runtime input state,
        // rather than Unity's separate Editor input state used by an editor command.
        private void Update()
        {
            try
            {
                while (_steps.Count > 0)
                {
                    var step = _steps.Peek();
                    if (!step.MoveNext()) { _steps.Pop(); continue; }
                    if (step.Current is IEnumerator nested) { _steps.Push(nested); continue; }
                    return;
                }
                SessionState.SetString("Achtung.MenuVerification", "PASS\n" + string.Join("\n", _passed));
                Debug.Log("Menu flow verification passed: " + string.Join("; ", _passed));
                Destroy(gameObject);
            }
            catch (Exception error)
            {
                SessionState.SetString("Achtung.MenuVerification", "FAIL: " + error.Message + "\n" + string.Join("\n", _passed));
                Debug.LogException(error);
                Destroy(gameObject);
            }
        }

        private IEnumerator Verify()
        {
            yield return Until(() => LocalizationSettings.InitializationOperation.IsDone, "Localization initialization");
            yield return Frames(5);
            yield return Click("JapaneseFlag");
            yield return Until(() => LocalizationSettings.SelectedLocale.Identifier.Code == "ja", "Japanese locale");
            yield return Until(() => GameObject.Find("PlayButton").GetComponentInChildren<TMP_Text>().text == "プレイ" &&
                                    GameObject.Find("PlayButton").GetComponentInChildren<TMP_Text>().font.name == "MenuJapanese" &&
                                    GameObject.Find("QuitButton").GetComponentInChildren<TMP_Text>().text == "ゲーム終了", "Japanese text and font");
            Check(GameObject.Find("PlayButton").GetComponentInChildren<TMP_Text>().text == "プレイ", "Japanese play label");
            Check(GameObject.Find("QuitButton").GetComponentInChildren<TMP_Text>().text == "ゲーム終了", "Japanese quit label");
            foreach (var text in FindObjectsByType<TMP_Text>())
                Check(text.font.HasCharacters(text.text, out uint[] missing, true, true), "Glyph coverage: " + text.text);
            ScreenCapture.CaptureScreenshot("Temp/MenuWork/main-menu-ja.png");
            yield return Frames(3);
            yield return Click("EnglishFlag");
            yield return Until(() => GameObject.Find("PlayButton").GetComponentInChildren<TMP_Text>().text == "PLAY" &&
                                    GameObject.Find("PlayButton").GetComponentInChildren<TMP_Text>().font.name == "MenuLatin", "English text and font");
            ScreenCapture.CaptureScreenshot("Temp/MenuWork/main-menu-en.png");
            yield return Frames(3);
            Pass("Both flag buttons, live English/Japanese text, font glyphs and screenshots");

            yield return Click("PlayButton");
            yield return Until(() => SceneManager.GetActiveScene().name == "GamePlayScene", "Play scene load");
            yield return Frames(3);
            var manager = FindAnyObjectByType<UIManager>();
            _input = Field<IInputService>(manager, "_input");
            Isolate(Field<InputSystem_Actions>(_input, "_actions").asset);
            _input.FirePressed += CountFire;
            Check(_input.GameplayEnabled && !manager.IsMenuOpen && Time.timeScale == 1f, "Gameplay input on entry");

            // A fresh press first, followed immediately by Escape, also tests the buffered intent.
            MouseState(1);
            yield return Frames(1);
            Check(_input.FireHeld && _fireEvents == 1, "Initial fire delivery");
            KeyState(Key.Escape);
            yield return Frames(1);
            Check(manager.IsMenuOpen && !_input.GameplayEnabled && Time.timeScale == 0f, "Escape opens pause");
            var intent = Field<PlayerIntentProvider>(FindAnyObjectByType<PlayerRoot>(), "_intent");
            Check(!_input.FireHeld && !intent.FireHeld && !intent.FirePressed, "Pause clears buffered fire");
            KeyState();
            MouseState(0);
            yield return Frames(2);
            MouseState(3);
            yield return Frames(3);
            Check(_fireEvents == 1 && !_input.FireHeld, "Paused pointer does not fire");
            ScreenCapture.CaptureScreenshot("Temp/MenuWork/pause-en.png");
            KeyState(Key.Escape);
            yield return Frames(2);
            Check(!manager.IsMenuOpen && _input.GameplayEnabled && Time.timeScale == 1f, "Escape closes pause");
            Check(!_input.FireHeld && _fireEvents == 1, "Held fire suppressed across resume");
            KeyState();
            MouseState(0);
            yield return Frames(3);
            MouseState(1);
            yield return Frames(1);
            Check(_input.FireHeld && _fireEvents == 2, "Fresh fire recovers after release");
            MouseState(0);
            yield return Frames(2);
            Pass("Escape toggles; pause clears intent; held fire is blocked until release and a new press");

            KeyState(Key.Escape);
            yield return Frames(2);
            KeyState();
            yield return Frames(2);
            yield return Click("ContinueButton");
            Check(!manager.IsMenuOpen && _input.GameplayEnabled && _fireEvents == 2, "Continue click must not fire");
            Pass("Continue works through EventSystem and its click does not shoot");

            KeyState(Key.Escape);
            yield return Frames(2);
            KeyState();
            yield return Frames(2);
            yield return Click("ExitToMenuButton");
            yield return Until(() => SceneManager.GetActiveScene().name == "MenuScene", "Return to main menu");
            Check(Time.timeScale == 1f && !_input.GameplayEnabled, "Main menu resets pause/input");
            Check(LocalizationSettings.SelectedLocale.Identifier.Code == "en", "Locale retained across scenes");
            yield return Click("PlayButton");
            yield return Until(() => SceneManager.GetActiveScene().name == "GamePlayScene", "Second gameplay entry");
            yield return Frames(3);
            manager = FindAnyObjectByType<UIManager>();
            KeyState(Key.Escape);
            yield return Frames(2);
            Check(manager.IsMenuOpen, "Single Escape after re-entry (no duplicate subscriptions)");
            KeyState();
            yield return Frames(2);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("ja");
            yield return Until(() => GameObject.Find("ContinueButton").GetComponentInChildren<TMP_Text>().text == "続ける", "Japanese pause localization");
            Check(GameObject.Find("ContinueButton").GetComponentInChildren<TMP_Text>().text == "続ける", "Japanese pause label");
            ScreenCapture.CaptureScreenshot("Temp/MenuWork/pause-ja.png");
            yield return Frames(3);
            yield return Click("ExitToMenuButton");
            yield return Until(() => SceneManager.GetActiveScene().name == "MenuScene", "Second menu return");
            yield return Click("EnglishFlag");
            yield return Frames(5);
            Pass("Return-to-menu loader, locale retention and repeated gameplay entry");
        }

        private IEnumerator Click(string name)
        {
            Isolate(EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset);
            var rect = GameObject.Find(name).GetComponent<RectTransform>();
            var position = RectTransformUtility.WorldToScreenPoint(null, rect.TransformPoint(rect.rect.center));
            MouseState(0, position);
            yield return Frames(2);
            var data = new PointerEventData(EventSystem.current) { position = position };
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);
            Check(hits.Count > 0 && hits[0].gameObject == rect.gameObject, "Raycast hits " + name);
            MouseState(1, position);
            yield return Frames(2);
            MouseState(0, position);
            yield return Frames(3);
        }

        private void Isolate(InputActionAsset asset)
        {
            if (!_originalDevices.ContainsKey(asset))
                _originalDevices.Add(asset, asset.devices.HasValue ? asset.devices.Value.ToArray() : null);
            asset.devices = new InputDevice[] { _keyboard, _mouse };
        }

        private void KeyState(params Key[] keys) => InputSystem.QueueStateEvent(_keyboard, new KeyboardState(keys));
        private void MouseState(ushort buttons, Vector2? position = null) =>
            InputSystem.QueueStateEvent(_mouse, new MouseState { buttons = buttons, position = position ?? new Vector2(1800, 80) });
        private static IEnumerator Frames(int count) { for (var i = 0; i < count; i++) yield return null; }
        private static IEnumerator Until(Func<bool> condition, string label)
        {
            var deadline = Time.realtimeSinceStartup + 15f;
            while (!condition())
            {
                Check(Time.realtimeSinceStartup < deadline, "Timeout: " + label);
                yield return null;
            }
        }
        private static T Field<T>(object target, string name) =>
            (T)target.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
        private void CountFire() => _fireEvents++;
        private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
        private void Pass(string message) => _passed.Add(message);

        private void OnDestroy()
        {
            Application.runInBackground = _previousRunInBackground;
            if (_input != null) _input.FirePressed -= CountFire;
            foreach (var pair in _originalDevices)
                if (pair.Key != null)
                    pair.Key.devices = pair.Value;
            if (_mouse != null && _mouse.added) InputSystem.RemoveDevice(_mouse);
            if (_keyboard != null && _keyboard.added) InputSystem.RemoveDevice(_keyboard);
        }
    }
}
#endif
