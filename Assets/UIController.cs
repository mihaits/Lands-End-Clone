using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image Hand;
    public Image Grab;

    private static UIController _instance;

    public void Start()
    {
        _instance = this;
    }

    public static void FocusManipulatable()
    {
        if (_instance.Grab.enabled == false)
            _instance.Hand.enabled = true;
    }

    public static void FocusExitManipulatable()
    {
        _instance.Hand.enabled = false;
    }

    public static void ClickManipulatable()
    {
        _instance.Hand.enabled = false;
        _instance.Grab.enabled = true;
    }

    public static void ClickUpManipulatable()
    {
        _instance.Grab.enabled = false;
    }
}
