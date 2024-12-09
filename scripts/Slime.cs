using Godot;
using System;

public partial class Slime : CharacterBody2D{

    [Export] float move = 70;
    private float _gravity = 98;
    public int dead = 1;
    public bool hit = false;

    AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        Velocity = new Vector2(-move, 0);
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

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
        if (hit){
            if (dead > 0){
                SetPhysicsProcess(false);
                _animatedSprite.AnimationFinished += OnAnimationFinished; // Conectamos el evento
                _animatedSprite.Play("dead"); // Asegúrate de usar la animación correcta
            }
        }
    }

    private void OnAnimationFinished()
    {
        // Lógica después de que termine la animación
        GD.Print("Animación finalizada");
        QueueFree(); // Por ejemplo, elimina el objeto después de la animación
    }

    public void _on_area_2d_body_entered(Node2D body){
        if (body is Wizard wizard)
        {
            GD.Print("Wizard detectado. Llamando a Dead...");
            wizard.Dead();
        }
        else
        {
            GD.Print("El nodo detectado no es un Wizard. Es: " + body.Name);
        }
    }
}
