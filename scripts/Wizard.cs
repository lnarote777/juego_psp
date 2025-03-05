using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class Wizard : CharacterBody2D
{
	[Export] public float Speed = 200f;
	[Export] public float JumpVelocity = 800.0f;
	[Export] public float DashSpeed = 500f;
    [Export] public float DashDuration = 0.4f;
	[Export] public float MaxHealth = 10f;
	[Export] public bool attack = false;

	private float _currentHealth;
	private float _acceleration = .25f;
	private int _gravity = 3000;
	private bool _isDashing = false;
    private float _dashTimer = 0f;
    private int _lastDirection = 1;
	private Vector2 _initialPosition;
	private bool _enemyInattackRange = false;
	private bool _enemyAttackCooldown = true;
	private Timer _attackCooldown ;
	


	AnimatedSprite2D _animatedSprite;
	CollisionShape2D _collision;
	Area2D _area2D;

    public override void _Ready()
    {
        
        // Inicializa el AnimatedSprite2D obteniéndolo del nodo
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collision = GetNode<CollisionShape2D>("CollisionShape2D");
		_area2D = GetNode<Area2D>("SwordHit");
		_currentHealth = MaxHealth;
		_initialPosition = Position;
		AddToGroup("player");

		_attackCooldown = new Timer();
		_attackCooldown.WaitTime = 1.0f;  
		_attackCooldown.OneShot = true;
		AddChild(_attackCooldown);       
		_attackCooldown.Timeout += _on_attack_cooldown_timeout;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Attack"))  // Evita interrumpir la animación si ya está en curso
        {
            Attack(); // Llama a la función de ataque
        }
		TakeDamage();
    }

    public override void _PhysicsProcess(double delta)
	{
	
		Vector2 velocity = Velocity;
		Vector2 areaPosition = _area2D.Position ;
		int direction = 0;

		if (!_isDashing){
			velocity.Y += (float)(_gravity * delta);
		}
		
		if (_isDashing){
            _dashTimer -= (float)delta;
            if (_dashTimer <= 0)
            {
                _isDashing = false;
                _animatedSprite.Play("idle"); // Volver a la animación idle
            }
            else
            {
                velocity.X = _lastDirection * DashSpeed; // Mantener la velocidad del dash
                Velocity = velocity;
                MoveAndSlide();
                return; // Salir para que no interfieran otras lógicas
            }
        }

		if (!attack){
			if (Input.IsActionPressed("move_left")){

				_animatedSprite.Play("walk");
				direction -= 1;
				_lastDirection = -1;
				areaPosition.X = 0;
				_area2D.Position = areaPosition;
				_animatedSprite.FlipH = true;

			}else if (Input.IsActionPressed("move_right")){

				_animatedSprite.Play("walk");
				direction += 1;
				_lastDirection = 1;
				areaPosition.X = 24;
				_area2D.Position = areaPosition;
				_animatedSprite.FlipH = false;
				

			}else{

				_animatedSprite.Play("idle");

			}

			if (direction != 0){

				velocity.X = (direction * Speed);

			}else{
				
				velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			}

			if (Input.IsActionJustPressed("Jump") && IsOnFloor()){
						
				velocity.Y = -JumpVelocity;
				_animatedSprite.Play("jump");
					
			}

			if (Input.IsActionJustPressed("dash") && !_isDashing){

				_isDashing = true;
				_dashTimer = DashDuration;
				velocity.X = _lastDirection * DashSpeed;
				_animatedSprite.Play("dash");

			}
		}

        Velocity = velocity;
        MoveAndSlide();
    }

    private async void Attack(){
		attack = true;
        _animatedSprite.Play("attack");
        await ToSignal(_animatedSprite, "animation_finished");
		attack = false;
	}

	public void _on_sword_hit_body_entered(Node2D body){
		if (body.IsInGroup("ene")){
			GD.Print("Slime golpeado");
			body.Call("TakeDamage", 1);
		}

	}

	
	public async void Dead(){
		GD.Print("Reproduciendo animación 'dead'.");
		_animatedSprite.Play("dead");
		SetPhysicsProcess(false);
		await ToSignal(_animatedSprite, "animation_finished");
		CallDeferred(nameof(ResetPosition));
	}

	private void ResetPosition(){
		Position = _initialPosition;
		SetPhysicsProcess(true);
		_currentHealth = MaxHealth;
		_animatedSprite.Play("idle");
	}

	public void _on_wizar_hitbox_body_entered(Node2D body){
		if (body.IsInGroup("enemy")){
			_enemyInattackRange = true;
		}
	}
	
	public void _on_wizar_hitbox_body_exited(Node2D body){
		if (body.IsInGroup("enemy")){
			_enemyInattackRange = false;
		}
	}

	public void TakeDamage(){
		if (_enemyInattackRange && _enemyAttackCooldown == true){
			_currentHealth -= 1;
			_enemyAttackCooldown = false;
			_attackCooldown.Start();
			GD.Print("Wizard health: " + _currentHealth);
		}

		if (_currentHealth <= 0){
				Dead();
		}

	}


	public void _on_attack_cooldown_timeout(){
		_enemyAttackCooldown = true;

	}
}
