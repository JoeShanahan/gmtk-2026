using UnityEngine;

public class MainMenuPage : MonoBehaviour
{
    public enum PageType
    {
        Root,
        LevelSelect,
        Settings,
        About
    }

    public PageType Type;
}
