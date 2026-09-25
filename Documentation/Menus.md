# Menus

The UI is authored in `Assets/Scenes/MenuScene.unity` and in the existing Canvas in `Assets/Scenes/GamePlayScene.unity`. It uses the Layer Lab GUI Pro — Survival Clean button, panel and language-flag sprites.

- BootScene opens MenuScene. Existing build IDs are preserved: BootScene = 0, GamePlayScene = 1, TestScene = 2, MenuScene = 3.
- PlayButton has `SceneLoader` targeting ID 1; ExitToMenuButton targets ID 3. Select a different enabled scene with the **Scene ID** field or **Scene** dropdown. Button On Click invokes `SceneLoader.LoadScene`.
- QuitButton calls `MainMenuController.QuitGame`: `Application.Quit` in a player, exits Play Mode in the Editor.
- UIManager receives the shared `IInputService` through Reflex. The `UI/Pause` action binds Escape. It remains enabled when the Player action map is disabled.
- Pause stops scaled gameplay time, unlocks the pointer, and selects Continue for keyboard navigation. Continue or a second Escape restores the previous time scale and pointer state.
- Disabling gameplay clears the player's buffered commands. On resume, fire remains suppressed until all bound fire buttons have been released, preventing held fire and menu clicks from becoming shots. Escape is not part of player intent.

## Localization

All newly added labels use `LocalizeStringEvent` and the existing `StringTable`, with `Menu.*` keys filled for both `en` and `ja`. The two icon-only buttons use Layer Lab's combined UK/US flag and Japanese flag. They set `LocalizationSettings.SelectedLocale`; the selected language is retained in `selected-locale` PlayerPrefs.

`MenuAssets` is an Asset Table mapping `Menu.Font` to a project copy of Layer Lab's Oxanium font for English and Noto Sans CJK JP for Japanese. The Japanese font has a dynamic multi-atlas and is also the English font's fallback while asynchronous localization loads. Original Layer Lab fonts are unchanged. Noto font source: https://github.com/notofonts/noto-cjk/blob/main/Sans/OTF/Japanese/NotoSansCJKjp-Regular.otf . Its SIL Open Font License is included in `Assets/Game/UI/Fonts/OFL.txt`.

Existing gameplay text outside `Menu.*` was not part of this change. The pre-existing Japanese StringTable had no translations before the menu entries were added.

## Background

`Assets/Game/UI/Art/MenuBackground.png` was generated with the built-in image-generation tool from a screenshot of GamePlayScene. The exact prompt is saved in `Documentation/MenuBackgroundPrompt.md`. The UI uses an envelope aspect fit so the background fills different screen ratios without stretching.

## Verification

Verified on Unity 6000.5.9f1, 2026-09-25: the complete runtime smoke test passed; all 8 new string keys have both translations; the Quit button stopped Play Mode; no runtime errors were reported. Addressables player content built successfully (33.6 seconds; empty BuildError in `Library/com.unity.addressables/buildlayout.json`). A standalone executable was not built or launched. Addressables emitted the profile configuration warning `ProfileValueReference: GetValue called with empty id` while completing successfully.

`Assets/Game/Tests/MenuFlowSmokeTest.cs` is an Editor-only Play Mode smoke test, excluded from player builds. Enter Play Mode, wait for MenuScene, then call `Ach.UI.Editor.MenuFlowSmokeTest.Run()` using the Unity MCP C# executor. It creates temporary virtual keyboard/mouse devices and clicks through the real EventSystem. The temporary runner and devices are removed automatically. Results are in the Console and `UnityEditor.SessionState.GetString("Achtung.MenuVerification", "")`.

It checks both language buttons, Japanese glyph coverage, Play, Escape opening/closing pause, buffered/held-fire suppression, recovery after releasing the trigger, Continue click safety, scene return and repeated gameplay entry. Screenshots are saved under the ignored `Temp/MenuWork` directory. The test restores the runtime background setting and finishes in the English main menu.

`MenuSceneSetup` is an authoring utility; it does not run in a built game and refuses to replace existing menus. Edit the saved scene objects in place.
