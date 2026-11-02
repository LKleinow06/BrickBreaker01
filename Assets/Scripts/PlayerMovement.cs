using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    //determines how quickly the paddle moves horizontally
    public float speed = 5f;
    //prevents the paddle from moving outside playable area
    public float maxX = 7.5f;


    private void Update()
    {
        //check whether keyboard is available
        if (Keyboard.current == null)
        {
            return;
        }

        //get player's horizontal keyboard input
        float movementHorizontal = GetHorizontalInput();

        MovePaddle(movementHorizontal);
    }


    private float GetHorizontalInput()
    {
        //stores the horizontal movement direction
        float movementHorizontal = 0f;

        //move left using either the left arrow or A key
        if (Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.aKey.isPressed)
        {
            movementHorizontal -= 1f;
        }

        //move right using either the right arrow or D key
        if (Keyboard.current.rightArrowKey.isPressed ||
            Keyboard.current.dKey.isPressed)
        {
            movementHorizontal += 1f;
        }

        return movementHorizontal;
    }


    private void MovePaddle(float movementHorizontal)
    {
        Vector3 position = transform.position;

        //calculate the new horizontal position
        position.x += movementHorizontal * speed * Time.deltaTime;
        //keep the paddle within the screen boundaries
        position.x = Mathf.Clamp(position.x, -maxX, maxX);

        transform.position = position;
    }
}