﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinCounter : MonoBehaviour {

	public Text standingDisplay;

	private GameManager gameManager;
	private bool ballOutOfPlay = false;
	private int lastStandingCount = -1;
	private int lastSettledCount = 10;
	private float lastChangeTime;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		standingDisplay.text = CountStanding().ToString();

		if(ballOutOfPlay){
			UpdateStandingCountAndSettle();
			standingDisplay.color = Color.blue;
		}
	}

	public void Reset(){
		lastSettledCount = 10;
	}

	void OnTriggerExit(Collider collider){
		if(collider.gameObject.name == "Ball"){
			ballOutOfPlay = true;
		}
	}

	void UpdateStandingCountAndSettle(){
		// update the lastStandingCount

		int currentStanding = CountStanding();


		if(currentStanding != lastStandingCount){
			lastChangeTime = Time.time;
			lastStandingCount = currentStanding;
			return;
		}
	

		float settleTime = 3f;	// how long to wait to consider pins have settled


		if(Time.time - lastChangeTime > settleTime){
			PinsHaveSettled();
		}
	}

	void PinsHaveSettled(){
		int currentStanding = CountStanding();
		int pinFall = lastSettledCount - currentStanding;
		lastSettledCount = currentStanding;

		gameManager.Bowl(pinFall);

		lastStandingCount = -1;		// pins have settled and ball not back in box
		ballOutOfPlay = false;
		standingDisplay.color = Color.green;
	}

	int CountStanding(){
		int standing = 0;
		foreach(Pin pin in GameObject.FindObjectsOfType<Pin>()){
			if(pin.IsStanding()){
				standing++;
			}
		}
		return standing;
	}
}
