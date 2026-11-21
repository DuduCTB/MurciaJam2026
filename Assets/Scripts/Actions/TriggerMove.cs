using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class TriggerMove : TriggerAction
{
    public Vector3 finalPosition;
    private Vector3 initialPosition;
    public float actionTime = 2f;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool isMovingBack = false;
    [SerializeField] private bool oneShot = false; // Por el momento está oculto en el inspector, veremos si a futuro lo necesitamos
    [SerializeField] private bool hasBeenActivated = false;
    private Tween currentTween;

    [SerializeField] private Material defaultMaterial, flickerMaterial;
    private Renderer myRenderer;
    [SerializeField] private GameObject platformModel;

    private void Awake()
    {
        if (platformModel != null)
        {
            myRenderer = platformModel.GetComponent<Renderer>();
        }

    }

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    public override bool ExecuteAction(bool instantaneus = false, Action onActionFinished = null)
    {
        if (_isMoving) return false; // Evitar activar mientras se mueve
        if (oneShot && hasBeenActivated) return false;

        if (isMovingBack)
        {
            // Si llega a esta línea significa que es un objeto que no se está moviendo a la ida, que no es de Onshot y que estaba en medio de un movimiento de vuelta
            // Por lo que paramos el movimiento de vuelta para comenzar el de ida

            isMovingBack = false;
            StopCurrentMovement();
        }

        Vector3 targetPosition = finalPosition;
        float timeMultiplier = instantaneus ? 0f : 1f;


        currentTween = DOTween.Sequence()
            .Append(transform.DOLocalMove(targetPosition, actionTime * timeMultiplier).SetEase(Ease.InOutSine))
            .OnStart(() => _isMoving = true)
            .OnComplete(() =>
            {
                _isMoving = false;
                hasBeenActivated = true;
                onActionFinished?.Invoke();
            });
        return true;

    }

    public override bool UndoAction(bool instantaneus = false, Action onActionFinished = null)
    {
        if (isMovingBack) return false;
        Vector3 targetPosition = initialPosition;
        float timeMultiplier = instantaneus ? 0f : 1f;

        // Debug.Log("TriggerMove: volviendo");
        currentTween = DOTween.Sequence()
            .Append(transform.DOLocalMove(targetPosition, actionTime * timeMultiplier).SetEase(Ease.InOutSine))
            .OnStart(() => isMovingBack = true)
            .OnComplete(() => {
                isMovingBack = false;
                onActionFinished?.Invoke();
            });

        //ChangeMaterial(defaultMaterial);
        return true;
    }

    public void StopCurrentMovement()
    {
        currentTween.Kill();
    }
}
