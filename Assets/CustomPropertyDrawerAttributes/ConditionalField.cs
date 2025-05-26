using UnityEngine;

// Attribute that makes a field appear/disappear based on a bool field
public class ConditionalField : PropertyAttribute
{
    public string conditionalFieldName;
    public bool inverted;
    public ConditionalField(string conditionalFieldName, bool inverted = false)
    {
        this.conditionalFieldName = conditionalFieldName;
        this.inverted = inverted;
    }
}


