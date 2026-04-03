using UnityEngine;

// This script should be attached to any ingredient GameObject that you want to be draggable.
public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    private bool isDragging = false;

    // Initialize references
    void Start()
    {
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // on click:
    //     bring the object to the front by changing its sorting layer
    //     calculate the offset between the mouse position and the object's position
    void OnMouseDown()
    {
        // Check global interaction lock (e.g. during pause, video playback, etc.)
        if (TaskManager.IsInteractionLocked) return;

        isDragging = true;

        spriteRenderer.sortingLayerName = "Front";
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;
    }

    // on hold and drag:
    //     update the object's position to follow the mouse, accounting for the offset
    void OnMouseDrag()
    {
        // Check global interaction lock (e.g. during pause, video playback, etc.)
        if (TaskManager.IsInteractionLocked) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos + offset;
    }

    // IMPORTANT:
    // Object only stays if BOTH:
    // 1. Container accepted it
    // 2. Ingredient is marked StayAfterUse (e.g., balloon)
    // Otherwise it returns to its original position
    void OnMouseUp() {
        // Check global interaction lock (e.g. during pause, video playback, etc.)
        if (TaskManager.IsInteractionLocked) return;
        
        isDragging = false;

        spriteRenderer.sortingLayerName = "Default";

        Collider2D myCollider = GetComponent<Collider2D>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            myCollider.bounds.center,
            myCollider.bounds.size,
            0f
        );

        bool accepted = false;

        foreach (Collider2D hit in hits) {
            if (hit.gameObject == this.gameObject) {
                continue;
            }

            Container container = hit.GetComponent<Container>();
            if (container != null)
            {
                Debug.Log("Container detected: " + hit.name);
                if (container.TryAccept(this.gameObject))
                {
                    accepted = true;
                    break;
                }
            }
        }

        // reset back to original position if not accepted or if ingredient should not stay after use
        if (!accepted || !GetComponent<Ingredient>().StayAfterUse)
        {
            transform.position = originalPosition;
        }
    }

    // Resets the ingredient to its original position and sorting layer.
    public void ResetState()
    {
        isDragging = false;
        spriteRenderer.sortingLayerName = "Default";
        transform.position = originalPosition;
    }

    // Public method to reset the ingredient if it's currently being dragged (e.g. when interactions are locked).
    public void ResetIfDragging()
    {
        if (isDragging)
        {
            isDragging = false;
            ResetState();
        }
    }

    // Allows external scripts (e.g. Container) to update the original position after moving the ingredient.
    public void SetNewOriginalPosition(Vector3 newPosition)
    {
        originalPosition = newPosition;
    }
}