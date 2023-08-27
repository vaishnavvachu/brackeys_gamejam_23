using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OxygenController : MonoBehaviour
{
    [SerializeField] 
    private float durationToTotallyDeplete = 10f;
    
    private Image oxygenImage;
    private bool oxygenDepleted;

    private float timer;
    
    public UnityEvent onOxygenDepleted;

    private void Start()
    {
        oxygenImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (oxygenDepleted) { return; }
        ReduceOxygenAmount();
    }

    private void ReduceOxygenAmount()
    {
        timer += Time.deltaTime;
        var progress = Mathf.Clamp01(timer / durationToTotallyDeplete);
        var currentFill = Mathf.Lerp(1.0f, 0.0f, progress);
        if (currentFill <= 0)
        {
            oxygenDepleted = true;
            onOxygenDepleted?.Invoke();
            oxygenImage.fillAmount = 0;
        }
        else { oxygenImage.fillAmount = currentFill; }
    }

    public void RefillOxygen()
    {
        oxygenImage.fillAmount = 1;
        timer = 0;
    }
}
