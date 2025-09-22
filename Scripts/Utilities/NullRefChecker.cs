using UnityEngine;

public interface IValidatable
{
    void Validate();
    /*
    public void Validate()
    {
        if (someObject == null)
            Debug.LogError($"Missing assignment for 'someObject' in {nameof(ExampleComponent)} on {gameObject.name}", this);

        if (rigidbodyComponent == null)
            Debug.LogError($"Missing assignment for 'rigidbodyComponent' in {nameof(ExampleComponent)} on {gameObject.name}", this);
    }
    */
}

public static class NullRefChecker
{
    // Note: instance is not always a MonoBehaviour

    public static void Validate<T>(T instance) where T : class, IValidatable
    {
        if (instance == null)
        {
            Debug.LogError("Instance is null. Validation cannot be performed.");
            return;
        }

        instance.Validate();
    }
}

/// <summary>
/// A custom PropertyAttribute to bypass the above Validate check.
/// </summary>
public class OptionalAttribute : PropertyAttribute
{
}

// Mark a field with the Optional attribute if it's not required
// public class MyClass : MonoBehaviour
// {
//    public string requiredField;

//    [Optional]
//    public string optionalField;
// }
//
// This allows the Validate to ignore the field.
