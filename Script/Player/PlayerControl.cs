using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerAnim))]
// bu cod otomatik olarak charactercontroller ý cagýrýor
public class PlayerControl : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick _joystick;
    private PlayerAnim _playerAnimator;
    private CharacterController _characterController;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;
    private float raycastDistance = 0.3f; // Karakterin altýndaki yüzeyi kontrol etmek için raycast mesafesi
    private bool isFalling = false; // Düþüþ durumunu kontrol etmek için bir bayrak
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
            // Karakter yere temas ediyor, düþme durumunu sýfýrla
            isFalling = false;
            _characterController.enabled = true;
        }
        else
        {
            // Karakter yere temas etmiyor, düþüþ animasyonunu baþlat
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
        // Karakterin þu anki Y pozisyonunu alýn
        float currentY = transform.position.y;

        // Düþme animasyonunu baþlat
        LeanTween.moveY(gameObject, currentY - 0.3f, 0.5f) // Y pozisyonunu 0.3 birim aþaðýya kaydýr
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() =>
            {
                isFalling = false; // Düþme durumunu sýfýrla
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
