using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    private float yRotation = 45f;

    [Header( "Mouse look" )]
	public float mouseSensitivity = 1;
	public float turretYawSensitivity = 6;
    float pitchDeg;
	float yawDeg;

	float rotationAcc;
	[SerializeField] private float rotationSpeed = 10f;

    [Header( "Movement" )]
	public float playerAccMagnitude = 400; // meters per second^2
	public float drag = 1.6f;

	// internal state
	Vector3 vel, acc;

    void Awake() {
		Vector3 startEuler = transform.eulerAngles;
		pitchDeg = startEuler.x;
		yawDeg = startEuler.y;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
	}

    void Update() {
		if( Input.GetKeyDown( KeyCode.Escape ) ) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		UpdateMovement();
		UpdateMouseLook();
		// UpdateRotation();
	}

	void UpdateRotation()
	{
		if (Input.GetKey(KeyCode.Q)) {
			yRotation += rotationSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.E)) {
			yRotation -= rotationSpeed * Time.deltaTime;
		}
		gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
	}

	void UpdateMovement() {
		// local function
		void TestInput( KeyCode key, Vector3 dir ) {
			if( Input.GetKey( key ) )
				acc += dir;
		}

		acc = Vector3.zero;
		TestInput( KeyCode.W, transform.forward );
		TestInput( KeyCode.S, -transform.forward );
		TestInput( KeyCode.A, -transform.right );
		TestInput( KeyCode.D, transform.right );
		TestInput( KeyCode.Space, Vector3.up );
		TestInput( KeyCode.LeftControl, Vector3.down );

		if( acc != Vector3.zero )
			acc = acc.normalized * playerAccMagnitude;
		vel += acc * Time.deltaTime;

		// 1 meter per second
		float dt = Time.deltaTime;
		transform.position += vel * dt;
	}

	void FixedUpdate() {
		vel /= drag; // movement dampening
	}

    void UpdateMouseLook() {
        if (Input.GetMouseButton(1))
        {
            float xDelta = Input.GetAxis( "Mouse X" );
            float yDelta = Input.GetAxis( "Mouse Y" );
            pitchDeg += -yDelta * mouseSensitivity;
            pitchDeg = Mathf.Clamp( pitchDeg, -90, 90 );
            yawDeg += xDelta * mouseSensitivity;
            mainCamera.rotation = Quaternion.Euler( pitchDeg, yawDeg, 0 );
        }
	}
}
