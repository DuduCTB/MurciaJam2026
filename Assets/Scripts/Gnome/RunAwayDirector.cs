using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RunAwayDirector : MonoBehaviour
{
    [Header("StoryState")]

    public int storyState = 0;

    [Header("Needed references")]

    [SerializeField] private List<WayPoint> hidePoints = new List<WayPoint>();
    [SerializeField] private List<ObjectiveData> objectives = new List<ObjectiveData>();
    [SerializeField] private List<DetectionArea> detectionAreas = new List<DetectionArea>();

    [SerializeField] private WayPoint leverCampPoint;
    [SerializeField] private WayPoint steelPoint2;

    public GnomeAI gnomeAI;

    [SerializeField] private float arrivalDistance = 1.5f;   // Debe ser >= al Stopping Distance del NavMeshAgent
    [SerializeField] private float waitAtPoint = 0f;

    [Header("References")]

    [SerializeField] private Interactable lever;
    [SerializeField] private Interactable mineral;

    public static RunAwayDirector instance;

    private Coroutine randomChaseRoutine;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GoNextStateAndExecute(); // DEBUG!!!
        }
    }



    private void ExecuteCurrentEvent()
    {
        switch (storyState)
        {
            case -1: // el jugador alcanza el área que ejecuta que el Gnomo robe por primera vez el mineral, por lo que pasa el gnomo y lo roba. La UI le pide que vaya por él
                GameEvents.onObjectiveChanged?.Invoke(objectives[1].objectiveDesc);
                gnomeAI.SetDestination(hidePoints[1].transform);
                detectionAreas[1].gameObject.SetActive(true);
                lever.gameObject.layer = 0;
                mineral.gameObject.layer = 0;

                break;

            case 0: // Este punto se ejecuta cuando el jugador intenta pillar en el hueco (2) y se va justo. Por lo que sale corriendo hasta el hueco 3.
                GameEvents.onObjectiveChanged?.Invoke(objectives[2].objectiveDesc); // Persiguelo!
                gnomeAI.SetDestination(hidePoints[2].transform);
                detectionAreas[2].gameObject.SetActive(true);


                break;

            case 1: // Este punto se ejecuta cuando el jugador llega a cierta distancia del hueco 3. El Gnomo se esconde dentro del hueco y la UI te pide que desistas y recojas el mineral.
                GameEvents.onObjectiveChanged?.Invoke(objectives[3].objectiveDesc);
                mineral.gameObject.layer = 6;
                // Gnomo se esconde y desactiva
                //Mineral resaltado
                break;

            case 2: // Este punto se ejecuta cuando el jugador pilla el mineral. La UI le pide que busque el siguiente trozo.
                GameEvents.onObjectiveChanged?.Invoke(objectives[4].objectiveDesc);
                detectionAreas[3].gameObject.SetActive(true);
                break;

            case 3: // Este punto se ejecuta cuando el jugador se acerca lo suficiente al siguiente trozo, el nomo lo pilla y el juego le pide que lo persiga
                gnomeAI.TeleportGnome(steelPoint2.transform);
                gnomeAI.SetDestination(hidePoints[3].transform);
                //Cambiar objetivo a que le persiga
                detectionAreas[4].gameObject.SetActive(true);
                GameEvents.onObjectiveChanged?.Invoke(objectives[5].objectiveDesc);

                break;

            case 4: // Este punto se ejecuta cuando el jugador se acerca lo suficiente al siguiente al gnomo. Haciendo que huya hasta el hueco 2 de nuevo.
                gnomeAI.SetDestination(hidePoints[1].transform);
                //Cambiar objetivo a que vuelva a la sala de mandos. Pulsa la palanca para abrir la puerta
                detectionAreas[5].gameObject.SetActive(true);
                break;

            case 5: // Este punto se ejecuta cuando el jugador se acerca lo suficiente al siguiente al gnomo en el hueco 2, mirando hacia la nave. //El juego ya le pide que desista y que vaya a dejar el mineral, pulsando la palanca antes. La palanca se resalta con shader
                GameEvents.onObjectiveChanged?.Invoke(objectives[6].objectiveDesc);
                lever.gameObject.layer = 6;
                //La palanca debería brillar con shader

                break;

            case 6: // Este punto se ejecuta cuando la palanca es pulsada. Se abre la puerta y se crea un area de deteccion del jugador que pille justo cuando vaya a entrar
                detectionAreas[6].gameObject.SetActive(true);
                lever.gameObject.layer = 0;
                break;

            case 7: // Se ejecuta cuando el jugador entra en área. La puerta se cierra de repente y el gnomo spawnea al lado de la palanca.
                gnomeAI.ClearDestination();
                gnomeAI.TeleportGnome(leverCampPoint.transform);
                lever.targetAction.UndoAction();
                detectionAreas[7].gameObject.SetActive(true);
                break;

            case 8: // Se ejecuta cuando el jugador está mirando hacia el gnomo, este sale corriendo al hueco de delante
                detectionAreas[8].gameObject.SetActive(true);
                gnomeAI.SetDestination(hidePoints[0].transform);
                break;

            case 9: // Se ejecuta cuando el jugador llega casi al Gnomo, el juego le pide que desista y vuelva a la palanca
                //Gnomo se esconde
                // Palanca brilla
                lever.gameObject.layer = 6;
                break;

            case 10: //Se ejecuta cuando el jugador pulsa la palanca, hace aparecer un área de deteccion frente a la puerta
                detectionAreas[9].gameObject.SetActive(true);
                lever.gameObject.layer = 0;
                break;


            case 11: //Se ejecuta cuando el jugador entra en el área de la puerta, esta se cierra. 
                gnomeAI.ClearDestination();
                gnomeAI.TeleportGnome(leverCampPoint.transform);
                lever.targetAction.UndoAction();
                gnomeAI.ClearDestination();
                detectionAreas[10].gameObject.SetActive(true);
                break;

            case 12:
                StartRandomChase();
                gnomeAI.pickGnome.gameObject.SetActive(true);
                GameEvents.onObjectiveChanged?.Invoke(objectives[7].objectiveDesc);
                // Se ejecuta cuando el jugador está mirando hacia el gnomo, este sale corriendo y empieza la persecución aleatoria final. La UI te pide que vayas por el de una vez por todas, la rabia al máximo y
                //la UI también lo sugiere. El eneano empieza a ir de hueco a hueco corriendo de formaleatoria hasta que lo pilles

                break;

            case 13: //Alcanzas al enano y la UI te puide que vayas a la sala de mandos y lo mandes a bolar con el cohete resaltado.
                lever.gameObject.layer = 6;
                GameEvents.onObjectiveChanged?.Invoke(objectives[8].objectiveDesc);
                break;

        }
    }

    public void GoNextStateAndExecute()
    {
        ExecuteCurrentEvent();
        storyState++;
    }


    public void StartRandomChase()
    {
        StopRandomChase();
        randomChaseRoutine = StartCoroutine(RandomChaseRoutine());
    }

    public void StopRandomChase()
    {
        if (randomChaseRoutine == null) return;

        StopCoroutine(randomChaseRoutine);
        randomChaseRoutine = null;
    }

    private IEnumerator RandomChaseRoutine()
    {
        if (hidePoints.Count == 0) yield break;

        WayPoint current = null;

        while (true)
        {
            WayPoint next = GetRandomHidePoint(current);
            current = next;

            gnomeAI.SetDestination(next.transform);

            yield return new WaitUntil(() => HasArrived(next.transform));

            if (waitAtPoint > 0f)
            {
                yield return new WaitForSeconds(waitAtPoint);
            }
        }
    }

    private WayPoint GetRandomHidePoint(WayPoint exclude)
    {
        if (hidePoints.Count == 1) return hidePoints[0];

        WayPoint candidate;
        do
        {
            candidate = hidePoints[UnityEngine.Random.Range(0, hidePoints.Count)];
        }
        while (candidate == exclude);

        return candidate;
    }

    private bool HasArrived(Transform target)
    {
        Vector3 gnomePos = gnomeAI.transform.position;
        Vector3 targetPos = target.position;
        gnomePos.y = 0f;
        targetPos.y = 0f; // Ignora la altura para que no falle si el punto está elevado

        return (gnomePos - targetPos).sqrMagnitude <= arrivalDistance * arrivalDistance;
    }

}
