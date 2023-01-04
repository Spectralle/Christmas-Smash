using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CasualGame
{
    public static class SaveSystem
    {
        public static event EventHandler OnSaved;
        public static event EventHandler OnLoaded;
        
        private static string FilePath => Application.persistentDataPath + "/" + "Save.dat";
        public static string GetFilePath => FilePath;

        private static SaveData _saveData = new(LevelSaveData);
        public static LevelsSaveData LevelSaveData = new();
        
        
        public static void Save(object sender)
        {
            ScoreSystem.Instance.SetScoreValueIfBLarger(sender, ValSysInit.StrID_HighestLevelScore, _saveData.HighestScore);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_HighestLevelScore, out float highestScore);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_Currency, out float gold);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelsCleared, out float levelsCleared);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_LevelsPlayed, out float levelsPlayed);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_Opt_MusicVol, out float musicVolume);
            ScoreSystem.Instance.GetScoreValue(ValSysInit.StrID_Opt_UIVol, out float uiVolume);
            _saveData = new SaveData(
                (int)highestScore,
                (int)gold,
                (int)levelsCleared,
                (int)levelsPlayed,
                musicVolume,
                uiVolume,
                LevelSaveData
            );

            FileStream file = new(FilePath, FileMode.OpenOrCreate);
            try
            {
                BinaryFormatter formatter = new();
                formatter.Serialize(file, _saveData);
            }
            catch (SerializationException e) { Debug.LogError($"[Save System] Save error: {e}"); }
            finally { file.Close(); }
            
            OnSaved?.Invoke(sender, EventArgs.Empty);
        }

        public static void Load(object sender)
        {
            if (!File.Exists(FilePath))
            {
                Debug.Log("[Save System] No existing data to load.");
                return;
            }

            FileStream file = new(FilePath, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new();
                _saveData = (SaveData)formatter.Deserialize(file);
                
                ScoreSystem.Instance.SetScoreValue(sender, ValSysInit.StrID_HighestLevelScore, _saveData.HighestScore);
                ScoreSystem.Instance.SetScoreValue(sender, ValSysInit.StrID_Currency, _saveData.Gold);
                ScoreSystem.Instance.SetScoreValue(sender, ValSysInit.StrID_LevelsCleared, _saveData.LevelsCleared);
                ScoreSystem.Instance.SetScoreValue(sender, ValSysInit.StrID_LevelsPlayed, _saveData.LevelsPlayed);
                
                ScoreSystem.Instance.SetScoreValue(sender, ValSysInit.StrID_Opt_MusicVol, _saveData.MusicVolume);
                ScoreSystem.Instance.SetScoreValue(sender, ValSysInit.StrID_Opt_UIVol, _saveData.UiVolume);
            }
            catch (SerializationException e) { Debug.LogError($"[Save System] Load error: {e}"); }
            finally { file.Close(); }
            
            OnLoaded?.Invoke(sender, EventArgs.Empty);
        }

        public static void ResetSavedData()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            
            ValSysInit.ReInitializeScoreSystem();
        }
    }
}