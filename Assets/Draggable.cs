using UnityEngine;

// This script should be attached to any ingredient GameObject that you want to be draggable.
public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer.sortingLayerName = "Front";
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;
    }

    // on hold and drag:
    //     update the object's position to follow the mouse, accounting for the offset
    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos + offset;
    }

    // on release:
    //     reset the sorting layer to default
    //     check for overlapping containers and try to place the ingredient in one if found
    void OnMouseUp() {
        spriteRenderer.sortingLayerName = "Default";

        Collider2D myCollider = GetComponent<Collider2D>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            myCollider.bounds.center,
            myCollider.bounds.size,
            0f
        );

        bool foundContainer = false;

        foreach (Collider2D hit in hits) {
            if (hit.gameObject == this.gameObject) {
                continue;
            }

            Container container = hit.GetComponent<Container>();

            if (container != null)
            {
                Debug.Log("Container detected: " + hit.name);
                container.TryAccept(this.gameObject);
                foundContainer = true;
                break;
            }
        }

        if (!foundContainer)
        {
            Debug.Log("No container detected.");
        }

        transform.position = originalPosition;
    }
}