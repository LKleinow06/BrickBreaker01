using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class BouncyBall : MonoBehaviour
{
    [Header("Ball Settings")]
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public float startingSpeed = 8f;

    [Header("UI References")]
    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;

    public GameObject gameOverPanel;
    public GameObject youWinPanel;

    //stores the ball's physics component
    private Rigidbody2D rb;

    //tracks the player's current score and remaining lives
    private int score = 0;
    private int lives = 3;

    //tracks bricks in level
    private int brickCount = 0;

    private Vector3 startingPosition;
    private bool gameEnded = false;


    private void Start()
    {
        //make sure game is not paused when the scene starts
        Time.timeScale = 1f;

        //get the rigidbody2d attached to the ball
        rb = GetComponent<Rigidbody2D>();
        //disable gravity because the ball moves using its own velocity
        rb.gravityScale = 0f;

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        startingPosition = transform.position;

        //find out how many bricks were generated in the level
        InitializeBrickCount();
        //hide the game over and you win panels at the beginning
        InitializePanels();

        score = 0;
        lives = 3;
        gameEnded = false;

        //update ui to display starting values
        UpdateScoreUI();
        UpdateLivesUI();

        //launch the ball immediately when the game starts
        rb.linearVelocity = Vector2.zero;
        Time.timeScale = 1f;

        LaunchBall();
    }

    private void InitializeBrickCount()
    {
        LevelGenerator generator = FindAnyObjectByType<LevelGenerator>();

        if (generator != null)
        {
            brickCount = generator.transform.childCount;
            Debug.Log("Initial brick count: " + brickCount);
        }
        else
        {
            Debug.LogError("BouncyBall, LevelGenerator not found");
        }
    }

    private void InitializePanels()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (youWinPanel != null)
        {
            youWinPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameEnded)
        {
            return;
        }
        //check whether the ball has fallen below the playable area
        if (transform.position.y < minY)
        {
            LoseLife();
            return;
        }

        //keep the ball from exceeding its max allowed speed
        LimitBallVelocity();
    }

    private void LimitBallVelocity()
    {
        //check whether the ball is moving faster than max speed
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }
    }

    private void LaunchBall()
    {
        //generate a slightly randomized upward direction
        Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        //apply starting speed to ball
        rb.linearVelocity = direction * startingSpeed;
    }

    private void LoseLife()
    {
        lives--;

        Debug.Log("Player lost a life, remaining: " + lives);

        //update the life icons on the screen
        UpdateLivesUI();

        //check whether player has lost all lives
        if (lives <= 0)
        {
            GameOver();
            return;
        }

        //return ball to starting position
        ResetBall();
    }

    private void ResetBall()
    {
        //stop the ball before moving it back to its starting position
        rb.linearVelocity = Vector2.zero;
        transform.position = startingPosition;

        //automatically launch the ball again
        LaunchBall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ignore collisions after game has ended
        if (gameEnded)
        {
            return;
        }

        //check whether the ball collided with a brick
        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject);

            score += 10;
            UpdateScoreUI();

            brickCount--;

            Debug.Log("Brick destroyed, remaining: " + brickCount);

            //check whether every brick has been destroyed
            if (brickCount <= 0)
            {
                WinGame();
            }
        }
    }

    private void UpdateScoreUI()
    {
        //display the player's score using 5 digits
        if (scoreTxt != null)
        {
            scoreTxt.text = score.ToString("00000");
        }
    }

    private void UpdateLivesUI()
    {
        //make sure the icon array exists
        if (livesImage == null)
        {
            return;
        }

        //activates only the icons representing the player's remaining lives
        for (int i = 0; i < livesImage.Length; i++)
        {
            if (livesImage[i] != null)
            {
                livesImage[i].SetActive(i < lives);
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over: Player has lost all lives.");

        gameEnded = true;

        rb.linearVelocity = Vector2.zero;

        //display game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Panel Active Self: " + gameOverPanel.activeSelf);
            Debug.Log("Panel Active In Hierarchy: " + gameOverPanel.activeInHierarchy);
            Debug.Log("Game Over Panel successful");
        }
        else
        {
            Debug.Log("Game Over Panel not assigned");
        }

        //pause the game
        Time.timeScale = 0f;
    }

    private void WinGame()
    {
        Debug.Log("YOU WIN!! All bricks have been destroyed");

        gameEnded = true;

        rb.linearVelocity = Vector2.zero;

        //display the you win panel
        if (youWinPanel != null)
        {
            youWinPanel.SetActive(true);
            Debug.Log("You Win Panel successfully activated");
        }
        else
        {
            Debug.Log("You win panel not assigned");
        }

        //pause the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}