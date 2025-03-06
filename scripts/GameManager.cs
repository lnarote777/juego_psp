using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; } 
    private int _coins = 0;
    private int _score = 0;

    Label _scoreLabel;
    Label _coinsLabel;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            SetMeta("singleton", true);
        }
        else
        {
            QueueFree(); // Evita duplicados si por error se instancia más de una vez
            return;
        }

        _scoreLabel = GetNode<Label>("HUD/Score");
        _coinsLabel = GetNode<Label>("HUD/Coins");
    }

    public void AddCoin(){
        _coins += 1;
        _coinsLabel.Text = "Coins: " + _coins;
    }

    public void AddPoint(){
        _score += 10;
        _scoreLabel.Text = "Score: " + _score;
        
    }

}
