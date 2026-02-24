using UnityEngine;

// This script should be attached to the container GameObject (e.g., flask, cup).
public class Container : MonoBehaviour
{
    [System.Serializable]
    public class IngredientSpritePair
    {
        public string ingredientID;
        public Sprite newSprite;
    }

    [SerializeField] private IngredientSpritePair[] acceptedIngredients;

    private SpriteRenderer spriteRenderer;
    private TaskManager taskManager;

    // Initialize references
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        taskManager = FindObjectOfType<TaskManager>();
    }

    // This method is called when an ingredient is added to the container.
    public void TryAccept(GameObject obj)
    {
        Ingredient ingredient = obj.GetComponent<Ingredient>();
        if (ingredient == null) return;

        foreach (IngredientSpritePair pair in acceptedIngredients)
        {
            if (ingredient.ingredientID == pair.ingredientID)
            {
                Debug.Log("Accepted: " + pair.ingredientID);

                // Complete task
                if (ingredient.taskToComplete != null)
                    ingredient.taskToComplete.CompleteTask();

                // Change sprite
                if (pair.newSprite != null)
                    spriteRenderer.sprite = pair.newSprite;

                return;
            }
        }

        Debug.Log("Wrong ingredient added.");
        taskManager.AddPenalty();
    }
}