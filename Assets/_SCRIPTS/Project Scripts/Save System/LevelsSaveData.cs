using System;
using System.Collections.Generic;
using UnityEngine;

namespace CasualGame
{
    [Serializable]
    public class LevelsSaveData
    {
        public static LevelsSaveData Instance;
        public LevelsSaveData() => Instance ??= this;

        public class LevelProgressionData
        {
            public int BestTime { get; }
            public int BestShots { get; }

            public LevelProgressionData(int cTime, int cShots)
            {
                if (BestTime == 0 || cTime < BestTime)
                    BestTime = cTime;
                if (BestShots == 0 || cShots < BestShots)
                    BestShots = cShots;
            }
        }

        private static List<LevelProgressionData> _levelProgressionList = new()
        {
            new(0,0), new(0,0),
            new(0,0), new(0,0),
            new(0,0), new(0,0),
            new(0,0)
        };
        public static List<LevelProgressionData> LPL => _levelProgressionList;

        
        public static void SetLevelData(int levelID, int completionTime, int completionShots)
        {
            if (levelID <= _levelProgressionList.Count)
                _levelProgressionList[levelID - 1] = new LevelProgressionData(completionTime, completionShots);
            else
                Debug.LogError($"Invalid level ID when setting level completion data: {levelID} {_levelProgressionList.Count}");
        }

        public static (int bestCompletionTime, int bestCompletionShots) GetLevelData(int levelID) =>
            (_levelProgressionList[levelID].BestTime, _levelProgressionList[levelID].BestShots);
    }
}