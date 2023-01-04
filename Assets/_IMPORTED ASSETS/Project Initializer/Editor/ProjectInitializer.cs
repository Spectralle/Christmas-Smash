using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities.ProjectInitializer
{
    public static class ProjectInitializer
    {
        private const string EDITORTOOLSPATH = "TIS Tools/Project Initializer/";
        
        #region Set up Project folders
        private const string SCENES         =       "_SCENES";
        private const string TEST_SCENES    =           "TESTING";
        private const string GAME_SCENES    =           "GAME";
        private const string GRAPHICS       =       "_GRAPHICS";
        private const string MATERIALS      =           "MATERIALS";
        private const string TEXTURES       =           "TEXTURES";
        private const string SHADERS        =           "SHADERS";
        private const string ANIMATIONS     =       "_ANIMATIONS";
        private const string CHAR_ANIMS     =           "CHARACTER";
        private const string OTHER_ANIMS    =           "OTHER";
        private const string AUDIO          =       "_AUDIO";
        private const string MUSIC          =           "MUSIC";
        private const string SFX            =           "SFX";
        private const string MIXERS         =           "MIXERS";
        private const string PREFABS        =       "_PREFABS";
        private const string SCRIPTS        =       "_SCRIPTS";
        private const string IMPORTS        =       "_IMPORTED ASSETS";
        

        [MenuItem(EDITORTOOLSPATH + "Create All Project Folders", false, 0)]
        private static void CreateAllFolders()
        {
            CreateScenesFolder();
            CreateGraphicsFolder();
            CreateAnimationsFolder();
            CreateAudioFolder();
            CreatePrefabsFolder();
            CreateScriptsFolder();
            CreateImportedAssetsFolder();
        }

        /// <summary>
        /// Creates a new folder in the Unity project Assets folder
        /// </summary>
        /// <param name="NAME">The name of the new folder</param>
        /// <param name="PARENT">The file path of the new folder (No "/" at the end, e.g. "Assets/Test")</param>
        private static void CreateFolder(string NAME, string PARENT = "Assets")
        {
            if (!AssetDatabase.IsValidFolder($"{PARENT}/{NAME}"))
                AssetDatabase.CreateFolder(PARENT, NAME);
        }

        #region Individual Folder Creation
        
        [MenuItem(EDITORTOOLSPATH + "Create Scenes Folder", false, 11)]
        private static void CreateScenesFolder()
        {
            CreateFolder(SCENES);
            CreateFolder(GAME_SCENES, "Assets/" + SCENES);
            CreateFolder(TEST_SCENES, "Assets/" + SCENES);
        }

        [MenuItem(EDITORTOOLSPATH + "Create Graphics Folder", false, 12)]
        private static void CreateGraphicsFolder()
        {
            CreateFolder(GRAPHICS);
            CreateFolder(MATERIALS, "Assets/" + GRAPHICS);
            CreateFolder(TEXTURES, "Assets/" + GRAPHICS);
            CreateFolder(SHADERS, "Assets/" + GRAPHICS);
        }

        [MenuItem(EDITORTOOLSPATH + "Create Animations Folder", false, 13)]
        private static void CreateAnimationsFolder()
        {
            CreateFolder(ANIMATIONS);
            CreateFolder(CHAR_ANIMS, "Assets/" + ANIMATIONS);
            CreateFolder(OTHER_ANIMS, "Assets/" + ANIMATIONS);
        }
        
        [MenuItem(EDITORTOOLSPATH + "Create Audio Folder", false, 14)]
        private static void CreateAudioFolder()
        {
            CreateFolder(AUDIO);
            CreateFolder(MUSIC, "Assets/" + AUDIO);
            CreateFolder(SFX, "Assets/" + AUDIO);
            CreateFolder(MIXERS, "Assets/" + AUDIO);
        }
        
        [MenuItem(EDITORTOOLSPATH + "Create Prefabs Folder", false, 15)]
        private static void CreatePrefabsFolder() => CreateFolder(PREFABS);

        [MenuItem(EDITORTOOLSPATH + "Create Scripts Folder", false, 16)]
        private static void CreateScriptsFolder() => CreateFolder(SCRIPTS);

        [MenuItem(EDITORTOOLSPATH + "Create Imports Folder", false, 17)]
        private static void CreateImportedAssetsFolder() => CreateFolder(IMPORTS);

        #endregion
        #endregion
        
        #region Set up Scene Objects
        private const string DIVIDER                    =       "============================";
        
        private const string GAME_MANAGER               =       "GAME MANAGERS";
        private const string AUDIO_MANAGER              =           "AUDIO MANAGER";
        private const string MUSIC_AUDIO_SOURCE         =               "Audio Source: Music";
        private const string VOICEOVER_AUDIO_SOURCE     =               "Audio Source: Voice";
        private const string SFX_AUDIO_SOURCE           =               "Audio Source: SFX";
        private const string UI_AUDIO_SOURCE            =               "Audio Source: UI";
        
        private const string SCENE_MANAGER              =       "SCENE MANAGERS";
        
        private const string SCENE_OBJECTS              =       "SCENE OBJECTS";
        private const string CAMERAS                    =           "Cameras";
        private const string ENVIRONMENT                =           "Environment";
        private const string NPC_CHARACTERS             =           "NPCs";
        private const string PLAYER                     =           "Player";
        
        private const string USER_INTERFACE             =       "USER INTERFACE";
        private const string MAIN_CANVAS                =           "Main Canvas";
        
        
        [MenuItem(EDITORTOOLSPATH + "Set up Scene Objects", false, 30)]
        private static void SetupSceneObjects()
        {
            CreateGameObject(DIVIDER, true);
            
            CreateGameObject(GAME_MANAGER).AddComponent<PersistentGameSystemsObject>();
                var AS = CreateGameObject(AUDIO_MANAGER, GAME_MANAGER).AddComponent<AudioSystem>();
                    AS.SetMusicSource(CreateGameObject(MUSIC_AUDIO_SOURCE, AUDIO_MANAGER).AddComponent<AudioSource>());
                    AS.SetVoiceSource(CreateGameObject(VOICEOVER_AUDIO_SOURCE, AUDIO_MANAGER).AddComponent<AudioSource>());
                    AS.SetSfxSource(CreateGameObject(SFX_AUDIO_SOURCE, AUDIO_MANAGER).AddComponent<AudioSource>());
                    AS.SetUISource(CreateGameObject(UI_AUDIO_SOURCE, AUDIO_MANAGER).AddComponent<AudioSource>());
                
            CreateGameObject(DIVIDER, true);

            CreateGameObject(SCENE_MANAGER);
            
            CreateGameObject(DIVIDER, true);
            
            CreateGameObject(SCENE_OBJECTS);
                var CAMS = CreateGameObject(CAMERAS, SCENE_OBJECTS);
                if (Camera.main)
                    Camera.main.transform.parent = CAMS.transform;
                CreateGameObject(ENVIRONMENT, SCENE_OBJECTS);
                CreateGameObject(NPC_CHARACTERS, SCENE_OBJECTS);
                CreateGameObject(PLAYER, SCENE_OBJECTS);
            
            CreateGameObject(DIVIDER, true);
            
            CreateGameObject(USER_INTERFACE);
                var MC = CreateGameObject(MAIN_CANVAS, USER_INTERFACE);
                if (MC.TryGetComponent(out Canvas canvas))
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                else
                    MC.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                var ES = CreateGameObject("EventSystem", USER_INTERFACE);
                if (!ES.GetComponent<EventSystem>())
                    ES.AddComponent<EventSystem>();
                if (!ES.GetComponent<StandaloneInputModule>())
                    ES.AddComponent<StandaloneInputModule>();
            
            CreateGameObject(DIVIDER, true);
        }

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="POS">The Vector3 position for the new GameObject (Relative to any parent).</param>
        /// <param name="ROT">The Quaternion rotation for the new GameObject (Relative to any parent).</param>
        /// <param name="PARENT">The parent Transform for the new GameObject. Blank if none.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, Vector3 POS, Quaternion ROT, Transform PARENT = null, bool AllowDuplicates = false)
        {
            GameObject GO = GameObject.Find(NAME);
            if (!AllowDuplicates && GO)
            {
                if (GO.TryGetComponent(out RectTransform RT))
                    RT.SetParent(PARENT);
                else
                    GO.transform.SetParent(PARENT);
                GO.transform.position = POS;
                GO.transform.rotation = ROT;
            }
            else
            {
                GO = new GameObject(NAME)
                {
                    transform =
                    {
                        parent = PARENT,
                        position = POS,
                        rotation = ROT
                    }
                };
            }

            return GO;
        }

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="POS">The Vector3 position for the new GameObject (Relative to any parent).</param>
        /// <param name="ROT">The Quaternion rotation for the new GameObject (Relative to any parent).</param>
        /// <param name="PARENTNAME">The NAME of the parent Transform for the new GameObject. Blank if none.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, Vector3 POS, Quaternion ROT, string PARENTNAME = "", bool AllowDuplicates = false) =>
            CreateGameObject(NAME, POS, ROT, GameObject.Find(PARENTNAME)?.transform, AllowDuplicates);

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="POS">The Vector3 position for the new GameObject (Relative to any parent).</param>
        /// <param name="PARENT">The parent Transform for the new GameObject. Blank if none.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, Vector3 POS, Transform PARENT, bool AllowDuplicates = false) =>
            CreateGameObject(NAME, POS, Quaternion.identity, PARENT, AllowDuplicates);

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="POS">The Vector3 position for the new GameObject (Relative to any parent).</param>
        /// <param name="PARENTNAME">The NAME of the parent Transform for the new GameObject. Blank if none.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, Vector3 POS, string PARENTNAME, bool AllowDuplicates = false) =>
            CreateGameObject(NAME, POS, Quaternion.identity, PARENTNAME, AllowDuplicates);

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="PARENT">The NAME of the parent Transform for the new GameObject. Blank if none.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, Transform PARENT, bool AllowDuplicates = false) =>
            CreateGameObject(NAME, Vector3.zero, Quaternion.identity, PARENT, AllowDuplicates);

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="PARENTNAME">The NAME of the parent Transform for the new GameObject. Blank if none.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, string PARENTNAME, bool AllowDuplicates = false) =>
            CreateGameObject(NAME, Vector3.zero, Quaternion.identity, PARENTNAME, AllowDuplicates);

        /// <summary>
        /// Creates a new GameObject in the current Unity scene.
        /// </summary>
        /// <param name="NAME">The name of the new GameObject.</param>
        /// <param name="AllowDuplicates">Whether multiple GameObjects with the same name can be created.</param>
        /// <returns>Returns the GameObject.</returns>
        private static GameObject CreateGameObject(string NAME, bool AllowDuplicates = false) =>
            CreateGameObject(NAME, Vector3.zero, Quaternion.identity, string.Empty, AllowDuplicates);
        #endregion
    }
}