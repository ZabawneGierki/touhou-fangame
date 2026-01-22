using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChapterEndingScreen : MonoBehaviour
{
    PointsByChapter pointsByChapter;

    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI grazeText;
    [SerializeField] TextMeshProUGUI bonusText;
    [SerializeField] TextMeshProUGUI totalText;

  
    private void OnEnable()
    {
            InputManager.Instance.SubmitAction.action.performed += OnSubmit;

        pointsByChapter = Score.instance.GetCurrentPoints();

        pointsText.text = pointsByChapter.points.ToString();
        grazeText.text = pointsByChapter.graze.ToString();
        
        bonusText.text = 1000.ToString();
        int total = pointsByChapter.points + pointsByChapter.graze + 1000;
        totalText.text = total.ToString();

        // disable player control and enable UI.
        InputManager.Instance.SwitchToUIInput();

    }

    private void OnDisable()
    {
        InputManager.Instance.SubmitAction.action.performed -= OnSubmit;
        // enable player control and disable UI.
        InputManager.Instance.SwitchToPlayerInput();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        // proceed to next chapter or main menu
        Debug.Log("Submit pressed on Chapter Ending Screen");
        EnemySpawner.instance.StartNextChapter();
    }


}
