using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    [Header("Player Input Actions")]
    [SerializeField]
    public   InputActionReference MoveAction;
    [SerializeField]
    public   InputActionReference ShootAction;
    [SerializeField]
    public   InputActionReference BombAction;
    [SerializeField]
    public   InputActionReference FocusAction;


    [Header("UI Input Actions")]
    [SerializeField]
    public InputActionReference SubmitAction;



    public static InputManager Instance;
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void EnablePlayerInput(bool enable)
    {
        if (enable)
        {
             
            MoveAction.action.Enable();
            ShootAction.action.Enable();
            BombAction.action.Enable();
            FocusAction.action.Enable();
        }
        else

        {
             
            MoveAction.action.Disable();
            ShootAction.action.Disable();
            BombAction.action.Disable();
            FocusAction.action.Disable();
        }
    }

    private void EnableUIInput(bool enable)
    {
        if (enable)
        {
            SubmitAction.action.Enable();
        }
        else
        {
            SubmitAction.action.Disable();
        }
    }

    public void SwitchToPlayerInput()
    {
        EnableUIInput(false);
        EnablePlayerInput(true);
    }

    public void SwitchToUIInput()
    {
        EnablePlayerInput(false);
        EnableUIInput(true);
    }
}
