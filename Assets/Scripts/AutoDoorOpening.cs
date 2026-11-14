using UnityEngine;

public class AutoDoorOpening : MonoBehaviour
{
    public Transform leftDoorPivot;
    public Transform rightDoorPivot;

    private float openAngle = 90f;
    private float rotationSpeed = 120f;

    private float currentAngle = 0f;
    private bool isOpen = false;

    private void Update()
    {
        float step = rotationSpeed * Time.deltaTime;

        if (isOpen && currentAngle < openAngle)
        {
            step = Mathf.Min(step, openAngle - currentAngle);

            leftDoorPivot.Rotate(Vector3.up, step);
            rightDoorPivot.Rotate(Vector3.up, -step);

            currentAngle += step;
        }

        else if (!isOpen && currentAngle > 0f)
        {
            step = Mathf.Min(step, currentAngle);

            leftDoorPivot.Rotate(Vector3.up, -step);
            rightDoorPivot.Rotate(Vector3.up, step);

            currentAngle -= step;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
            Debug.Log("Doors opening");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = false;
            Debug.Log("Doors closing");
        }
    }
}