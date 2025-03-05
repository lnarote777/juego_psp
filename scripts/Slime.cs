using Godot;
using System;

public partial class Slime : CharacterBody2D{

    [Export] public float MaxHealth = 3f;
    [Export] float move = 70;

    private float _gravity = 98;
    public bool hit = false;
    private float _currentHealth;
    private bool _playerInattackRange = false;
	private bool _playerAttackCooldown = true;
	private Timer _attackCooldown ;


    AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        Velocity = new Vector2(-move, 0);
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _currentHealth = MaxHealth;
        AddToGroup("enemy");

        _attackCooldown = new Timer();
		_attackCooldown.WaitTime = 1.0f;  
		_attackCooldown.OneShot = true;
		AddChild(_attackCooldown);       
		_attackCooldown.Timeout += _on_attack_cooldown_timeout;

    }

    public override void _PhysicsProcess(double delta){
        Vector2 velocity = Velocity;

        velocity.Y += _gravity * (float)delta;

        if (IsOnWall())
        {
            if (!_animatedSprite.FlipH){
                velocity.X = move;
            } else{
                velocity.X = -move;
            }
        }

        // Controlamos la dirección del sprite
        if (velocity.X < 0)
        {
            _animatedSprite.FlipH = false; // Mirando a la izquierda
        }
        else if (velocity.X > 0)
        {
            _animatedSprite.FlipH = true; // Mirando a la derecha
        }

        Velocity = velocity;

        MoveAndSlide();
    }

    private void Dead(){
        
        SetPhysicsProcess(false);
        _animatedSprite.Play("dead");
        _animatedSprite.AnimationFinished += OnAnimationFinished;
        
    }

    private void OnAnimationFinished()
    {
        GD.Print("Animación finalizada");
        QueueFree();
    }

    public void TakeDamge(float damage){

        _currentHealth -= damage;
        GD.Print("Slime health: " + _currentHealth);
        if (_currentHealth <= 0){
            Dead();
        }
    }

    public void _on_enemy_hitbox_body_entered(Node2D body){
        if (body.IsInGroup("player")){
            _playerInattackRange = true;
        }
        
    }

    public void _on_enemy_hitbox_body_exited(Node2D body){
        if (body.IsInGroup("player")){
            _playerInattackRange = false;
        }
    }

    public void _on_attack_cooldown_timeout(){

    }

}
