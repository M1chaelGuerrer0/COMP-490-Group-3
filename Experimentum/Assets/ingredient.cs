using UnityEngine;

public class ingredient : MonoBehaviour
{
    // Adds the task
    [SerializeField] private Task taskToComplete;

    // starting position
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
    }

     // offset prevents sprite from moving to middle of screen
    private Vector3 offset;

    // on click
    void OnMouseDown() {
        spriteRenderer.sortingLayerName = "Front";
        // finds mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // finds offset
        offset = transform.position - mousePosition;
    }

    // on drag
    void OnMouseDrag() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition + offset;
    }

    // on let go
    void OnMouseUp() {
        spriteRenderer.sortingLayerName = "Default";
        // finds if item was dropped on target
        Collider2D dropTarget = checkForDropTarget();

        // if right target, task completed
        if (dropTarget != null) {
            taskToComplete.CompleteTask();
        }

        // return to starting position
        transform.position = originalPosition;
    }

    // checks if sprite was dropped on the targeted sprite
    Collider2D checkForDropTarget() {
        // gets all overlapping sprites from current sprite
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        // checks all overlapping sprites for targeted sprite
        foreach (Collider2D collider in overlaps) {
            if (collider.CompareTag("Container")) {
                return collider; // found
            }
        }
        return null; // not found
    }
}
