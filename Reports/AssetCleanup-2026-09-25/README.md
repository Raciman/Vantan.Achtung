# Очистка ассетов Achtung — 25 сентября 2026

Удалены **14,467 неиспользуемых ассетов**, их `.meta` и 166 пустых папок.
Освобождено **1.287 ГиБ**. Размер `Assets` с метаданными уменьшился с **1553.95 МиБ** до **235.61 МиБ** (84.84%).
В проекте осталось 555 ассетов. Размеры относятся к `Assets`, без Git, Library, сборок и отчётов.

## Что сохранено

- Вся папка `Assets/Game`: 346 файлов, включая метаданные, побайтово совпадают с состоянием до очистки.
- Все 182 файлов кода, библиотек, шейдерного кода и редакторского UI.
- Все четыре сцены из Build Settings: BootScene, MenuScene, GamePlayScene, TestScene.
- Прямые и транзитивные зависимости сохранённых файлов, настройки проекта, Addressables, локализация, Resources, предзагружаемые ассеты и редакторские ресурсы.
- Ассеты, которые редакторский код MenuSceneSetup загружает по строковым путям: исходный шрифт Oxanium, кнопка, панель, английский и японский флаги.
- Лицензии и данные, обнаруживаемые редакторскими инструментами динамически.

Очистка затронула `Assets/Thirdparty` и пять неиспользуемых исходных/веб-вариантов шрифта в `Assets/BoldPixels`. Используемые BoldPixels SDF и TTF сохранены. Остальные каталоги сохранены.
Удалены три неиспользуемые демосцены поставщиков; их зависимости сохранены там, где они нужны игре.

## Проверка зависимостей

Перед удалением экспортированы прямые зависимости всех 15022 ассетов через `AssetDatabase.GetDependencies` в Unity 6000.5.9f1. Граф дополнен GUID-ссылками из сериализованных файлов, `.meta`, ProjectSettings, Addressables и ссылками из кода. Удалены только файлы вне транзитивного замыкания сохраняемых корней. Перед удалением сверены полные пути и SHA-256 каждого файла.

После обновления Unity:

- Все 555 наборов зависимостей сохранённых ассетов совпали с исходными.
- Все 1306 оставшихся файлов `Assets` совпали с исходными по SHA-256.
- Ссылок на GUID удалённых файлов не найдено.
- Все четыре игровые сцены повторно открыты и проверены: новых битых ссылок и отсутствующих скриптов нет.
- Проверены 52 сохранённых префаба (180 объектов): битых ссылок и отсутствующих скриптов нет.
- Существующий `MenuFlowSmokeTest` прошёл в Play Mode: английская/японская локализация, глифы, запуск игры, Escape/пауза, продолжение, блокировка стрельбы в меню, возврат в меню и повторный вход.
- После проверки в консоли Unity нет ошибок и предупреждений. Редактор возвращён в Edit Mode с исходной чистой MenuScene; настройка выбранного языка восстановлена.
- Git: только ожидаемые удаления и новый каталог отчёта. Файлы кода, сцен, настроек и пакетов не изменены. Коммит не создавался.

Отдельная сборка player не запускалась.

## Удалённое по пакетам

| Каталог | Ассетов | МиБ с `.meta` |
|---|---:|---:|
| `Assets/BoldPixels/Assets` | 5 | 0.52 |
| `Assets/Thirdparty/Animations` | 288 | 297.58 |
| `Assets/Thirdparty/Ciathyza` | 1 | 0.22 |
| `Assets/Thirdparty/Hovl Studio` | 16 | 5.89 |
| `Assets/Thirdparty/Layer Lab` | 3,672 | 131.49 |
| `Assets/Thirdparty/Models/PolygonBattleRoyale` | 1,166 | 40.52 |
| `Assets/Thirdparty/Models/PolygonMilitary` | 4,199 | 202.43 |
| `Assets/Thirdparty/Models/PolygonZombies` | 84 | 13.68 |
| `Assets/Thirdparty/Models/Synty` | 4,947 | 548.63 |
| `Assets/Thirdparty/Rifle 8-Way Locomotion Pack` | 48 | 19.23 |
| `Assets/Thirdparty/Universal Animation Library 2[Standard]` | 1 | 23.63 |
| `Assets/Thirdparty/Universal Animation Library[Standard]` | 40 | 34.51 |

## Проблемы, существовавшие до очистки

В GamePlayScene уже было 4 битые ссылки на draw/holster анимации. В TestScene было 11 битых ссылок: такие же 4 ссылки на анимации и 7 ссылок на аватар/меши/материалы Ch48. До и после очистки список полностью совпадает. Они не исправлялись в рамках удаления ассетов.

- `Assets/Scenes/GamePlayScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[0].drawClip`
- `Assets/Scenes/GamePlayScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[0].holsterClip`
- `Assets/Scenes/GamePlayScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[1].drawClip`
- `Assets/Scenes/GamePlayScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[1].holsterClip`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48 / SkinnedMeshRenderer / m_Materials.Array.data[0]`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48 / SkinnedMeshRenderer / m_Materials.Array.data[1]`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48 / SkinnedMeshRenderer / m_Materials.Array.data[2]`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48 / SkinnedMeshRenderer / m_Mesh`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48_hair1 / SkinnedMeshRenderer / m_Materials.Array.data[0]`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48_hair1 / SkinnedMeshRenderer / m_Mesh`
- `Assets/Scenes/TestScene.unity / MissingReference / Ch48_nonPBR / Animator / m_Avatar`
- `Assets/Scenes/TestScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[0].drawClip`
- `Assets/Scenes/TestScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[0].holsterClip`
- `Assets/Scenes/TestScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[1].drawClip`
- `Assets/Scenes/TestScene.unity / MissingReference / Player / PlayerAnimatorController / weaponSets.Array.data[1].holsterClip`

## Файлы отчёта

- `deletion-manifest.csv` — точный список удалённых файлов, размеры и исходные SHA-256.
- `removed-folder-metadata.csv` — удалённые `.meta` пустых папок.
- `retention-reasons.json` — корни сохранения, найденные строковые пути и цепочки зависимостей.
- `protected-file-hashes.json` — исходные SHA-256 сохраняемых файлов (за исключением явно удалённых `.meta` пустых папок).
- `verification.json` — итоговая проверка и список существовавших ранее проблем.
- `runtime-smoke-test.txt` — результат теста Play Mode.
- `git-verification.json` — проверка области изменений Git.
- `dependency-evidence.zip` — полные исходные графы GUID/зависимостей и сравнение до/после.
