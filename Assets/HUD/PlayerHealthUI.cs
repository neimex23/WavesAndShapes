using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Image coreImage;
    public Image shieldImage;
    public Transform cellsContainer;
    public GameObject cellPrefab; 

    [Header("Shield Settings")]
    public float shieldRotationSpeed = 45f;

    private List<Image> fullCells = new List<Image>();

    void Start()
    {
        if (playerHealth == null)
            playerHealth = Object.FindFirstObjectByType<PlayerHealth>();

        CreateCells();
        UpdateCells();
    }

    void Update()
    {
        RotateShield();
        UpdateCells();
    }

    void RotateShield()
    {
        if (shieldImage != null)
            shieldImage.transform.Rotate(0f, 0f, shieldRotationSpeed * Time.deltaTime);
    }

    void CreateCells()
    {
        foreach (Transform child in cellsContainer)
            Destroy(child.gameObject);

        fullCells.Clear();

        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            GameObject cellGO = Instantiate(cellPrefab, cellsContainer);
            Image fullImg = cellGO.transform.Find("Full").GetComponent<Image>();
            fullCells.Add(fullImg);
        }
    }

    void UpdateCells()
    {
        for (int i = 0; i < fullCells.Count; i++)
        {
            fullCells[i].enabled = (i < playerHealth.currentHealth);
        }
    }
}
