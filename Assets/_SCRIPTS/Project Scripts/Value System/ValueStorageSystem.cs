using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class ValueStorageSystem<T>
{
    public event EventHandler<OnValueCreatedEventArgs> OnValueCreated;
    public class OnValueCreatedEventArgs : EventArgs
    {
        public string Name;
        public T Value;
    }
    
    public event EventHandler<OnValueRemovedEventArgs> OnValueRemoved;
    public class OnValueRemovedEventArgs : EventArgs
    {
        public string Name;
    }
    
    public event EventHandler<OnValueChangedEventArgs> OnValueChanged;
    public class OnValueChangedEventArgs : EventArgs
    {
        public string Name;
        public T OldValue;
        public T NewValue;
    }
    
    public event EventHandler OnMultipleValuesCreated;
    public event EventHandler OnListRemade;
    public event EventHandler OnListCleared;
    
    protected Dictionary<string, T> ValueList;
    public string MainName { get; }
    
    
    /// <summary>
    /// Initialize the system for use.
    /// </summary>
    /// <param name="copyList">A data collection of the same type to make this a copy of.</param>
    /// <param name="mainName">The name of the main value in the system.</param>
    /// <param name="initialMainValue">The initial value of the main value in the system.</param>
    protected ValueStorageSystem(object s, IReadOnlyDictionary<string, T> copyList = null, string mainName = "Main", T initialMainValue = default(T))
    {
        ValueList = new Dictionary<string, T>();
        MainName = mainName != string.Empty ? mainName : "Main";

        if (copyList != null)
        {
            if (!copyList.ContainsKey(MainName))
                ValueList.Add(MainName, initialMainValue);
            ValueList.AddRange(copyList);
        }
        else
            ValueList.Add(MainName, initialMainValue);
    }

    /// <summary>
    /// Try and add a new value entry to the Dictionary, using the key and value.
    /// </summary>
    /// <param name="name">The string name given as the key to the new value.</param>
    /// <param name="value">The initial value which will be applied to the new entry.</param>
    /// <returns>Returns true is successful, false if unsuccessful.</returns>
    protected bool AddValueType(object s, string name, T value)
    {
        OnValueCreated?.Invoke(s, new OnValueCreatedEventArgs {Name = name, Value = value});
        return ValueList.TryAdd(name, value);
    }

    /// <summary>
    /// Add a new value entry to the Dictionary, using the key and value.
    /// </summary>
    /// <param name="collection">The list to add onto the existing value list</param>
    /// <returns></returns>
    protected void AddValueTypes(object s, Dictionary<string, T> collection)
    {
        OnMultipleValuesCreated?.Invoke(s, EventArgs.Empty);
        ValueList.AddRange(collection);
    }

    /// <summary>
    /// Try and get an existing value from the value list.
    /// </summary>
    /// <param name="key">The string name of the Dictionary entry to try and get.</param>
    /// <param name="value">The extracted value of the entry found.</param>
    /// <returns>Returns true is successful, false if unsuccessful.</returns>
    protected bool GetValue(string key, out T value) => ValueList.TryGetValue(key, out value);
    
    /// <summary>
    /// Try and get the existing main value from the value list.
    /// </summary>
    /// <param name="value">The extracted value of the entry found.</param>
    /// <returns>Returns true is successful, false if unsuccessful.</returns>
    protected bool GetMainValue(out T value) => ValueList.TryGetValue(MainName, out value);

    /// <summary>
    /// Set the value of an existing value entry in the list.
    /// </summary>
    /// <param name="key">The name of the value to set.</param>
    /// <param name="value">The new value to set the entry to.</param>
    /// <returns>Returns true if successful, false if not.</returns>
    protected bool SetValue(object s, string key, T value)
    {
        if (ValueList == null)
        {
            Debug.LogError("ValueStorageSystem: SetValue(s,T) failed. ValueList is null.");
            return false;
        }
        
        if (!ValueList.ContainsKey(key))
        {
            Debug.LogError("ValueStorageSystem: SetValue(s,T) failed. Key doesnt exist.");
            return false;
        }

        OnValueChanged?.Invoke(s, new OnValueChangedEventArgs {Name = key, OldValue = ValueList[key], NewValue = value});
        ValueList[key] = value;
        return true;
    }
    
    /// <summary>
    /// Set the value of an existing value entry in the list.
    /// </summary>
    /// <param name="keyA">The name of the value to set.</param>
    /// <param name="keyB">The new value to set the entry to.</param>
    /// <returns>Returns true if successful, false if not.</returns>
    protected bool SetValue(object s, string keyA, string keyB)
    {
        if (ValueList == null)
        {
            Debug.LogError("ValueStorageSystem: SetValue(s,s) failed. ValueList is null.");
            return false;
        }
        
        if (!ValueList.ContainsKey(keyA) || !ValueList.ContainsKey(keyB))
        {
            Debug.LogError("ValueStorageSystem: SetValue(s,s) failed. A key doesnt exist.");
            return false;
        }

        T newValue = ValueList[keyB];
        OnValueChanged?.Invoke(s, new OnValueChangedEventArgs {Name = keyA, OldValue = ValueList[keyA], NewValue = newValue});
        ValueList[keyA] = newValue;
        return true;
    }
    
    /// <summary>
    /// Removes an existing value entry from the list.
    /// </summary>
    /// <param name="key">The name of the value to remove.</param>
    /// <returns>Returns true if successful, false if not.</returns>
    protected bool RemoveValue(object s, string key)
    {
        OnValueRemoved?.Invoke(s, new OnValueRemovedEventArgs {Name = key});
        return ValueList.Remove(key);
    }
    
    /// <summary>
    /// Override the current list of values to a completely new list.
    /// NOTE: This makes no guarantees that there is a "Main" value entry.
    /// </summary>
    /// <param name="newList">The new list that the value list will now be a copy of.</param>
    protected void OverrideValueList(object s, Dictionary<string, T> newList)
    {
        OnListRemade?.Invoke(s, EventArgs.Empty);
        ValueList = newList;
    }

    /// <summary>
    /// Completely clears all entries from the value list, setting it to an empty list.
    /// NOTE: Make sure this is what you want to do, as there is no way to undo this action.
    /// </summary>
    protected void ClearValueList(object s)
    {
        OnListCleared?.Invoke(s, EventArgs.Empty);
        ValueList.Clear();
    }

    /// <summary>
    /// Creates and prints a log of all values to the console.
    /// </summary>
    protected void PrintValues()
    {
        string allValues = ValueList.Aggregate(string.Empty,
            (current, valuePair) => current + $"{valuePair.Key}:{valuePair.Value}, ");
        Debug.Log(allValues);
    }
}