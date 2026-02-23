using UnityEngine;

public class flask : MonoBehaviour
{
    // sprites
    [SerializeField] private Sprite h2o2Mix;
    [SerializeField] private Sprite soapMix;

    // tasks
    [SerializeField] private Task h2o2Task;
    [SerializeField] private Task soapTask;

    private SpriteRenderer spriteRenderer;

    // runs on start:
    //      Reads intial sprite and location
    //      Check if task is done to change sprite
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(0f,0f,0f);
        transform.localScale = new Vector3(1f,1f,1f);
        if (h2o2Task != null) {
            h2o2Task.OnTaskCompleted += h2o2Added;
        }
        if (soapTask != null) {
            soapTask.OnTaskCompleted += soapAdded;
        }
    }

    // When H202 Task is completed:
    //      Change sprite and remove task
    void h2o2Added(Task completedTask) {
        Debug.Log("Changed Sprite");
        spriteRenderer.sprite = h2o2Mix;
        h2o2Task.OnTaskCompleted -= h2o2Added;
    }

    // When Soap Task is completed:
    //      Change sprite and remove task
    void soapAdded(Task completedTask) {
        Debug.Log("Changed Sprite");
        spriteRenderer.sprite = soapMix;
        soapTask.OnTaskCompleted -= soapAdded;
    }
}
