using System;
using Godot;

public partial class Wizard : CharacterBody2D
{
	[Export] public float Speed = 200f;
	[Export] public float JumpVelocity = 700.0f;
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
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Attack"))  // Evita interrumpir la animación si ya está en curso
        {
            Attack(); // Llama a la función de ataque
        }
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

			if (Input.IsActionJustPressed("Jump")){
						
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
			GetTree().ChangeSceneToFile("res://scenes/gameover.tscn");
		}

	}

	
	public void Dead(){
		GD.Print("Reproduciendo animación 'dead'.");
		_animatedSprite.Play("dead");
	}
	
}
