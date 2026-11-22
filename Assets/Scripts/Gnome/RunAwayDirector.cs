using UnityEngine;
using System;
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

    [SerializeField] private GnomeAI gnomeAI;

    [Header("References")]

    public static RunAwayDirector instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

 

    private void ExecuteCurrentEvent()
    {
        switch (storyState)
        {
            case 0: // el jugador alcanza el área que ejecuta que el Gnomo robe por primera vez el mineral, por lo que pasa el gnomo y lo roba. La UI le pide que vaya por él
                GameEvents.onObjectiveChanged?.Invoke(objectives[0].objectiveDesc);
                gnomeAI.SetDestination(hidePoints[1].transform);
                detectionAreas[0].gameObject.SetActive(true);

                break;

            case 1: // Este punto se ejecuta cuando el jugador intenta pillar en el hueco (2) y se va justo. Por lo que sale corriendo hasta el hueco 3.

                break;

            case 2: // Este punto se ejecuta cuando el jugador llega a cierta distancia del hueco 3. El Gnomo se esconde dentro del hueco y la UI te pide que desistas y recojas el mineral.

                break;

            case 3: // Este punto se ejecuta cuando el jugador pilla el mineral. La UI le pide que busque el siguiente trozo.

                break;

            case 4: // Este punto se ejecuta cuando el jugador se acerca lo suficiente al siguiente trozo, el nomo lo pilla y el juego le pide que lo persiga

                break;

            case 5: // Este punto se ejecuta cuando el jugador se acerca lo suficiente al siguiente al gnomo. Haciendo que huya hasta el hueco 2 de nuevo.

                break;

            case 6: // Este punto se ejecuta cuando el jugador se acerca lo suficiente al siguiente al gnomo en el hueco 2, mirando hacia la nave. 
                //El juego ya le pide que desista y que vaya a dejar el mineral, pulsando la palanca antes. La palanca se resalta con shader
                break;

            case 7: // Este punto se ejecuta cuando la palanca es pulsada. Se abre la puerta y se crea un area de deteccion del jugador que pille justo cuando vaya a entrar
                
                break;

            case 8: // Se ejecuta cuando el jugador entra en área. La puerta se cierra de repente y el gnomo spawnea al lado de la palanca.

                break;

            case 9: // Se ejecuta cuando el jugador está mirando hacia el gnomo, este sale corriendo al hueco de delante

                break;

            case 10: // Se ejecuta cuando el jugador llega casi al Gnomo, el juego le pide que desista y vuelva a la palanca

                break;

            case 11: //Se ejecuta cuando el jugador pulsa la palanca, hace aparecer un área de deteccion frente a la puerta

                break;


            case 12: //Se ejecuta cuando el jugador entra en el área de la puerta, esta se cierra. La UI te pide que vayas por el de una vez por todas, la rabia al máximo y
                     //la UI también lo sugiere. El eneano empieza a ir de hueco a hueco corriendo de formaleatoria hasta que lo pilles

                break;

            case 13: //Alcanzas al enano y la UI te puide que vayas a la sala de mandos y lo mandes a bolar con el cohete resaltado.

                break;

        }
    }

    public void GoNextStateAndExecute()
    {
        storyState++;
        ExecuteCurrentEvent();
    }

    public void instantiatePlayerDetectionArea()
    {

    }
   
}
