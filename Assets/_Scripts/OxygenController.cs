using System;
using UnityEngine;
using UnityEngine.UI;

public class OxygenController : MonoBehaviour
{
    [SerializeField] 
    private float durationToTotallyDeplete = 10f;
    
    private Image oxygenImage;
    private bool oxygenDepleted;

    private float maxAmount = 1f;
    
    private float timer;
    
    public event Action OnOxygenDepleted;

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
        var currentFill = Mathf.Lerp(maxAmount, 0.0f, progress);
        if (currentFill <= 0)
        {
            oxygenDepleted = true;
            OnOxygenDepleted?.Invoke();
            oxygenImage.fillAmount = 0;
        }
        else { oxygenImage.fillAmount = currentFill; }
    }

    public void RefillOxygen()
    {
        ChangeOxygenAmount(1);
    }
    
    public void ReduceOxygenAmount(float amount)
    {
        ChangeOxygenAmount(amount);
    }

    private void ChangeOxygenAmount(float amount)
    {
        maxAmount = amount;
        oxygenImage.fillAmount = amount;
        timer = 0;
    }
}
