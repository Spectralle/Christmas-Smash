// ReSharper disable InconsistentNaming

using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace CasualGame
{
    public class ValSysInit : PersistentSingleton<ValSysInit>
    {
        public const string StrID_CurrentScore       =   "LEVEL SCORE";
        public const string StrID_HighestLevelScore  =   "HIGHEST LEVEL SCORE";
        public const string StrID_Currency           =   "COINS";
        public const string StrID_LevelsCleared      =   "LEVELS CLEARED";
        public const string StrID_LevelsPlayed       =   "LEVELS PLAYED";
        
        public const string StrID_LevelToLoad        =   "LEVEL TO LOAD";
        public const string StrID_ShotsTaken         =   "SHOT COUNT";
        public const string StrID_ScoreGoal          =   "SCORE GOAL";
        
        public const string StrID_Opt_MusicVol       =   "MUSIC VOLUME";
        public const string StrID_Opt_UIVol          =   "UI VOLUME";

        [Space]
        [SerializeField, Required] private AudioMixer _audioMixer;
        [SerializeField, Required] private AudioClip _musicClip;

        private static AudioMixer _AM;


        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
                return;
            
            _AM = _audioMixer;
            InitializeValueSystem();
        }

        private void Start()
        {
            if (!_musicClip)
                Debug.LogWarning("[Audio System] No music playing: Unspecified clip.");
            else
                AudioSystem.Instance.PlayMusic(_musicClip, true);
            
            ScoreSystem.Instance.GetScoreValue(StrID_Opt_MusicVol, out float savedMusicVolume);
            _AM.SetFloat("MusicVolume", Mathf.Log10(savedMusicVolume) * 20);
            ScoreSystem.Instance.GetScoreValue(StrID_Opt_UIVol, out float savedUIVolume);
            _AM.SetFloat("UIVolume", Mathf.Log10(savedUIVolume) * 20);
        }

        private static void InitializeValueSystem()
        {
            ScoreSystem scoreSystem = new (Instance, mainName: StrID_CurrentScore);
            scoreSystem.AddScoreType(Instance, StrID_HighestLevelScore);
            scoreSystem.AddScoreType(Instance, StrID_Currency);
            scoreSystem.AddScoreType(Instance, StrID_LevelsCleared);
            scoreSystem.AddScoreType(Instance, StrID_LevelsPlayed);
            
            scoreSystem.AddScoreType(Instance, StrID_ShotsTaken);
            scoreSystem.AddScoreType(Instance, StrID_LevelToLoad, 1);
            scoreSystem.AddScoreType(Instance, StrID_ScoreGoal, 1);

            scoreSystem.AddScoreType(Instance, StrID_Opt_MusicVol, 1);
            scoreSystem.AddScoreType(Instance, StrID_Opt_UIVol, 1);
            
            SaveSystem.Load(Instance);
        }
        
        public static void ResetScoreSystem()
        {
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_CurrentScore);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_ShotsTaken);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_ScoreGoal, 1);
        }

        public static void ReInitializeScoreSystem()
        {
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_CurrentScore);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_HighestLevelScore);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_Currency);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_LevelsCleared);
            
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_ShotsTaken);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_LevelToLoad, 1);
            ScoreSystem.Instance.SetScoreValue(Instance, StrID_ScoreGoal, 1);
        }
    }
}