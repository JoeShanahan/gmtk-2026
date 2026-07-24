using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public GameObject Prefab;

    public int SecondsBirdie;
    public int SecondsPar;
}
