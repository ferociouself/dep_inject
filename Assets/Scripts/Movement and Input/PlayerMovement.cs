using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D rb;

	private Settings movementSettings;

	private InputHandler input;

	private ParticleSystem part;

	private bool jumpThisFrame = false;
	private bool holdingJump = false;

	private bool jumpCharged = true;

	private Vector2 lastPlayerVelocity = Vector2.zero;

	// Use this for initialization
	void Start () {
		
	}

	[Inject]
	void Init(Settings settings, [Inject(Id = "Player")]InputHandler inp, [Inject(Id = "Player")]Rigidbody2D playerRB, [Inject(Id = "Player")]ParticleSystem particles) {
		movementSettings = settings;
		input = inp;
		rb = playerRB;
		part = particles;
	}
	
	// Update is called once per frame
	void Update () {
		if (!input.GetActive ()) {
			return;
		}

		List<InputHandler.InputVals> curInputVals = input.GetCurrentInputVals ();

		float playerHorizVel = 0.0f;

		foreach (InputHandler.InputVals inputVal in curInputVals) {
			if (inputVal == InputHandler.InputVals.RunLeft) {
				playerHorizVel += -1 * movementSettings.runSpeed;
			}
			if (inputVal == InputHandler.InputVals.RunRight) {
				playerHorizVel += movementSettings.runSpeed;
			}
			if (inputVal == InputHandler.InputVals.WalkLeft) {
				playerHorizVel += -1 * movementSettings.walkSpeed;
			}
			if (inputVal == InputHandler.InputVals.WalkRight) {
				playerHorizVel += movementSettings.walkSpeed;
			}
			if (inputVal == InputHandler.InputVals.Jump) {
				if (!jumpThisFrame && jumpCharged) {
					rb.AddForce (new Vector2 (0.0f, movementSettings.jumpPower));
					jumpThisFrame = true;
					jumpCharged = false;
				} else {
					rb.AddForce (new Vector2 (0.0f, movementSettings.floatPower));
					holdingJump = true;
					if (jumpThisFrame) {
						jumpThisFrame = false;
					}
				}
			} else if (holdingJump) {
				holdingJump = false;
			}
		}

		if (input.GetCurrentModifier () == InputHandler.ModifierVals.FastDescent && !jumpCharged) {
			rb.AddForce (new Vector2 (0.0f, -1 * movementSettings.slamPower * input.GetCurrentModifierValue()));
		}

		rb.velocity = new Vector2(playerHorizVel, rb.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.CompareTag ("Ground")) {
			CollidedWithGround (coll);
		}
	}

	void CollidedWithGround(Collision2D coll) {
		jumpCharged = true;

		if (input.GetCurrentModifier () == InputHandler.ModifierVals.FastDescent) {
			float magnitudeOfHit = coll.relativeVelocity.magnitude;

			if (magnitudeOfHit > movementSettings.minMagnitudeSlam) {
				part.Emit ((int)Mathf.Floor (magnitudeOfHit));
			}
		}
	}

	[System.Serializable]
	public class Settings {

		public float runSpeed;
		public float walkSpeed;

		public float jumpPower;
		public float floatPower;

		public float slamPower;

		public float minMagnitudeSlam;
	}
}
