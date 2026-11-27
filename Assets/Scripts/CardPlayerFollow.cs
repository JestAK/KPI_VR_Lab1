using TMPro;
using UnityEngine;

public class CardPlayerFollow : MonoBehaviour
{
    private Transform head;

    [Header("Item Name")]
    [SerializeField] private string itemName;

    [Header("Item Name Text")]
    [SerializeField] private TMP_Text itemNameText;

    private void Start()
    {
        head = Camera.main.transform;
        itemNameText.text = itemName;
    }

    void LateUpdate()
    {
        Vector3 direction = transform.position - head.position;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
