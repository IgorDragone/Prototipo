using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeSliderController : MonoBehaviour
{
    public UnityEvent OnXRPointerEnter;
    public UnityEvent OnXRPointerExit;

    public Slider targetSlider; // Slider a controlar con la mirada
    private Camera xRCamera;

    private bool isGazing = false; // Si la mirada está activa
    public float gazeSpeed = 0.5f; // Velocidad de cambio del slider
    private float gazeDirection = 0; // Dirección de ajuste (-1 para bajar, 1 para subir)

    void Start()
    {
        xRCamera = CameraPointerManager.Instance.gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (isGazing && targetSlider != null)
        {
            // Ajustar el valor del slider en función de la dirección y la velocidad
            targetSlider.value += gazeDirection * gazeSpeed * Time.deltaTime;
        }
    }

    public void OnPointerEnterXR()
    {
        GazeManager.Instance.SetUpGaze(1.5f);
        OnXRPointerEnter.Invoke();

        // Determinar dirección de ajuste según la posición del puntero respecto al slider
        Vector3 screenPos = xRCamera.WorldToScreenPoint(CameraPointerManager.Instance.hitPoint);
        float normalizedPos = GetNormalizedPointerPosition(screenPos);
        gazeDirection = normalizedPos < 0.5f ? -1f : 1f; // Izquierda para bajar, derecha para subir

        isGazing = true;
    }

    public void OnPointerExitXR()
    {
        GazeManager.Instance.SetUpGaze(2.5f);
        OnXRPointerExit.Invoke();
        isGazing = false;
        gazeDirection = 0; // Detener el ajuste
    }

    private float GetNormalizedPointerPosition(Vector3 screenPos)
    {
        // Calcular posición normalizada del puntero en relación con el slider
        RectTransform sliderRect = targetSlider.GetComponent<RectTransform>();
        Rect rect = sliderRect.rect;
        Vector3 localPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(sliderRect, screenPos, xRCamera, out localPoint);
        float normalizedX = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
        return normalizedX;
    }
}