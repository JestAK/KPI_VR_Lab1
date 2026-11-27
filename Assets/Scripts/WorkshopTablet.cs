
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkshopTablet : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject[] pages;

    [Header("Welcome")]
    [SerializeField] private TMP_Text welcomeText;

    [Header("Instruction")]
    [SerializeField] private Toggle instructionToggle;

    [Header("Knowledge Check")]
    [SerializeField] private TMP_Text feedbackText;

    [Header("Shift")]
    [SerializeField] private Slider shiftHoursSlider;
    [SerializeField] private TMP_Text shiftHoursText;

    private bool quizPassed = false;
    private bool shiftStarted = false;

    private void Start()
    {
        ResetTablet();
    }

    public void UpdateWelcomeText()
    {
        string username = PlayerPrefs.GetString("WorkerName", "");

        welcomeText.text = $"Вітаємо на заводі, {username}!";
    }

    public void ShowPage(int pageIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == pageIndex);
        }

        if (pageIndex == 0)
        {
            UpdateWelcomeText();
        }
    }

    public void ConfirmInstruction(bool isConfirmed)
    {
        if (isConfirmed)
        {
            ShowPage(2);
        }
    }

    public void CheckAnswer(bool isCorrect)
    {
        quizPassed = isCorrect;

        if (isCorrect)
        {
            feedbackText.text = "";
            ShowPage(3);
        }
        else
        {
            feedbackText.text = "Неправильно. Спробуйте ще раз";
        }
    }

    public void UpdateShiftHours(float hours)
    {
        shiftHoursText.text = $"{hours:0} год";
    }

    public void StartShift()
    {
        if (!instructionToggle.isOn || !quizPassed || shiftStarted)
        {
            return;
        }

        PlayerPrefs.SetInt("InstructionPassed", 1);

        PlayerPrefs.SetInt(
            "ShiftHours",
            (int)shiftHoursSlider.value
        );

        PlayerPrefs.Save();

        shiftStarted = true;

        ShowPage(4);
    }

    public void ResetTablet()
    {
        quizPassed = false;
        shiftStarted = false;

        instructionToggle.SetIsOnWithoutNotify(false);
        feedbackText.text = "";

        shiftHoursSlider.SetValueWithoutNotify(8);
        UpdateShiftHours(shiftHoursSlider.value);

        ShowPage(0);
    }
}
