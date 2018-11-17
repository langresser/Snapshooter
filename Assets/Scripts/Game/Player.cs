﻿using System.Collections.Generic;
using UnityEngine;

	/**
	* El jugador local.
	*/

public class Player : MonoBehaviour {

	// Comandos de movimiento:
	private const string KEY_W = "w";
	private const string KEY_A = "a";
	private const string KEY_S = "s";
	private const string KEY_D = "d";
	private const string KEY_Q = "q";
	private const string KEY_E = "e";

	protected Configuration config;
	protected Snapshot snapshot;
	protected Client client;
	protected int id;
	protected List<Direction> directions;

	/**
	* Obtiene la configuración global.
	*/
	protected void Awake() {
		config = GameObject
			.Find("Configuration")
			.GetComponent<Configuration>();
	}

	/**
	* Inicializa las variables de instancia.
	*/
	protected void Start() {
		directions = new List<Direction>(4);
	}

	/**
	* Envía un comando de movimiento al servidor.
	*/
	protected void Move() {
		directions.Clear();
		if (Input.GetKey(KEY_W)) {
			directions.Add(Direction.FORWARD);
		}
		if (Input.GetKey(KEY_A)) {
			directions.Add(Direction.STRAFING_LEFT);
		}
		if (Input.GetKey(KEY_S)) {
			directions.Add(Direction.BACKWARD);
		}
		if (Input.GetKey(KEY_D)) {
			directions.Add(Direction.STRAFING_RIGHT);
		}
		if (Input.GetAxis("Mouse X")<0) {
			directions.Add(Direction.ROTATE_LEFT);
		}
		if (Input.GetAxis("Mouse X")>0) {
			directions.Add(Direction.ROTATE_RIGHT);
		}

		client.Move(directions);
	}

	/**
	* Actualiza el estado del jugador. El estado se actualiza automáticamente
	* al activar predicción, o desde la snapshot global, en caso contrario.
	*/
	protected void Update() {
		// Actualizar vida en HUD usando:
		// snapshot.lifes[id];
		Move();
		if (!config.usePrediction) {
			transform.SetPositionAndRotation(snapshot.positions[id], snapshot.rotations[id]);
		}
	}

	/**
	* Actualiza la posición de la cámara. Debe ser en 1ra persona.
	*/
	protected void LateUpdate() {
		//Camera.main.transform.LookAt(transform);
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation;
	}

	/** **********************************************************************
	******************************* PUBLIC API ********************************
	 *********************************************************************** */

	public int GetID() {
		return id;
	}

	public Player SetClient(Client client) {
		this.client = client;
		return this;
	}

	public Player SetID(int id) {
		this.id = id;
		return this;
	}

	public Player SetSnapshot(Snapshot snapshot) {
		this.snapshot = snapshot;
		return this;
	}
}
