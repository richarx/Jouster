using UnityEngine;

public class JoustTrigger : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BasicTrigger>().OnPlayerEnterTrigger.AddListener(JoustPreparation.instance.StartJoust);
    }
}
