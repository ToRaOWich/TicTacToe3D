using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public int boardSize = 3;
    public GameObject[,,] board;
    private Color player1Color = Color.red;
    private Color player2Color = Color.blue;
    private int currentPlayer = 1; // 1 for Player 1, 2 for Player 2
    private void Start()
    {
        CreateBoard();
    }
    private void CreateBoard()
    {
        GameObject boardParentObject = GameObject.Find("GameBoard");

        if (boardParentObject == null)
        {
            Debug.LogError("GameObject with the name 'GameBoard' not found in the Hierarchy!");
            return;
        }

        board = new GameObject[boardSize, boardSize, boardSize];
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                for (int z = 0; z < boardSize; z++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                    cube.transform.SetParent(boardParentObject.transform);
                    cube.GetComponent<Renderer>().material.color = Color.white; // Set initial color
                    CubeClickHandler clickHandler = cube.AddComponent<CubeClickHandler>();
                    clickHandler.SetGameController(this, x, y, z);
                    board[x, y, z] = cube;
                }
            }
        }
    }
    public void ClearBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                for (int z = 0; z < boardSize; z++)
                {
                    if (board[x, y, z] != null)
                    {
                        board[x, y, z].GetComponent<Renderer>().material.color = Color.white;
                    }
                }
            }
        }
        currentPlayer = 1;
        Debug.Log("Board cleared!");
    }
    public void HandleCubeClick(int x, int y, int z)
    {
        if (board[x, y, z] != null)
        {
            Renderer cubeRenderer = board[x, y, z].GetComponent<Renderer>();
            if (cubeRenderer.material.color == Color.white)
            {
                Color currentColor = (currentPlayer == 1) ? player1Color : player2Color;
                cubeRenderer.material.color = currentColor;

                if (CheckWinCondition(x, y, z, currentColor))
                {
                    Debug.Log("Player " + currentPlayer + " wins!");
                }
                else
                {
                    currentPlayer = (currentPlayer == 1) ? 2 : 1;
                }
            }
        }
    }
    bool CheckWinCondition(int x, int y, int z, Color color)
    {
        // Check row in x-axis
        if (CheckLine(color, new Vector3Int(1, 0, 0), new Vector3Int(x, y, z))) return true;
        // Check column in y-axis
        if (CheckLine(color, new Vector3Int(0, 1, 0), new Vector3Int(x, y, z))) return true;
        // Check depth in z-axis
        if (CheckLine(color, new Vector3Int(0, 0, 1), new Vector3Int(x, y, z))) return true;

        // Check diagonals in xy-plane
        if (CheckLine(color, new Vector3Int(1, 1, 0), new Vector3Int(x, y, z))) return true;
        if (CheckLine(color, new Vector3Int(1, -1, 0), new Vector3Int(x, y, z))) return true;

        // Check diagonals in xz-plane
        if (CheckLine(color, new Vector3Int(1, 0, 1), new Vector3Int(x, y, z))) return true;
        if (CheckLine(color, new Vector3Int(1, 0, -1), new Vector3Int(x, y, z))) return true;

        // Check diagonals in yz-plane
        if (CheckLine(color, new Vector3Int(0, 1, 1), new Vector3Int(x, y, z))) return true;
        if (CheckLine(color, new Vector3Int(0, 1, -1), new Vector3Int(x, y, z))) return true;

        return false;
    }

    bool CheckLine(Color color, Vector3Int direction, Vector3Int startPosition)
    {
        int count = 0;
        for (int i = -(boardSize - 1); i <= (boardSize - 1); i++)
        {
            Vector3Int checkPosition = startPosition + direction * i;
            if (IsWithinBoard(checkPosition) && GetCubeColor(checkPosition) == color)
            {
                count++;
                if (count >= boardSize)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    bool IsWithinBoard(Vector3Int position)
    {
        return position.x >= 0 && position.x < boardSize &&
               position.y >= 0 && position.y < boardSize &&
               position.z >= 0 && position.z < boardSize;
    }

    Color GetCubeColor(Vector3Int position)
    {
        if (IsWithinBoard(position) && board[position.x, position.y, position.z] != null)
        {
            return board[position.x, position.y, position.z].GetComponent<Renderer>().material.color;
        }
        return Color.clear;
    }
}
