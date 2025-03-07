using Godot;
using System;

public partial class Game : Node2D
{
    private Label _scoreLabel;
    private Label _coinsLabel;

    public override void _Ready()
    {
        // Obtener las etiquetas del HUD
        _scoreLabel = GetNode<Label>("Wizard/HUD/Score");
        _coinsLabel = GetNode<Label>("Wizard/HUD/Coins");

        // Inicializar el GameManager con las etiquetas
        GameManager.InitializeHUD(_scoreLabel, _coinsLabel);
    }
}
