using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    //determines how many columns and rows of bricks generate
    public Vector2Int size = new Vector2Int(12, 6);
    //controls horizontal and vertical spacing between bricks
    public Vector2 offset = new Vector2(1.1f, 0.6f);
    //determines starting vertical position of first row
    public float topOffset = 2.5f;


    [Header("Brick Prefab")]
    public GameObject brickPrefab;

    private void Awake()
    {
        //check whether a brick prefab has been assigned
        if (brickPrefab == null)
        {
            Debug.LogError("LevelGenerator, assign a Brick Prefab");
            return;
        }

        GenerateBricks();
    }

    private void GenerateBricks()
    {
        //loop through each column
        for (int i = 0; i < size.x; i++)
        {
            //loop through each row
            for (int j = 0; j < size.y; j++)
            {
                CreateBrick(i, j);
            }
        }
    }

    private void CreateBrick(int x, int y)
    {
        GameObject newBrick = Instantiate(brickPrefab, transform);

        //calculates the horizontal position of the brick, centers the grid around LevelGenerator object
        float brickX = ((size.x - 1) * 0.5f - x) * offset.x;
        //calculates vertical position of the brick
        float brickY = topOffset - y * offset.y;
        
        newBrick.transform.position = transform.position + new Vector3(brickX, brickY, 0f);

    }
}