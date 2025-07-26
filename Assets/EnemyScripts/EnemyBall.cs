using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;            
    public Vector3 direction = Vector3.zero;  

    [Header("Auto Destroy Settings")]
    public float margin = 1f;           
    public bool autoDestroy = true;    

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (autoDestroy)
            CheckOutOfScreen();
    }

    private void CheckOutOfScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPoint.x < -margin || screenPoint.x > 1 + margin ||
            screenPoint.y < -margin || screenPoint.y > 1 + margin)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the direction towards a target.
    /// </summary>
    public void SetDirectionToTarget(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
    }

    /// <summary>
    /// Change Speed at runtime.
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    /// <summary>
    /// Change Margin at runtime.
    /// </summary>
    public void SetMargin(float newMargin)
    {
        margin = newMargin;
    }
}
