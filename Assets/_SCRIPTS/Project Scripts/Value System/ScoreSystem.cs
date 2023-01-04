using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CasualGame
{
    public class ScoreSystem : ValueStorageSystem<float>
    {
        public static ScoreSystem Instance;

        public ScoreSystem(object s, IReadOnlyDictionary<string, float> copyList = null, string mainName = "SCORE",
            float initialMainValue = default) : base(s, copyList, mainName, initialMainValue)
        {
            // This calls the ValueStorageSystem constructor.
            if (Instance != null)
                return;
            
            Instance = this;
        }

        /// <summary>
        /// Add a new score entry to the Dictionary, using the key and value.
        /// </summary>
        /// <param name="name">The string name given as the key to the new value.</param>
        /// <param name="value">The initial value which will be applied to the new entry.</param>
        /// <returns>Returns true is successful, false if unsuccessful.</returns>
        public bool AddScoreType(object s, string name, float value = 0) => AddValueType(s, name, value);

        /// <summary>
        /// Add a new score entry to the Dictionary, using the key and value.
        /// </summary>
        /// <param name="collection">The list to add onto the existing score list</param>
        public void AddScoreTypes(object s, Dictionary<string, float> collection) => AddValueTypes(s, collection);

        /// <summary>
        /// Try and get an existing value from the score list.
        /// </summary>
        /// <param name="key">The string name of the Dictionary entry to try and get.</param>
        /// <param name="value">The extracted value of the entry found.</param>
        /// <returns>Returns true is successful, false if unsuccessful.</returns>
        public bool GetScoreValue(string key, out float value) => GetValue(key, out value);

        /// <summary>
        /// Try and get the existing main value from the score list.
        /// </summary>
        /// <param name="getMainScoreValue"></param>
        /// <param name="value">The extracted value of the entry found.</param>
        /// <returns>Returns true is successful, false if unsuccessful.</returns>
        public bool GetMainScoreValue(out float value) => GetMainValue(out value);

        /// <summary>
        /// Set the value of an existing score entry in the list.
        /// </summary>
        /// <param name="key">The name of the score value to set.</param>
        /// <param name="value">The new value to set the entry to.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool SetScoreValue(object s, string key, float value = 0) => SetValue(s, key, value);
        
        /// <summary>
        /// Set the value of an existing score entry in the list.
        /// </summary>
        /// <param name="keyA">The name of the score value to set.</param>
        /// <param name="keyB">The new value to set the entry to.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool SetScoreValue(object s, string keyA, string keyB) => SetValue(s, keyA, keyB);
        
        /// <summary>
        /// Set the value of an existing score entry in the list to the same value as another entry,
        /// as long as the second entry is larger than the first.
        /// </summary>
        /// <param name="keyA">The name of the score value to set.</param>
        /// <param name="keyB">The new value to set the entry to.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool SetScoreValueIfBLarger(object s, string keyA, string keyB)
        {
            GetScoreValue(keyA, out float A);
            GetScoreValue(keyB, out float B);
            if (A > B)
            {
                if ((GridManager)s == GridManager.Instance)
                    Debug.Log($"Compared values {A} / {B}. B lower");
                return SetValue(s, keyA, B);
            }
            if ((GridManager)s == GridManager.Instance)
                Debug.Log($"Compared values {A} / {B}. A lower");
            return true;
        }
        
        /// <summary>
        /// Set the value of an existing score entry in the list to the same value as another entry,
        /// as long as the second entry is larger than the first, or the first is zero.
        /// </summary>
        /// <param name="keyA">The name of the score value to set.</param>
        /// <param name="keyB">The new value to set the entry to.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool SetScoreValueIfBLargerOrNotZero(object s, string keyA, string keyB)
        {
            GetScoreValue(keyA, out float A);
            GetScoreValue(keyB, out float B);
            if (A < B || A == 0)
                return SetValue(s, keyA, B);
            return true;
        }
        
        /// <summary>
        /// Set the value of an existing score entry in the list to a new value,
        /// as long as the new value is larger than the first.
        /// </summary>
        /// <param name="keyA">The name of the score value to set.</param>
        /// <param name="valueB">The new value to set the entry to, if it's larger.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool SetScoreValueIfBLarger(object s, string keyA, float valueB)
        {
            GetScoreValue(keyA, out float A);
            if (A < valueB)
                return SetValue(s, keyA, valueB);
            return true;
        }
        
        /// <summary>
        /// Set the value of an existing score entry in the list to a new value,
        /// as long as the new value is larger than the first, or the first is zero.
        /// </summary>
        /// <param name="keyA">The name of the score value to set.</param>
        /// <param name="valueB">The new value to set the entry to, if it's larger.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool SetScoreValueIfBLargerOrNotZero(object s, string keyA, float valueB)
        {
            GetScoreValue(keyA, out float A);
            if (A < valueB || A == 0)
                return SetValue(s, keyA, valueB);
            return true;
        }

        /// <summary>
        /// Modify the value of an existing score entry in the list.
        /// A positive (>0) value increases the score value by that much, while a negative (<0) decreases it.
        /// </summary>
        /// <param name="key">The name of the score value to set.</param>
        /// <param name="value">The new value to modify the entry by.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool ModifyScoreValue(object s, string key, float value) =>
            GetValue(key, out float currentValue) && SetValue(s, key, currentValue + value);

        /// <summary>
        /// Compare a stored system value to a new float value to see which is larger.
        /// </summary>
        /// <param name="keyA">The string of the stored A value to compare.</param>
        /// <param name="valueB">The float B value to compare.</param>
        /// <returns>Returns 1 if A is larger, -1 if B is larger, and 0 if equal.</returns>
        public float CompareAOverB(string keyA, float valueB)
        {
            if (!ValueList.TryGetValue(keyA, out float valueA))
                return 0;

            return valueA > valueB ? 1 : Math.Abs(valueA - valueB) < 0.001f ? 0 : -1;
        }

        /// <summary>
        /// Compare 2 stored system values to see which is larger.
        /// </summary>
        /// <param name="keyA">The string of the stored A value to compare.</param>
        /// <param name="keyB">The string of the stored B value to compare.</param>
        /// <returns>Returns 1 if A is larger, -1 if B is larger, and 0 if equal.</returns>
        public float CompareAOverB(string keyA, string keyB)
        {
            if (!ValueList.TryGetValue(keyA, out float valueA) || !ValueList.TryGetValue(keyB, out float valueB))
                return 0;
            
            return Math.Abs(valueA - valueB) < 0.001f ? 0 : valueA > valueB ? 1 : -1;
        }
        
        /// <summary>
        /// Override the current list of scores to a completely new list.
        /// NOTE: This makes no guarantees that there is a "Main" score entry.
        /// </summary>
        /// <param name="newList">The new list that the score list will now be a copy of.</param>
        public void OverrideScoreList(object s, Dictionary<string, float> newList) => OverrideValueList(s, newList);

        /// <summary>
        /// Completely clears all entries from the score list, setting it to an empty list.
        /// Make sure this is what you want to do, as there is no way to undo this action.
        /// </summary>
        public void ClearScoreList(object s) => ClearValueList(s);

        /// <summary>
        /// Creates and prints a log of all values to the console.
        /// </summary>
        public void PrintScores() => PrintValues();
    }
}