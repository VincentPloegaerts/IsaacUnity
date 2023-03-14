using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;  
    [SerializeField] Rigidbody2D playerRigidbody = null;
    [SerializeField] Vector2 direction = Vector2.zero;  //which direction is the player facing / going
    [SerializeField] Animator playerAnimationController = null;
    static readonly int DirectionXHash = Animator.StringToHash("DirectionX");
    static readonly int DirectionYHash = Animator.StringToHash("DirectionY");
    static readonly int MovementXHash = Animator.StringToHash("MovementX");
    static readonly int MovementYHash = Animator.StringToHash("MovementY");

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.useFullKinematicContacts = true;
        playerAnimationController = GetComponent<Animator>();
        GameLogic.Instance.OnResetLevel += ResetPlayer;
    }

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        playerAnimationController.SetFloat(MovementXHash,direction.x);
        playerAnimationController.SetFloat(MovementYHash,direction.y);
        if (direction.x != 0.0f || direction.y != 0.0f)
        {
            playerAnimationController.SetFloat(DirectionXHash, direction.x); //change direction only if there's inputs so the player stays oriented the same way for the idle
            playerAnimationController.SetFloat(DirectionYHash, direction.y); //if the player let go of inputs
        }
        playerRigidbody.MovePosition(playerRigidbody.position +  (moveSpeed / 10.0f) * direction); //player is too fast setting up speed between
                                                                                                   //0 and 1 was a pain so added a /10 factor
                                                                                                   
    }

    void ResetPlayer()
    {
        transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
        Camera.main.transform.position= new Vector3(0.0f, 0.0f,  Camera.main.transform.position.z);
    }
}
