using UnityEngine;

// Used for containers that accept ingredients (e.g., Flask, Cup, Sink).
// Also used for sources that tools can be dipped into / grabbed with (e.g., Soap for Spoon → SpoonSoap).
public class Container : MonoBehaviour
{
    [System.Serializable]
    public class IngredientSpritePair
    {
        public string ingredientID; // What this container accepts
        public Sprite newTool; // change tool sprite to this (optional, for cases like Spoon → SpoonSoap)
        public string newToolID; // If set → transform tool into this (e.g., Spoon → SpoonSoap). If null/empty → no transformation (e.g., Yeast → Cup)
        public Task taskToComplete; // Optional task to complete when accepted
    }
    
    [Header("Accepted Ingredients")]
    [SerializeField] private IngredientSpritePair[] acceptedIngredients;

    [Header("Tool Reset Sprite (Optional)")]
    [SerializeField] private Sprite defaultToolSprite;

    private SpriteRenderer spriteRenderer;
    private TaskManager taskManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    // This method is called when an ingredient is added to the container.
    public void TryAccept(GameObject obj)
    {
        Ingredient ingredient = obj.GetComponent<Ingredient>();
        if (ingredient == null) return;

        foreach (IngredientSpritePair pair in acceptedIngredients)
        {
            // Accept either exact match OR base tool (for flexibility like SpoonSoap → Sink)
            if (ingredient.ingredientID == pair.ingredientID)
            {
                Debug.Log("Accepted: " + pair.ingredientID);

                // CASE 1: TRANSFORM TOOL (e.g., Spoon → SoapLoaded)
                if (!string.IsNullOrEmpty(pair.newToolID))
                {
                    ingredient.SetIngredient(pair.newToolID, null);

                    // Change TOOL sprite
                    SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
                    if (objRenderer != null && pair.newTool != null)
                    {
                        objRenderer.sprite = pair.newTool;
                    }

                    Debug.Log("Tool now holds: " + pair.newToolID);
                    return;
                }

                // CASE 2: NORMAL USE (e.g., SpoonSoap → Flask, Yeast → Cup):

                // Complete task
                bool canProceed = true;

                if (pair.taskToComplete != null)
                {
                    canProceed = taskManager.TryCompleteTask(pair.taskToComplete);

                    if (!canProceed)
                    {
                        return; // stop everything
                    }

                    // NOW actually complete the task
                    pair.taskToComplete.CompleteTask();
                }

                // ONLY reset if it's actually a TOOL (like spoon)
                if (!string.IsNullOrEmpty(ingredient.baseIngredientID))
                {
                    // AND only if it was transformed (e.g., SpoonSoap → Spoon)
                    if (ingredient.ingredientID != ingredient.baseIngredientID)
                    {
                        ingredient.ResetIngredient();

                        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
                        if (objRenderer != null && defaultToolSprite != null)
                        {
                            objRenderer.sprite = defaultToolSprite;
                        }

                        Debug.Log("Tool reset to: " + ingredient.baseIngredientID);
                    }
                }
                return;
            }
        }

        // Wrong ingredient
        Debug.Log("Wrong ingredient added.");
        if (taskManager != null)
            taskManager.AddPenalty();
    }
}