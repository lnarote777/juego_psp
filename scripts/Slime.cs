using Godot;
using System;

public partial class Slime : CharacterBody2D{

    [Export] public float MaxHealth = 15f;
    [Export] float move = 70;

    private float _gravity = 98;
    private float _currentHealth;
    private bool _playerInattackRange = false;
	private bool _playerAttackCooldown = true;
    private bool _canTakeDamage = true;
	
    private Timer _attackCooldown ;
    private Timer _damageCooldown ;
    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        Velocity = new Vector2(-move, 0);
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _currentHealth = MaxHealth;
        AddToGroup("enemy");

        _attackCooldown = new Timer();
		_attackCooldown.WaitTime = 0.5f;  
		_attackCooldown.OneShot = true;
		AddChild(_attackCooldown);       
		_attackCooldown.Timeout += _on_attack_cooldown_timeout;

        _damageCooldown = new Timer();
		_damageCooldown.WaitTime = 0.5f;  
		_damageCooldown.OneShot = true;
		AddChild(_damageCooldown);       
		_damageCooldown.Timeout += _on_damage_cooldown_timeout;

    }


    public override void _PhysicsProcess(double delta){
        TakeDamage();

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
        QueueFree();
    }

    public void TakeDamage(){

        if (_playerInattackRange && Global.playerCurrentAttack == true){
            if (_canTakeDamage){
                _currentHealth -= 5;
                _damageCooldown.Start();
                _canTakeDamage = false;
                GD.Print("Slime health: " + _currentHealth);

                if (_currentHealth <= 0){
				    Dead();
		        }
            }
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
        _playerAttackCooldown = true;
    }

    public void _on_damage_cooldown_timeout(){
        _canTakeDamage = true;
    }

}
