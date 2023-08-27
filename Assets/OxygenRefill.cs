using UnityEngine;

public class OxygenRefill : MonoBehaviour
{
    private OxygenController oxygenController;

    private void Start()
    {
        oxygenController = FindObjectOfType<OxygenController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var colGameObject = col.gameObject;

        if (!colGameObject.CompareTag("Player")) { return; }
        if (!oxygenController) { return; }
        
        oxygenController.RefillOxygen();
    }
}
