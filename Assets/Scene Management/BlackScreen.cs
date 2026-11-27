using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public static BlackScreen instance;

    private Image image;

    public Image Image => image;

    private void Awake()
    {
        instance = this;
        image = GetComponent<Image>();
    }
}
