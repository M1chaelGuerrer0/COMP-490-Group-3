using UnityEngine;

public class ingredient : MonoBehaviour
{
    // Add the task
    [SerializeField] private Task taskToComplete;

    // starting position and sprite
    private Vector3 originalPosition;   
    private SpriteRenderer spriteRenderer;
    
    // runs on start:
    //      Reads intial sprite and location
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
    }

     // offset prevents sprite from moving to middle of screen
    private Vector3 offset;

    // on click and hold:
    //      Sprite moves layer to Front
    //      Sprite moves to mouse location
    void OnMouseDown() {
        spriteRenderer.sortingLayerName = "Front";
        // finds mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // finds offset
        offset = transform.position - mousePosition;
    }

    // on drag:
    //      Sprite moves with mouse
    void OnMouseDrag() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition + offset;
    }

    // on let go:
    //      Change layer to Default
    //      Checks if in a drop off target Object for task completion
    //      Return sprite to original location
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
