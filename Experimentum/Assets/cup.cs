using UnityEngine;

public class cup : MonoBehaviour
{
    // sprites
    [SerializeField] private Sprite yeastMix;
    [SerializeField] private Sprite waterMix;

    // tasks
    [SerializeField] private Task yeastTask;
    [SerializeField] private Task waterTask;
    [SerializeField] private Task taskToComplete;


    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        if (yeastTask != null) {
            yeastTask.OnTaskCompleted += yeastAdded;
        }
        if (waterTask != null) {
            waterTask.OnTaskCompleted += waterAdded;
        }
    }

    void yeastAdded(Task completedTask) {
        Debug.Log("Changed Sprite");
        spriteRenderer.sprite = yeastMix;
        yeastTask.OnTaskCompleted -= yeastAdded;
    }

    void waterAdded(Task completedTask) {
        Debug.Log("Changed Sprite");
        spriteRenderer.sprite = waterMix;
        waterTask.OnTaskCompleted -= waterAdded;
    }

    // ingredient part

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
