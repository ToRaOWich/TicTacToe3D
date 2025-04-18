using UnityEngine;

public class CubeClickHandler : MonoBehaviour
{
    private GameManager gameController;
    private int xIndex;
    private int yIndex;
    private int zIndex;

    public void SetGameController(GameManager controller, int x, int y, int z)
    {
        gameController = controller;
        xIndex = x;
        yIndex = y;
        zIndex = z;
    }

    void OnMouseDown()
    {
        if (gameController != null)
        {
            gameController.HandleCubeClick(xIndex, yIndex, zIndex);
        }
    }
}
