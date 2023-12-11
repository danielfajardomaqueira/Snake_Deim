using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private class SnakeBodyPart
    {
        private Vector2Int gridPosition; // Posición 2D de la SnakeBodyPart
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyPartGameObject = new GameObject("Snake Body",
                typeof(SpriteRenderer));
            SpriteRenderer snakeBodyPartSpriteRenderer = snakeBodyPartGameObject.GetComponent<SpriteRenderer>();
            snakeBodyPartSpriteRenderer.sprite =
                GameAssets.Instance.snakeBodySprite;
            snakeBodyPartSpriteRenderer.sortingOrder = -bodyIndex;
            transform = snakeBodyPartGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition; // Posición 2D de la SnakeBodyPart
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0); // Posición 3D del G.O.
        }
    }

    private Vector2Int gridPosition;
    private Vector2Int startGridPosition;
    private Vector2Int gridMoveDirection;

    private float horizontalInput, verticalInput;

    private float gridMoveTimer;
    private float gridMoveTimerMax = 1f; // La serpiente se moverá a cada segundo

    private LevelGrid levelGrid;

    private int snakeBodySize; // Cantidad de partes del cuerpo (sin cabeza)
    private List<Vector2Int> snakeMovePositionsList; // Posiciones de cada parte (por orden)
    private List<SnakeBodyPart> snakeBodyPartsList;


    private void Awake()
    {
        startGridPosition = new Vector2Int(0, 0);
        gridPosition = startGridPosition;

        gridMoveDirection = new Vector2Int(0, 1); // Dirección arriba por defecto
        transform.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
        HandleMoveDirection();
        HandleGridMovement();

    }

    public void Setup(LevelGrid levelGrid)
    {
        // levelGrid de snake = levelGrid que viene por parámetro
        this.levelGrid = levelGrid;
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;

            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

            // ¿He comido comida?
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                // El cuerpo crece
                snakeBodySize++;
            }

            if (snakeMovePositionsList.Count > snakeBodySize)
            {
                snakeMovePositionsList.
                    RemoveAt(snakeMovePositionsList.Count - 1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

        }

    }
    private void HandleMoveDirection()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Cambio dirección hacia arriba
        if (verticalInput > 0) // Si he pulsado hacia arriba (W o Flecha Arriba)
        {
            if (gridMoveDirection.y != 1) // Si iba en horizontal
            {
                // Cambio la dirección hacia arriba
                gridMoveDirection.x = 0;
                gridMoveDirection.y = 1;
            }
        }

        // Cambio dirección hacia abajo
        // Input es abajo?
        if (verticalInput < 0)
        {
            // Mi dirección hasta ahora era horizontal
            if (gridMoveDirection.x != 0)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }

        // Cambio dirección hacia derecha
        if (horizontalInput > 0)
        {
            if (gridMoveDirection.y != 0)
            {
                gridMoveDirection.x = 1;
                gridMoveDirection.y = 0;
            }
        }

        // Cambio dirección hacia izquierda
        if (horizontalInput < 0)
        {
            if (gridMoveDirection.y != 0)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
    }

    private float GetAngleFromVector(Vector2Int direction)
    {
        float degrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (degrees < 0)
        {
            degrees += 360;
        }

        return degrees - 90;
    }
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeBodyGridPosition()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionsList);
        return gridPositionList;
    }

    private void CreateBodyPart()
    {
        snakeBodyPartsList.Add(new SnakeBodyPart(snakeBodySize));
    }

    private void UpdateBodyParts()
    {
        for (int i = 0; i < snakeBodyPartsList.Count; i++)
        {
            snakeBodyPartsList[i].SetGridPosition(snakeMovePositionsList[i]);
        }
    }
}
