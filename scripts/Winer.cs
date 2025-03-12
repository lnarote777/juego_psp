using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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

		//POST
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

        popupNombre.Visible = false;
		
	}

	public void _on_get_players_pressed(){
		Error error = httpRequest.Request(url);
		if (error != Error.Ok)
		{
			GD.Print("Error en la solicitud GET: " + error);
		}
	}

	public void _on_http_request_request_completed(long result, long responseCode, string[] headers, byte[] body){

		string responseText = Encoding.UTF8.GetString(body);

		if (responseCode == 201) 
        {
            GD.Print("Jugador guardado exitosamente.");
        }
        else if (responseCode == 200){
		
			string[] lista = responseText.Split(new string[] { "}," }, StringSplitOptions.RemoveEmptyEntries);
			
			if (lista == null){
				GD.Print("Aun no hay jugadores...");
			}

			GD.Print("\n===== Lista de Jugadores =====\n");

			foreach (var jugador in lista){
				string id = ExtraerValor(jugador, "\"id\":");
				string nombre = ExtraerValor(jugador, "\"name\":");
				string coins = ExtraerValor(jugador, "\"coins\":");
				string points = ExtraerValor(jugador, "\"points\":");
				string connection = ExtraerValor(jugador, "\"ultimaConexion\":");

				GD.Print($"ID: {id}");
				GD.Print($"	Nombre: {nombre}");
				GD.Print($"	Monedas: {coins}");
				GD.Print($"	Puntos: {points}");
				GD.Print($"	Última conexión: {connection}");
				GD.Print("--------------------------------");
			};
		}
		else
        {
            GD.Print("Error - Código de respuesta: " + responseCode);
        }
	}

	string ExtraerValor(string texto, string clave)
	{
		int inicio = texto.IndexOf(clave);
		if (inicio == -1) return "";  

		inicio += clave.Length;  
		int fin = texto.IndexOf(",", inicio);  
		if (fin == -1) fin = texto.IndexOf("}", inicio);  

		if (fin == -1) return texto.Substring(inicio).Trim().Trim('"');  
		return texto.Substring(inicio, fin - inicio).Trim().Trim('"');  
	}


}
