using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
     public static MobileJoystick Instance;
    // Start is called before the first frame update
    [Header("Elements")]
    [SerializeField] private RectTransform _joystickOutline;
    [SerializeField] private RectTransform _joystickKnob;

    [Header("Settings")]
    [SerializeField] private float _moveFactor;
    private Vector3 _clickedPosition;
    private Vector3 _move;
   [HideInInspector] public bool CanControl;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(Instance);
    }
    void Start()
    {
        HideJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanControl)
        ControlJoystick();
    }
    public void ClickedOnJoystickZoneCallBack()
    {
        _clickedPosition=Input.mousePosition;
        _joystickOutline.position = _clickedPosition;
        ShowJoystick();
        CanControl = true;
    }
    private void ShowJoystick()
    {
        _joystickOutline.gameObject.SetActive(true);
    }
    private void HideJoystick()
    {
        _joystickOutline.gameObject.SetActive(false);
        CanControl = false;
        _move = Vector3.zero;
    }
    private void ControlJoystick()
    {
        Vector3 currentPosition=Input.mousePosition;
        Vector3 direction = currentPosition - _clickedPosition;

        // joystick ýn outline ýna sýnýrlandýrýlmasý 
        float moveMagnitude=direction.magnitude* _moveFactor / Screen.width;
        moveMagnitude=Mathf.Min(moveMagnitude, _joystickOutline.rect.width/2);

        _move = direction.normalized*moveMagnitude;
        Vector3 targetPosition = _clickedPosition + _move;
        _joystickKnob.position = targetPosition;
        if (Input.GetMouseButtonUp(0))
            HideJoystick();
    }
    public Vector3 GetMoveVector()
    {
        return _move;
    }
}
