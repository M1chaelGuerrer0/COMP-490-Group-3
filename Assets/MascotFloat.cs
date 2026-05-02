using UnityEngine;

/// <summary>
/// Creates a floating and slightly rotating motion for the mascot.
/// </summary>
public class MascotFloat : MonoBehaviour
{
    public float floatSpeed = 2f; // Speed of the floating motion
    public float floatAmount = 5f; // How much the mascot floats up and down

    private Vector3 startPos;

    // Store the initial position when the object starts
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update the position and rotation every frame to create floating effect
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);

        float rotationZ = Mathf.Sin(Time.time * floatSpeed) * 5f;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }
}