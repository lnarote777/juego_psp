using Godot;
using Godot.NativeInterop;
using System;
using System.Text;

public partial class Winer : Control
{

	private string url = "https://apijuego-7l37.onrender.com/api/Jugador";
	private Control popupNombre;
	private TextEdit nombreTextEdit;
	private HttpRequest httpRequest;

    public override void _Ready()
    {
		nombreTextEdit = GetNode<TextEdit>("savePlayer/TextEdit");

        httpRequest = GetNode<HttpRequest>("HTTPRequest");

		popupNombre = GetNode<Control>("savePlayer");
		
		popupNombre.Visible = false;
    }

    public void _on_menu_pressed(){
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}

	public void _on_exit_pressed(){
		GetTree().Quit();
	}

	public void _on_save_pressed(){
		popupNombre.Visible = true;
	}

	public void _on_cancel_pressed(){
		popupNombre.Visible = false;
	}

	public void _on_ok_pressed(){
		string nombreJugador = nombreTextEdit.Text;

		var jugador = new JugadorData{
			Name = nombreJugador,
			Coins = GameManager.Coins,
			Points = GameManager.Score
		};

		var jugadorDict = new Godot.Collections.Dictionary
		{
			{ "Name", jugador.Name },
			{ "Coins", jugador.Coins },
			{ "Points", jugador.Points }
		};

		string jsonData = Json.Stringify(jugadorDict);
		GD.Print("JSON enviado: " + jsonData);

		string[] headers = new string[] { "Content-Type: application/json" };
        httpRequest.Request(url, headers, HttpClient.Method.Post, jsonData);

        // Oculta el Control personalizado después de guardar
        popupNombre.Visible = false;
	}

	public void _on_http_request_request_completed(long result, long responseCode, string[] headers, byte[] body){
		if (responseCode == 201) 
        {
            GD.Print("Jugador guardado exitosamente.");
        }
        else
        {
            GD.Print("Error al guardar el jugador. Código de respuesta: " + responseCode);
        }
	}

}
