using UnityEngine;

public class CrystalPowderController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;

    private Collider2D myCollider;

    private void Start()
    {
        startPosition = transform.position;
        myCollider = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        isDragging = true;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        offset = transform.position - mouseWorldPos;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos + offset;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            myCollider.bounds.center,
            myCollider.bounds.size,
            0f
        );

        Debug.Log("Dropped crystal powder");

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            Debug.Log("Hit object: " + hit.name + " | Tag: " + hit.tag);

            if (hit.CompareTag("Cups"))
            {
                CrystalCupController cup = hit.GetComponent<CrystalCupController>();

                if (cup != null)
                {
                    cup.AddCrystalPowder();
                    ReturnToStart();
                    return;
                }
                else
                {
                    Debug.Log("CrystalCupController missing on " + hit.name);
                }
            }
        }

        ReturnToStart();
    }

    private void ReturnToStart()
    {
        transform.position = startPosition;
    }
}