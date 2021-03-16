using System.Globalization;
using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimerText = null;

    private void Awake()
    {
        InvokeRepeating(nameof(UpdateTime), 0f, 0.1f);
    }

    private void UpdateTime()
    {
        TimerText.text = GameManager.TimeLeft.ToString("00.0", CultureInfo.InvariantCulture);
    }
}