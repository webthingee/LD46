using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    public Image foreground;
    public int maxValue = 10;
    [Range(0,10)] public int currentValue = 10;
    
    private void Update()
    {
        if (foreground != null)
        {
            float val = (float)currentValue / (float)maxValue;
            Debug.Log($"VAL: {val}");
            foreground.fillAmount = val;
        }
    }
}