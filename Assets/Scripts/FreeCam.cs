using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float movementSpeed = 100f;

    public float freeLookSensitivity = 3f;

    public float zoomSensitivity = 50f;

    private bool looking = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;   
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(transform.forward.x, 0f, transform.forward.z) * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(transform.forward.x, 0f, transform.forward.z) * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.up * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= Vector3.up * movementSpeed * Time.deltaTime;
        }

        if (looking)
        {
            var newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            var newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            transform.position += transform.forward * axis * zoomSensitivity;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }
    }

    void OnDisable()
    {
        StopLooking();
    }

    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
