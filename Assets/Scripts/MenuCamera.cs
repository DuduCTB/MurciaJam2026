using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [Header("Órbita")]
    public Transform target;          // a qué mira al orbitar
    public float orbitSpeed = 2f;

    [Header("Plano de opciones")]
    public Transform optionsShot;     // el vacío PlanoOpciones
    public float moveTime = 1.2f;     // duración del viaje en segundos

    enum State { Orbit, ToOptions, InOptions, ToOrbit }
    State state = State.Orbit;

    Vector3 fromPos, orbitPos;
    Quaternion fromRot, orbitRot;
    float t;

    void Update()
    {
        switch (state)
        {
            case State.Orbit:
                if (!target) return;
                transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
                transform.LookAt(target);
                break;

            case State.ToOptions:
                Move(optionsShot.position, optionsShot.rotation, State.InOptions);
                break;

            case State.ToOrbit:
                Move(orbitPos, orbitRot, State.Orbit);
                break;
        }
    }

    void Move(Vector3 toPos, Quaternion toRot, State next)
    {
        t += Time.deltaTime / moveTime;
        float k = Mathf.SmoothStep(0f, 1f, t);   // arranca y frena suave
        transform.position = Vector3.Lerp(fromPos, toPos, k);
        transform.rotation = Quaternion.Slerp(fromRot, toRot, k);
        if (t >= 1f) state = next;
    }

    public void GoToOptions()
    {
        if (state == State.Orbit)
        {
            orbitPos = transform.position;   // recuerda dónde estaba para volver
            orbitRot = transform.rotation;
        }
        Begin(State.ToOptions);
    }

    public void BackToOrbit() => Begin(State.ToOrbit);

    void Begin(State s)
    {
        fromPos = transform.position;
        fromRot = transform.rotation;
        t = 0f;
        state = s;
    }
}