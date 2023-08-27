using System;
using UnityEngine;
using UnityEngine.UI;

public class OxygenController : MonoBehaviour
{
    [SerializeField] 
    private float durationToTotallyDeplete = 10f;

    private float defaultDepleteDuration;
    
    private Image oxygenImage;
    private bool oxygenDepleted;

    private float startAmount = 1f;
    
    private float timer;
    
    public event Action OnOxygenDepleted;

    private void Start()
    {
        defaultDepleteDuration = durationToTotallyDeplete;
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
        var progress = Mathf.Clamp01(timer / (durationToTotallyDeplete));
        var currentFill = Mathf.Lerp(startAmount, 0.0f, progress);
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
        var currentAmount = oxygenImage.fillAmount;
        ChangeOxygenAmount(currentAmount - amount);
    }

    private void ChangeOxygenAmount(float amount)
    {
        startAmount = amount;
        oxygenImage.fillAmount = startAmount;
        timer = 0;
        durationToTotallyDeplete = defaultDepleteDuration * (startAmount / 1f);
    }
}
