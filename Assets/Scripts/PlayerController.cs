using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Momentum Movement")]

    [Header("Ground Movement")]
    public float groundMaxSpeed = 6f;
    public float groundAcceleration = 45f;   // unidades/s² al acelerar
    public float groundDeceleration = 30f;   // unidades/s² al frenar (mayor = más grip)

    public Vector3 horizontalVelocity;
    public Vector3 playerVelocity;

    [Header("Inputs and movement")]
    public Transform orientation;
    public InputActionReference move;

    [Header("Needed Components")]

    [SerializeField] private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerWithMomentum();
    }

    public void MovePlayerWithMomentum()
    {
        if (characterController.enabled == false) return;

        Vector2 input = move.action.ReadValue<Vector2>();
        float maxSpeed = groundMaxSpeed;
        float accel = groundAcceleration;
        float decel = groundDeceleration;

        Vector3 inputDir = ReadInputVector();
        bool isMoving = input.sqrMagnitude > 0.01f;
        Vector3 targetVelocity = inputDir * (isMoving ? maxSpeed : 0f);

        // Elegir aceleración según si hay input o no
        float rate = isMoving ? accel : decel;

        // MoveTowards trabaja sobre el vector completo:
        // si vas a la derecha y pulsas izquierda, pasa por cero progresivamente
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, rate * Time.deltaTime);

        // mover de verdad
        Vector3 finalVelocity = horizontalVelocity + Vector3.up * playerVelocity.y;
        characterController.Move(finalVelocity * Time.deltaTime);
    }

    public Vector3 ReadInputVector()
    {

        Vector2 input = move.action.ReadValue<Vector2>();

        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * input.y + right * input.x;

        return Vector3.ClampMagnitude(moveDirection, 1f);
    }

    // relative input vector
    public Vector2 ReadRelativeInputVector()
    {
        Vector2 input = move.action.ReadValue<Vector2>();
        input = Vector2.ClampMagnitude(input, 1f);
        return input;
    }
}
