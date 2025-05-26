using UnityEngine;

public class RangeWithColorAttribute : PropertyAttribute
{
    public float min;
    public float max;
    public float warningThreshold;
    public Color color;

    public RangeWithColorAttribute(float min, float max, float warningThreshold, string color)
    {
        this.min = min;
        this.max = max;
        this.warningThreshold = warningThreshold;

        if (!ColorUtility.TryParseHtmlString(color, out this.color))
        {
            this.color = Color.white; // Default to white if parsing fails
        }
    }
}
