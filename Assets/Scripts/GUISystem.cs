using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GUISystem : MonoBehaviour
{
		private GUIState currentState;

		// Use this for initialization
		void Start ()
		{
				currentState = new MenuState (ChangeGUIState);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnGUI ()
		{
				currentState.OnGUI ();
		}

		void ChangeGUIState (GUIState newState)
		{
				currentState = newState;
		}
}

interface GUIState
{
		void OnGUI ();
}

class MenuState : GUIState
{
		private Action<GUIState> changeState;
		public MenuState (Action<GUIState> change)
		{
				changeState = change;
		}
		public void OnGUI ()
		{
				GUILayout.BeginArea (new Rect (Camera.main.pixelWidth / 4, Camera.main.pixelHeight / 2, Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
				if (GUILayout.Button ("Start")) {
				}
				if (GUILayout.Button ("Instructions")) {
						changeState (new InstructionState (changeState));
				}
				GUILayout.EndArea ();
		}
}

class InstructionState : GUIState
{
		private Action<GUIState> changeState;
		public InstructionState (Action<GUIState> change)
		{
				changeState = change;
		}
		public void OnGUI ()
		{
				GUILayout.BeginArea (new Rect (Camera.main.pixelWidth / 4, Camera.main.pixelHeight / 2, Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
				if (GUILayout.Button ("Back")) {
						changeState (new MenuState (changeState));
				}
				GUILayout.EndArea ();
		}
}