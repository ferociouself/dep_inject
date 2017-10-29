using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputHandler : InputHandler {

	private Settings settings;

	[Inject]
	public void Init(Settings set) {
		settings = set;
	}
	
	// Update is called once per frame
	public override void Tick () {
		base.Tick ();
		float horiz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");
		float horiz_abs = Mathf.Abs (horiz);
		float vert_abs = Mathf.Abs (vert);
		if (horiz_abs > settings.deadzone) {
			if (horiz_abs > settings.runzone) { /* in the run zone */
				curInputVals.Add((horiz < 0) ? InputVals.RunLeft : InputVals.RunRight);
			} else { /* in the walk zone */
				curInputVals.Add((horiz < 0) ? InputVals.WalkLeft : InputVals.WalkRight);
			}
		}

		if (vert_abs > settings.deadzone) {
			curModifier = (vert < 0) ? ModifierVals.FastDescent : ModifierVals.SlowDescent;
			modifierValue = vert_abs;
		}

		if (Input.GetButton (settings.jumpName)) {
			curInputVals.Add (InputVals.Jump);
		}
	}

	[System.Serializable]
	public class Settings {
		public float deadzone;
		public float runzone;

		public string jumpName;
	}
}
