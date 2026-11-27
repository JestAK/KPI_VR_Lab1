using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UserTablet : MonoBehaviour
{
    [SerializeField] private GameObject tablet;
    [SerializeField] private Transform head;
    [SerializeField] private float distance = 0.65f;
    [SerializeField] private TMP_InputField nameInput;

    private void Start()
    {
        PlayerPrefs.GetString("WorkerName", "");
        tablet.SetActive(false);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            ToggleTablet();
        }

        if (tablet.activeSelf &&
            Vector3.Distance(head.position, tablet.transform.position) > 1.2f)
        {
            MoveTablet();
        }
    }

    public void ToggleTablet()
    {
        if (tablet.activeSelf)
        {
            tablet.SetActive(false);
            return;
        }

        MoveTablet();

        tablet.SetActive(true);
    }

    public void MoveTablet()
    {
        Vector3 forward = head.forward;
        forward.y = 0;
        forward.Normalize();

        tablet.transform.position =
            head.position + forward * distance;

        tablet.transform.rotation =
            Quaternion.LookRotation(forward);
    }

    public void SaveProfile()
    {
        PlayerPrefs.SetString("WorkerName", nameInput.text);
        PlayerPrefs.Save();
    }
}
