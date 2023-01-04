using System;

namespace CasualGame
{
    [Serializable]
    public class SaveData
    {
        public int HighestScore;
        public int Gold;
        public int LevelsCleared;
        public int LevelsPlayed;
        
        public float MusicVolume;
        public float UiVolume;

        public LevelsSaveData LevelSaveData;
        

        public SaveData(LevelsSaveData LSD)
        {
            HighestScore = 0;
            Gold = 0;
            LevelsCleared = 0;
            LevelsPlayed = 0;
            
            MusicVolume = 1;
            UiVolume = 1;

            LevelSaveData = LSD;
        }
        
        public SaveData(
            int highestScore,
            int gold,
            int levelsCleared,
            int levelsPlayed,
            float musicVolume,
            float uiVolume,
            LevelsSaveData LSD
        ) {
            HighestScore = highestScore;
            Gold = gold;
            LevelsCleared = levelsCleared;
            LevelsPlayed = levelsPlayed;
            
            MusicVolume = musicVolume;
            UiVolume = uiVolume;

            LevelSaveData = LSD;
        }
    }
}