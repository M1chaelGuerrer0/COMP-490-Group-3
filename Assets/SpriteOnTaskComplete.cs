using UnityEngine;

public class SpriteOnTaskComplete : MonoBehaviour
{
    [System.Serializable]
    public class TaskSpritePair
    {
        public Task task;
        public Sprite sprite;
    }

    [SerializeField] private TaskSpritePair[] taskSprites;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach (TaskSpritePair pair in taskSprites)
        {
            if (pair.task != null)
            {
                pair.task.OnTaskCompleted += (t) => ChangeSprite(pair);
            }
        }
    }

    void ChangeSprite(TaskSpritePair pair)
    {
        if (pair.sprite != null)
        {
            spriteRenderer.sprite = pair.sprite;
            Debug.Log($"{gameObject.name} changed to sprite: {pair.sprite.name}");
        }
    }
}