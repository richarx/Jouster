using UnityEngine;

public class JoustPreparation : MonoBehaviour
{
    public static JoustPreparation instance;

    private bool isRegistered;
    public bool IsRegistered => isRegistered;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    public void RegisterPlayer()
    {
        isRegistered = true;
    }
}
