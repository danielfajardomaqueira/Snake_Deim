using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private LevelGrid levelGrid;
    private Snake snake;


    public static GameManager Instance { get; private set; }

    public const int POINTS = 100; //Puntos al comer comida

    private int score; //Puntuacion player
    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Debug.LogError("There is more than one Instance");
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Start()
    {
        GameObject snakeHeadGameObject = new GameObject("Snake Head");
        SpriteRenderer snakeSpriteRenderer = snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.Instance.snakeHeadSprite;
        snake = snakeHeadGameObject.AddComponent<Snake>();

        // Configurar el LevelGrid
        levelGrid = new LevelGrid(20, 20);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }
    public int GetScore()
    {
        return score;
    }
    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
    }
}
