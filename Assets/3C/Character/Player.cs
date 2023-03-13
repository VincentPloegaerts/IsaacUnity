using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;  
    [SerializeField] Rigidbody2D playerRigidbody = null;
    [SerializeField] Vector2 direction = Vector2.zero;  //which direction is the player facing / going
    [SerializeField] Animator playerAnimationController = null;
    static readonly int DirectionXHash = Animator.StringToHash("DirectionX");
    static readonly int DirectionYHash = Animator.StringToHash("DirectionY");

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.useFullKinematicContacts = true;
        playerAnimationController = GetComponent<Animator>();
    }

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        playerAnimationController.SetFloat(DirectionXHash,direction.x);
        playerAnimationController.SetFloat(DirectionYHash,direction.y);
        playerRigidbody.MovePosition(playerRigidbody.position +  (moveSpeed / 10.0f) * direction); //player is too fast setting up speed between
                                                                                                   //0 and 1 was a pain so added a /10 factor
    }
}
