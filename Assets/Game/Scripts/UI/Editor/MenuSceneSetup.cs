using System;
using System.Linq;
using Ach.UI;
using Reflex.Core;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.Localization;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.TextCore.LowLevel;

namespace Ach.UI.Editor
{
    // Authoring utility: the result is serialized into the scenes, never built at runtime.
    public static class MenuSceneSetup
    {
        private const string LayerLab = "Assets/Thirdparty/Layer Lab/GUI Pro-SurvivalClean/ResourcesData/";
        private const string MenuScene = "Assets/Scenes/MenuScene.unity";
        private const string GameplayScene = "Assets/Scenes/GamePlayScene.unity";
        private const string FontFolder = "Assets/Game/UI/Fonts/";
        private static readonly Color Amber = new Color32(237, 178, 87, 255);
        private static readonly Color Ink = new Color32(24, 29, 32, 255);
        private static TMP_FontAsset _latin;

        public static void CreateMenus()
        {
            if (EditorApplication.isPlaying)
                throw new InvalidOperationException("Create menus outside Play Mode.");
            for (var i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
                if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).isDirty)
                    throw new InvalidOperationException("Save current scene work before authoring menus.");

            SetupLocalization();
            var scenes = EditorBuildSettings.scenes.ToList();
            if (!scenes.Any(s => s.path == MenuScene))
                scenes.Add(new EditorBuildSettingsScene(MenuScene, true));
            else
                scenes.First(s => s.path == MenuScene).enabled = true;
            EditorBuildSettings.scenes = scenes.ToArray();
            var enabledScenes = scenes.Where(s => s.enabled).ToList();
            var menuIndex = enabledScenes.FindIndex(s => s.path == MenuScene);
            var gameplayIndex = enabledScenes.FindIndex(s => s.path == GameplayScene);

            var gameplay = EditorSceneManager.OpenScene(GameplayScene);
            var manager = UnityEngine.Object.FindAnyObjectByType<UIManager>();
            var canvas = UnityEngine.Object.FindAnyObjectByType<Canvas>();
            if (canvas.transform.Find("PauseMenu") != null)
                throw new InvalidOperationException("Pause menu already exists; edit it in place.");
            var pause = Image("PauseMenu", canvas.transform, null, new Color(0.025f, 0.035f, 0.045f, 0.86f));
            Stretch(pause.rectTransform);
            pause.raycastTarget = true;
            var panel = Rect("Panel", pause.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(610, 500));
            var panelImage = panel.gameObject.AddComponent<UnityEngine.UI.Image>();
            panelImage.sprite = Sprite("Sprites/Components/Popup/Popup_Frame01_White1.png");
            panelImage.type = UnityEngine.UI.Image.Type.Sliced;
            panelImage.color = new Color32(30, 36, 40, 250);
            panelImage.raycastTarget = false;
            var accent = Image("Accent", panel, null, Amber);
            Place(accent.rectTransform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(48, -42), new Vector2(56, 4));
            Label(panel, "Title", "Menu.Paused", "PAUSED", new Vector2(48, -64), new Vector2(514, 70), 50, Color.white);
            var resume = Button(panel, "ContinueButton", "Menu.Continue", "CONTINUE", new Vector2(48, -166), new Vector2(514, 84), true);
            var leave = Button(panel, "ExitToMenuButton", "Menu.Return", "RETURN TO MENU", new Vector2(48, -268), new Vector2(514, 84), false);
            UnityEventTools.AddPersistentListener(resume.onClick, manager.CloseMenu);
            WireScene(leave, menuIndex);
            Label(panel, "Hint", "Menu.ResumeHint", "ESC  /  RESUME", new Vector2(48, -404), new Vector2(514, 36), 20, new Color32(160, 169, 173, 255));
            var managerData = new SerializedObject(manager);
            managerData.FindProperty("pauseMenu").objectReferenceValue = pause.gameObject;
            managerData.FindProperty("continueButton").objectReferenceValue = resume;
            managerData.ApplyModifiedPropertiesWithoutUndo();
            pause.gameObject.SetActive(false);
            EditorSceneManager.MarkSceneDirty(gameplay);
            EditorSceneManager.SaveScene(gameplay);

