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

    void h2o2Added(Task completedTask) {
        Debug.Log("Changed Sprite");
        spriteRenderer.sprite = h2o2Mix;
        h2o2Task.OnTaskCompleted -= h2o2Added;
    }

    void soapAdded(Task completedTask) {
        Debug.Log("Changed Sprite");
        spriteRenderer.sprite = soapMix;
        soapTask.OnTaskCompleted -= soapAdded;
    }
}
