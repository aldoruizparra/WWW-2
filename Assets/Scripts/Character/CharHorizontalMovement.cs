using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHorizontalMovement : MonoBehaviour
{
    private Rigidbody2D characterRB2D;
    private SpriteRenderer characterSpriteRenderer;
    private bool isWalkBuffer;
    private bool isWalking;

    // different speeds between moving in the air vs moving on the ground
    [SerializeField] private float airMovementSpeed = 2;
    [SerializeField] private float groundMovementSpeed = 4;
    [SerializeField] private AudioSource runningSoundEffect;

    private CharGroundChecker groundChecker;
    private Vector2 velChange = Vector2.zero;

    private CharGravityChecker charGravityChecker;

    public static bool hasMovedAlready;
    private static CharHorizontalMovement _instance;

    // responsibility of class: character's horizontal movement from arrow key input

    // Start is called before the first frame update
    void Start()
    {
        characterRB2D = GetComponent<Rigidbody2D>();
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        groundChecker = GetComponent<CharGroundChecker>();
        charGravityChecker = GetComponent<CharGravityChecker>();
        hasMovedAlready = false;
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CheckWalking();
        CheckWalkBuffer();
    }

    private void FixedUpdate()
    {
        if (isWalkBuffer)
        {
            MoveHorizontally();
            isWalkBuffer = false;
            velChange = Vector2.zero;
        }

        if (!returnWalkingState())
        {
            runningSoundEffect.Stop();
        }
    }


    // checking input for wlaking functionality
    private void CheckWalkBuffer()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            isWalkBuffer = true;
        }
    }

    // checking walking state so that the walking status may be returned & used in other classes
    private void CheckWalking()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void MoveHorizontally()
    {
        if (hasMovedAlready == false)
        {
            hasMovedAlready = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!runningSoundEffect.isPlaying)
            {
                runningSoundEffect.Play();
            }

            if (charGravityChecker.returnGravityDown())
            {
                characterSpriteRenderer.flipX = false;
            } else
            {
                characterSpriteRenderer.flipX = true;
            }
            
            if (groundChecker.returnGroundedState())
            {
                velChange.x = groundMovementSpeed;
            }
            else
            {
                velChange.x = airMovementSpeed;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!runningSoundEffect.isPlaying)
            {
                runningSoundEffect.Play();
            }

            if (charGravityChecker.returnGravityDown())
            {
                characterSpriteRenderer.flipX = true;
            }
            else
            {
                characterSpriteRenderer.flipX = false;
            }

            if (groundChecker.returnGroundedState())
            {
                velChange.x = -groundMovementSpeed;
            }
            else
            {
                velChange.x = -airMovementSpeed;
            }
        }
        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)){
            characterRB2D.velocity = new Vector2(0, characterRB2D.velocity.y);
        }

        velChange.y = characterRB2D.velocity.y;
        characterRB2D.velocity = velChange;
    }

    public bool returnWalkingState()
    {
        return isWalking;
    }

    public static CharHorizontalMovement Get()
    {
        return _instance;
    }

    public bool returnHasMovedAlready()
    {
        return hasMovedAlready;
    }

}
