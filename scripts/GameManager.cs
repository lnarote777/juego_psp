using Godot;
using System;


public static class GameManager
{
    // Variables estáticas para almacenar el puntaje y las monedas
    public static int Coins { get; private set; } = 0;
    public static int Score { get; private set; } = 0;

    // Referencias a las etiquetas del HUD (pueden ser configuradas externamente)
    private static Label _scoreLabel;
    private static Label _coinsLabel;

    // Método para inicializar el GameManager con las etiquetas del HUD
    public static void InitializeHUD(Label scoreLabel, Label coinsLabel)
    {
        _scoreLabel = scoreLabel;
        _coinsLabel = coinsLabel;

        UpdateHUD();
    }

    // Actualiza el HUD con los valores actuales de las monedas y el puntaje
    public static void UpdateHUD()
    {
        if (_scoreLabel != null)
        {
            _scoreLabel.Text = "Score: " + Score;
        }

        if (_coinsLabel != null)
        {
            _coinsLabel.Text = "Coins: " + Coins;
        }
    }

    // Método para añadir una moneda
    public static void AddCoin()
    {
        Coins += 1;
        UpdateHUD();
    }

    // Método para añadir puntos
    public static void AddPoint()
    {
        Score += 10;
        UpdateHUD();
    }

    // Método para resetear el juego
    public static void ResetGame()
    {
        _scoreLabel = null;
        _coinsLabel = null;
        Coins = 0;
        Score = 0;
        if (_coinsLabel != null && _scoreLabel != null)
        {
            UpdateHUD();
        }
        else
        {
            GD.PrintErr("Error: Labels not found during reset.");
        }

        InitializeHUD(_scoreLabel, _coinsLabel);
    }
}


