using UnityEngine;

public class ColoredHeaderAttribute : PropertyAttribute
{
    public string header;
    public float height;
    public Color color;

    public ColoredHeaderAttribute(string header, float height = 30f, float r = 0.35f, float g = 0.35f, float b = 0.35f)
    {
        this.header = header;
        this.height = height;
        this.color = new Color(r, g, b);
    }
}