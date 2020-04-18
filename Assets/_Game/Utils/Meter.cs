using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    public Image foreground;
    public int maxValue = 10;
    [SerializeField] private int _currentValue = 10;
    public bool hasMinMax;

    public void AdjustValue(int value)
    {
        _currentValue += value;
        
        if (_currentValue > maxValue)
        {
            _currentValue = maxValue;
        }

        if (_currentValue < 0)
        {
            _currentValue = 0;
        }
    }

    private void Update()
    {
        if (foreground != null)
        {
            float val = (float)_currentValue / (float)maxValue;
            foreground.fillAmount = val;
        }
    }
}