using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CreditsData", menuName = "Scriptable Objects/CreditsData")]
public class CreditsData : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string Name;
        public Sprite Icon;
        public string ItchLink;
    }

    public Entry[] People;
}
