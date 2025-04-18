using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] targetBoard; // Drag your GameController (the parent of the cubes) here
    public float moveSpeed = 5f;
    public float verticalMoveSpeed = 2f; // Speed for vertical movement
    public float zoomSpeed = 5f;       // Speed for zooming
    public float minZoomDistance = 2f;  // Minimum zoom distance
    public float maxZoomDistance = 15f;
    public float minHeight = 1f;
    public float maxHeight = 10f;

    private Vector3 offset;
    private Transform currentTarget;
    private int targetIndex = 0;
    private GameObject[,,] gameBoard;
    [SerializeField] private GameManager gameManager;
    private float currentDistance;

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager script not found in the scene!");
            enabled = false;
            return;
        }

        gameBoard = gameManager.board;
        if (gameBoard == null || gameBoard.Length == 0)
        {
            Debug.LogError("Board array in GameManager is null or empty!");
            enabled = false;
            return;
        }
        Bounds boardBounds = new Bounds(gameBoard[0, 0, 0].transform.position, Vector3.zero);
        foreach (GameObject cube in gameBoard)
        {
            if (cube != null)
            {
                boardBounds.Encapsulate(cube.GetComponent<Renderer>().bounds);
            }
        }

        GameObject centerTargetObject = new GameObject("CameraTarget");
        centerTargetObject.transform.position = boardBounds.center;
        currentTarget = centerTargetObject.transform;

        offset = transform.position - currentTarget.position;
        currentDistance = offset.magnitude;
    }

    void Update()
    {
        if (currentTarget != null)

        {

            MoveCam();
            Zoom();
            transform.LookAt(currentTarget.position);
        }
    }
    private void MoveCam()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 horizontalMoveDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
        transform.Translate(horizontalMoveDirection * moveSpeed * Time.deltaTime);

        float verticalMovement = verticalInput * verticalMoveSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * verticalMovement);

        float currentHeight = transform.position.y;
        if (currentHeight < minHeight)
        {
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }
        else if (currentHeight > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }
    }
    private void Zoom()
    {
        float zoomInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            zoomInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            zoomInput = 1f;
        }

        float zoomAmount = zoomInput * zoomSpeed * Time.deltaTime;
        Vector3 zoomDirection = (transform.position - currentTarget.position).normalized;
        transform.Translate(zoomDirection * zoomAmount, Space.World);


        float currentDistance = Vector3.Distance(transform.position, currentTarget.position);
        if (currentDistance < minZoomDistance)
        {
            transform.position = currentTarget.position + zoomDirection * minZoomDistance;
        }
        else if (currentDistance > maxZoomDistance)
        {
            transform.position = currentTarget.position + zoomDirection * maxZoomDistance;
        }
    }
}