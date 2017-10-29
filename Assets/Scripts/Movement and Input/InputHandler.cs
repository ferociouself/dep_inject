using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputHandler : ITickable, IInitializable {

	public enum InputVals {
		WalkRight,
		WalkLeft,
		RunRight,
		RunLeft,
		Jump
	}

	public enum ModifierVals {
		SlowDescent,
		FastDescent,
		None
	}

	protected List<InputVals> curInputVals;
	protected ModifierVals curModifier;
	protected float modifierValue;

	protected bool active;
	
	// Update is called once per frame
	public virtual void Tick () {
		curInputVals.Clear ();
		curModifier = ModifierVals.None;
	}

	public virtual void Initialize() {
		curInputVals = new List<InputVals> ();
		active = true;
	}

	public List<InputVals> GetCurrentInputVals() {
		return curInputVals;
	}

	public ModifierVals GetCurrentModifier() {
		return curModifier;
	}

	public float GetCurrentModifierValue() {
		return modifierValue;
	}

	public bool GetActive() {
		return active;
	}
}
