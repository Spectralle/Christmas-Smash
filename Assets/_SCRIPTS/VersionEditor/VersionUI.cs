using UnityEngine;
using TMPro;
using Assets.Scripts.VersionEditor;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionUI : MonoBehaviour
{
    [OnInspectorInit]
    private void Start() =>
        GetComponent<TextMeshProUGUI>().SetText("v" + VersionInformation.ToString());
}
