using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerAnim))]
// bu cod otomatik olarak charactercontroller � cag�r�or
public class PlayerControl : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick _joystick;
    private PlayerAnim _playerAnimator;
    private CharacterController _characterController;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;
    private float raycastDistance = 0.3f; // Karakterin alt�ndaki y�zeyi kontrol etmek i�in raycast mesafesi
    private bool isFalling = false; // D���� durumunu kontrol etmek i�in bir bayrak
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<PlayerAnim>();
      
    }

    // Update is called once per frame
    void Update()
    {
        ManageMovement();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // Karakter yere temas ediyor, d��me durumunu s�f�rla
            isFalling = false;
            _characterController.enabled = true;
        }
        else
        {
            // Karakter yere temas etmiyor, d���� animasyonunu ba�lat
            if (!isFalling)
            {
                isFalling = true;
                Fall();
            }
        }
    }
    private void Fall()
    {
        _characterController.enabled = false;
        // Karakterin �u anki Y pozisyonunu al�n
        float currentY = transform.position.y;

        // D��me animasyonunu ba�lat
        LeanTween.moveY(gameObject, currentY - 0.3f, 0.5f) // Y pozisyonunu 0.3 birim a�a��ya kayd�r
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() =>
            {
                isFalling = false; // D��me durumunu s�f�rla
            });
    }
    private void ManageMovement()
    {
        Vector3 moveVector=_joystick.GetMoveVector()*_moveSpeed*Time.deltaTime/Screen.width;

        moveVector.z = moveVector.y;
        moveVector.y = 0;

       // Debug.Log("Move vector=" + moveVector);
        _characterController.Move(moveVector);

        _playerAnimator.ManageAnimations(moveVector);

    }
   
}
