using UnityEngine;
using System.Collections;
public class LightshowCharacterController : MonoBehaviour {
	public float movementSpeed = 2f;
	private Rigidbody2D rb;
	private bool facingRight = true;

	bool grounded = false;
	public Transform groundCheckPos;
	public float groundCheckRadius = 0.01f;
	public LayerMask layersThatAreGround;
	public int numJumps = 1; // num jumps is for setting more jumps. 2 = double jump; 1 = normal jump
	// Be sure to change public vars in the inspector and not in here.
	private int jumps;
	public float jumpVelocity = 7f;

	public float blinkDistance = 1f;
	public float blinkStartPause = 0.1f;
	public float blinkEndPause = 0.1f;
	public float blinkDashTime = 0.05f;
	private float curBlinkTime = 0f;
	private bool dashing = false;
	public float blinkCooldown = 3f;
	private float blinkCoolTime = 0;
	private bool blinkReady = true;
	private Vector2 blinkStartPos;
	private Vector2 blinkEndPos;
	//public GameObject blinkCooldownUIProgress;
	public GameObject blinkParticlePrefab;

	public float mainAttackAnimTime = 0.4f;
	public float chargeTime = 2f;
	private float attacking = 0f;
	private bool charging = false;
	private float curChargeTime = 0f;
	public GameObject laserPrefab;
	public GameObject laserChargePrefab;

	private bool jumpInput;
	private float horizontalInput;
	Animator anim;

	//public GameObject chargeMeter;

	private BoxCollider2D myCollider;
	private CircleCollider2D myCircCollider;