            var menu = EditorSceneManager.OpenScene(MenuScene);
            if (GameObject.Find("MenuCanvas") != null)
                throw new InvalidOperationException("Main menu already exists; edit it in place.");
            if (UnityEngine.Object.FindAnyObjectByType<Camera>() == null)
            {
                var camera = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
                camera.tag = "MainCamera";
                camera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
                camera.GetComponent<Camera>().backgroundColor = Ink;
            }
            if (UnityEngine.Object.FindAnyObjectByType<Light>() == null)
            {
                var light = new GameObject("Directional Light", typeof(Light));
                light.GetComponent<Light>().type = LightType.Directional;
                light.transform.rotation = Quaternion.Euler(50, -30, 0);
            }
            if (UnityEngine.Object.FindAnyObjectByType<ContainerScope>() == null)
                new GameObject("SceneScope", typeof(ContainerScope));
            var controller = new GameObject("MainMenuController", typeof(MainMenuController)).GetComponent<MainMenuController>();
            var root = new GameObject("MenuCanvas", typeof(RectTransform), typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
            root.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = root.GetComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            var background = Rect("Background", root.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            Stretch(background);
            var raw = background.gameObject.AddComponent<UnityEngine.UI.RawImage>();
            raw.texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Game/UI/Art/MenuBackground.png");
            raw.raycastTarget = false;
            var fit = background.gameObject.AddComponent<UnityEngine.UI.AspectRatioFitter>();
            fit.aspectMode = UnityEngine.UI.AspectRatioFitter.AspectMode.EnvelopeParent;
            fit.aspectRatio = (float)raw.texture.width / raw.texture.height;

            var content = Rect("MenuContent", root.transform, new Vector2(0.085f, 0.5f), new Vector2(0, 0.5f), Vector2.zero, new Vector2(600, 422));
            Label(content, "Section", "Menu.Main", "MAIN MENU", Vector2.zero, new Vector2(560, 35), 21, Amber);
            Label(content, "GameTitle", "Menu.Title", "ACHTUNG", new Vector2(-5, -50), new Vector2(605, 110), 94, Color.white, false);
            var line = Image("Accent", content, null, Amber);
            Place(line.rectTransform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, -186), new Vector2(65, 4));
            var play = Button(content, "PlayButton", "Menu.Play", "PLAY", new Vector2(0, -231), new Vector2(490, 84), true);
            var quit = Button(content, "QuitButton", "Menu.Quit", "QUIT", new Vector2(0, -333), new Vector2(490, 76), false);
            WireScene(play, gameplayIndex);
            UnityEventTools.AddPersistentListener(quit.onClick, controller.QuitGame);
            var languages = Rect("Languages", root.transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(64, -48), new Vector2(208, 82));
            Flag(languages, "English", "en", "Eng", 0);
            Flag(languages, "Japanese", "ja", "Jpn", 112);

            var eventSystem = UnityEngine.Object.FindAnyObjectByType<EventSystem>();
            if (eventSystem == null)
                eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule)).GetComponent<EventSystem>();
            eventSystem.firstSelectedGameObject = play.gameObject;
            Canvas.ForceUpdateCanvases();
            EditorSceneManager.MarkSceneDirty(menu);
            EditorSceneManager.SaveScene(menu);
            AssetDatabase.SaveAssets();
            Debug.Log("Created MenuScene and gameplay pause menu. Gameplay ID: " + gameplayIndex + ", menu ID: " + menuIndex);
        }

        private static void SetupLocalization()
        {
            var collection = LocalizationEditorSettings.GetStringTableCollection("StringTable");
            var entries = new[]
            {
                new[] { "Menu.Title", "ACHTUNG", "ACHTUNG" },
                new[] { "Menu.Main", "MAIN MENU", "メインメニュー" },
                new[] { "Menu.Play", "PLAY", "プレイ" },
                new[] { "Menu.Quit", "QUIT", "ゲーム終了" },
                new[] { "Menu.Paused", "PAUSED", "一時停止" },
                new[] { "Menu.Continue", "CONTINUE", "続ける" },
                new[] { "Menu.Return", "RETURN TO MENU", "メニューに戻る" },
                new[] { "Menu.ResumeHint", "ESC  /  RESUME", "ESC  /  再開" }
            };
            foreach (var table in collection.StringTables)
            {
                var column = table.LocaleIdentifier.Code == "ja" ? 2 : 1;
                if (table.LocaleIdentifier.Code != "en" && table.LocaleIdentifier.Code != "ja")
                    continue;
                foreach (var row in entries)
                    table.AddEntry(row[0], row[column]);
                EditorUtility.SetDirty(table);
            }
            EditorUtility.SetDirty(collection);
            EditorUtility.SetDirty(collection.SharedData);
            LocalizationEditorSettings.EditorEvents.RaiseCollectionModified(null, collection);

            var japanesePath = FontFolder + "MenuJapanese.asset";
            var japanese = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(japanesePath);
            if (japanese == null)
            {
                var source = AssetDatabase.LoadAssetAtPath<Font>(FontFolder + "NotoSansCJKjp-Regular.otf");
                japanese = TMP_FontAsset.CreateFontAsset(source, 64, 7, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic, true);
                japanese.name = "MenuJapanese";
                japanese.isMultiAtlasTexturesEnabled = true;
                AssetDatabase.CreateAsset(japanese, japanesePath);
                var characters = string.Concat(entries.Select(e => e[2])) + "ABCDEFGHIJKLMNOPQRSTUVWXYZ /";
                if (!japanese.TryAddCharacters(characters, out var missing))
                    throw new InvalidOperationException("Missing Japanese glyphs: " + missing);
                foreach (var atlas in japanese.atlasTextures)
                {
                    AssetDatabase.AddObjectToAsset(atlas, japanese);
                    EditorUtility.SetDirty(atlas);
                }
                AssetDatabase.AddObjectToAsset(japanese.material, japanese);
                japanese.material.mainTexture = japanese.atlasTexture;
                EditorUtility.SetDirty(japanese.material);
                EditorUtility.SetDirty(japanese);
            }
            var latinPath = FontFolder + "MenuLatin.asset";
            if (AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(latinPath) == null)
                AssetDatabase.CopyAsset(LayerLab + "Fonts/Oxanium-Bold SDF.asset", latinPath);
            _latin = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(latinPath);
            _latin.fallbackFontAssetTable = new System.Collections.Generic.List<TMP_FontAsset> { japanese };
            EditorUtility.SetDirty(_latin);

            var assets = LocalizationEditorSettings.GetAssetTableCollection("MenuAssets") ??
                         LocalizationEditorSettings.CreateAssetTableCollection("MenuAssets", "Assets/Game/Localization");
            assets.AddAssetToTable(new LocaleIdentifier("en"), "Menu.Font", _latin);
            assets.AddAssetToTable(new LocaleIdentifier("ja"), "Menu.Font", japanese);
            foreach (var table in assets.AssetTables)
                EditorUtility.SetDirty(table);
            EditorUtility.SetDirty(assets);
            EditorUtility.SetDirty(assets.SharedData);
            LocalizationEditorSettings.EditorEvents.RaiseCollectionModified(null, assets);
            var settings = LocalizationEditorSettings.ActiveLocalizationSettings;
            if (!settings.GetStartupLocaleSelectors().OfType<PlayerPrefLocaleSelector>().Any())
                settings.GetStartupLocaleSelectors().Insert(0, new PlayerPrefLocaleSelector());
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        private static void Flag(Transform parent, string name, string code, string suffix, float x)
        {
            var image = Image(name + "Flag", parent, Sprite("Sprites/Demo/Demo_LanguageFlag/Icon_LanguageFlag_Small01_" + suffix + ".png"), Color.white);
            Place(image.rectTransform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(x, 0), new Vector2(88, 66));
            image.preserveAspect = true;
            image.raycastTarget = true;
            var button = image.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.targetGraphic = image;
            var marker = Image("Selected", image.transform, null, Amber);
            Place(marker.rectTransform, new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, -8), new Vector2(88, 3));
            var locale = image.gameObject.AddComponent<LocaleButton>();
            var data = new SerializedObject(locale);
            data.FindProperty("localeCode").stringValue = code;
            data.FindProperty("selectedIndicator").objectReferenceValue = marker.gameObject;
            data.ApplyModifiedPropertiesWithoutUndo();
            marker.gameObject.SetActive(code == "en");
            UnityEventTools.AddPersistentListener(button.onClick, locale.SelectLocale);
        }

        private static UnityEngine.UI.Button Button(Transform parent, string name, string key, string initial, Vector2 position, Vector2 size, bool primary)
        {
            var image = Image(name, parent, Sprite("Sprites/Components/Button_Custom/Btn_TextButton_Square01_White1.png"), Color.white);
            Place(image.rectTransform, new Vector2(0, 1), new Vector2(0, 1), position, size);
            image.type = UnityEngine.UI.Image.Type.Sliced;
            image.raycastTarget = true;
            var button = image.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.normalColor = primary ? Amber : new Color32(40, 48, 53, 240);
            colors.highlightedColor = primary ? new Color32(255, 211, 142, 255) : new Color32(69, 81, 87, 255);
            colors.selectedColor = colors.highlightedColor;
            colors.pressedColor = primary ? new Color32(199, 139, 53, 255) : new Color32(24, 30, 34, 255);
            colors.fadeDuration = 0.12f;
            button.colors = colors;
            var label = Label(image.transform, "Label", key, initial, new Vector2(32, 0), new Vector2(size.x - 64, size.y), 30, primary ? Ink : Color.white);
            label.alignment = TextAlignmentOptions.MidlineLeft;
            return button;
        }

        private static void WireScene(UnityEngine.UI.Button button, int index)
        {
            var loader = button.gameObject.AddComponent<SceneLoader>();
            var data = new SerializedObject(loader);
            data.FindProperty("sceneId").intValue = index;
            data.ApplyModifiedPropertiesWithoutUndo();
            UnityEventTools.AddPersistentListener(button.onClick, loader.LoadScene);
        }

        private static TMP_Text Label(Transform parent, string name, string key, string initial, Vector2 position, Vector2 size, float fontSize, Color color, bool localizedFont = true)
        {
            var rect = Rect(name, parent, new Vector2(0, 1), new Vector2(0, 1), position, size);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.font = _latin;
            text.text = initial;
            text.fontSize = fontSize;
            text.enableAutoSizing = true;
            text.fontSizeMin = fontSize * 0.75f;
            text.fontSizeMax = fontSize;
            text.color = color;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.alignment = TextAlignmentOptions.MidlineLeft;
            text.raycastTarget = false;
            var localizer = rect.gameObject.AddComponent<LocalizeStringEvent>();
            localizer.StringReference = new LocalizedString("StringTable", key);
            var setter = (UnityAction<string>)Delegate.CreateDelegate(typeof(UnityAction<string>), text, "set_text");
            UnityEventTools.AddPersistentListener(localizer.OnUpdateString, setter);
            if (localizedFont)
                rect.gameObject.AddComponent<LocalizedMenuFont>();
            return text;
        }

        private static Sprite Sprite(string relativePath) => AssetDatabase.LoadAssetAtPath<Sprite>(LayerLab + relativePath);

        private static UnityEngine.UI.Image Image(string name, Transform parent, Sprite sprite, Color color)
        {
            var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            var image = rect.gameObject.AddComponent<UnityEngine.UI.Image>();
            image.sprite = sprite;
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private static RectTransform Rect(string name, Transform parent, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
        {
            var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            Place(rect, anchor, pivot, position, size);
            return rect;
        }

        private static void Place(RectTransform rect, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }
}