	void Start () {
		rb = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<BoxCollider2D> ();
		myCircCollider = GetComponent<CircleCollider2D> ();
		jumps = numJumps;
		anim = GetComponent<Animator> ();
	}
	void Update()
	{
		//GetButton is much more modular than using keycodes because then the button can be easily changed in the settings, 
		// and controllers work nativly with the default controlls setup by unity.
		//Check input manager for changing keybinds
		if (!dashing) {
			jumpInput = Input.GetButtonDown ("Jump"); // we are doing the jump inputs in Update because it is only true for 1 pass
		} else {
			jumpInput = false;
		}
		// If we used FixedUpdate it would only work sometimes when the frame aligns with the button press.
		// Anytime you use GetButton update is required
		Vector2 vel = rb.velocity;//Grab the velocity
		if (jumpInput) {
			if (jumps > 0) {
				vel.y = jumpVelocity;
				jumps--;
			}
		}

		if (grounded && vel.y<=0) { // If we are grounded and we aren't leaving the ground ie y velocity is not positive
			jumps = numJumps;

		}
		if (!grounded && jumps > 0) { 
			// This makes it so that if we fall off an edge and we have a jump left over, then we will have one air jump
			// without this you would have 2 jumps after falling off.
			jumps = 1;
		}
		rb.velocity = vel;//Set our changes

		anim.SetFloat ("VSpeed", vel.y);

		bool mainAttackInput = Input.GetButtonDown ("Fire1");
		bool secondAttackInput = Input.GetButton ("Fire2");

		if (mainAttackInput && attacking <= 0 && !charging) {
			attacking = mainAttackAnimTime; //Attacking is a float just to generalize the two attacks. it's set to the anim time and goes down to 0;
			float ang = 0;
			if (Mathf.Sign (transform.localScale.x) == -1) {
				ang = 180;
			}

			Instantiate(laserPrefab,transform.position,Quaternion.Euler(0,ang,0));
		}
		if (secondAttackInput) {
			curChargeTime += Time.deltaTime;
			charging = true;
			//chargeMeter.SendMessage ("SetValue",Mathf.Min((curChargeTime / chargeTime),1));
		} else {
			if (curChargeTime > chargeTime) {
				//shoot 
				float ang = 0;
				if (Mathf.Sign (transform.localScale.x) == -1) {
					ang = 180;
				}
					
				Instantiate(laserChargePrefab,transform.position,Quaternion.Euler(0,ang,0));
			}
			curChargeTime = 0f;
			charging = false;
		}
//		chargeMeter.SendMessage ("SetActive",charging);
		//blink code
		bool blinkInput = Input.GetButtonDown ("Fire3"); // Fire3 defaults to shift
		if (blinkReady && blinkInput && !dashing && attacking <= 0) {
			dashing = true;
			//Do blink 
			//LayersThatAreGround is used to ignore the player in the raycast, otherwise the player would be in the way of the ray

			//Spawn particles at the start
			Vector2 startPos = transform.position;
			Vector2 endPos;
			//Change radius in 2nd parameter if the raycast isn't working. Probably too large and it's hiting the floor
			RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, transform.right*Mathf.Sign(transform.localScale.x),blinkDistance,layersThatAreGround); // Mathf.Sign gets if it is negative, if so we need to go backwards
			if (hit.collider != null) {
				float dist = Mathf.Abs (hit.point.x - transform.position.x);
				if (dist > blinkDistance) { // If there is a wall in that direction, but it's not going to put us inside it.
					//transform.position = transform.position + transform.right * blinkDistance * Mathf.Sign (transform.localScale.x);
					endPos = transform.position + transform.right * blinkDistance * Mathf.Sign (transform.localScale.x);
					blinkCoolTime = blinkCooldown; //Start the cooldown timer
					blinkReady = false;
				} else { // IF we would be in the wall
					//transform.position = hit.point - new Vector2 (myCollider.size.x / 2, 0) * Mathf.Sign (transform.localScale.x);
					endPos = hit.point - new Vector2 (myCircCollider.radius, 0) * Mathf.Sign (transform.localScale.x);
					blinkCoolTime = blinkCooldown; //Start the cooldown timer
					blinkReady = false;
				}
				
			} else { // IF there is no wall in that direction
				//transform.position = transform.position + transform.right * blinkDistance * Mathf.Sign (transform.localScale.x);
				endPos = transform.position + transform.right * blinkDistance * Mathf.Sign (transform.localScale.x);
				blinkCoolTime = blinkCooldown; //Start the cooldown timer
				blinkReady = false;
			}
			//Vector2 endPos = transform.position;
			Object startParticles = Instantiate(blinkParticlePrefab,startPos,transform.rotation); // Spawn particles
			Destroy (startParticles, 0.5f);//Destroy it after 0.5 seconds

			blinkStartPos = startPos;
			blinkEndPos = endPos;
			curBlinkTime = 0;
		}
		if (dashing) {
			rb.isKinematic = true;
			if (curBlinkTime < blinkStartPause) {
				//Just wait cause nothing needs to happen
				//Probably play an anim here
			} else if (curBlinkTime >= blinkStartPause && curBlinkTime < blinkStartPause + blinkDashTime) {
				//Move between start and end.
				float dif = blinkEndPos.x - blinkStartPos.x;
				float lerpX = Mathf.Lerp(0, dif, (curBlinkTime-blinkStartPause)/(blinkDashTime)); // (curBlinkTime-blinkStartPause)/(blinkDashTime) makes the numerator start at 0 once this hits this block
				Debug.DrawLine (blinkStartPos,blinkEndPos);
				transform.position = blinkStartPos + new Vector2 (lerpX,0);
			} else if (curBlinkTime >= blinkStartPause + blinkDashTime) {
				//wait again
			}
			curBlinkTime += Time.deltaTime;
			if(curBlinkTime > blinkStartPause + blinkDashTime + blinkEndPause) {
				dashing = false;
				curBlinkTime = 0;
				Object endParticles = Instantiate(blinkParticlePrefab,blinkEndPos,transform.rotation); // Spawn particles
				Destroy (endParticles, 0.5f);//Destroy it after 0.5 seconds
				transform.position = blinkEndPos;//Make sure we actually got there.
			}
		}
		if (!dashing) {
			rb.isKinematic = false;
		}
		attacking -= Time.deltaTime;
		if (attacking < 0) {
			attacking = 0;
		}

	}
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheckPos.position, groundCheckRadius, layersThatAreGround);
		anim.SetBool ("Ground", grounded);

		if (!dashing && !charging) {
			horizontalInput = Input.GetAxis ("Horizontal");
			anim.SetFloat ("Speed", Mathf.Abs (horizontalInput));//Send info to animator for it to animate everything
		} else {
			horizontalInput = 0;
			anim.SetFloat ("Speed", Mathf.Abs (horizontalInput));
		}
		//GetAxis works for keyboard and for controller so it's very useful to just implement it this way rather than testing for keys
		//When you press an arrow key there is an acceleration to the GetAxis value, meaning that it starts at 0 and quickly goes to 1
		// so if you want no acceleration then go into the input settings and change the accel

		Vector2 vel = rb.velocity;//Grab the velocity
		vel.x = horizontalInput * movementSpeed;
		rb.velocity = vel; // Set the velocity on the rigidbody after we have changed it.




		if (horizontalInput > 0 && !facingRight) { // if the player is trying to/is moving right
			flip();
		}
		else if (horizontalInput < 0 && facingRight) { // if the player is trying to/is moving right
			flip();
		}




		//blink code
		if (blinkCoolTime > 0) {
			blinkCoolTime -= Time.deltaTime;
			}
		if (blinkCoolTime <= 0) {
			blinkCoolTime = 0;
			blinkReady = true;
		}
		//blinkCooldownUIProgress.SendMessage ("SetValue",1-(blinkCoolTime / blinkCooldown));
			
	}
	void flip()
	{
		Vector3 scale = transform.localScale;
		scale.x = (scale.x*-1);// "*-1" instead of just -1 to support scaling if we need to do so
		transform.localScale = scale;
		facingRight = !facingRight;//Flip the bool too so it represents the actual facing.
		//SendMessage("Damage",1,gameObject)
	}
	void Damage(float dmg)
	{
		if (!dashing) {
			Debug.Log ("Player damaged for " + dmg);
		}
	}
}
