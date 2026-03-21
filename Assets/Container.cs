using UnityEngine;

/// <summary>
/// Handles interactions between Ingredients (or tools) and containers.
/// 
/// Responsibilities:
/// 1. Accept valid ingredients/tools
/// 2. Optionally transform tools (e.g., Spoon → SpoonSoap)
/// 3. Trigger task progression (with order validation)
/// 4. Reset tools after use if needed
/// 
/// Does NOT handle:
/// - Visual state of containers (handled elsewhere)
/// </summary>

public class Container : MonoBehaviour
{
    [System.Serializable]
    public class IngredientAction {
        // The ID this container accepts (must match Ingredient.ingredientID)
        [Header("Matching")]
        public string ingredientID; 

        // If set → transforms the tool (e.g., Spoon → SpoonSoap)
        [Header("Tool Transformation (Optional)")]
        public string newToolID; 

        // Sprite to apply when tool transforms
        public Sprite newTool; 

        // Task triggered when this ingredient is accepted
        [Header("Task (Optional)")]
        public Task taskToComplete; 
    }

    // List of accepted ingredients/tools and their corresponding actions
    [Header("Accepted Ingredients / Actions")]
    [SerializeField] private IngredientAction[] acceptedIngredients;

    // Sprite to revert tool back to after use (e.g., Spoon)
    [Header("Tool Reset")]
    [SerializeField] private Sprite defaultToolSprite;

    // Reference to TaskManager for validating task progression and applying penalties
    private TaskManager taskManager;

    // Cache TaskManager reference on awake
    void Awake()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    /// <summary>
    /// Called when an object (ingredient/tool) is dropped onto this container.
    /// </summary>
    public void TryAccept(GameObject obj)
    {
        Ingredient ingredient = obj.GetComponent<Ingredient>();
        if (ingredient == null) return;

        foreach (IngredientAction action in acceptedIngredients)
        {
            // Check if this container accepts the ingredient
            if (ingredient.ingredientID != action.ingredientID)
                continue;

            Debug.Log("Accepted: " + action.ingredientID);

            // --- CASE 1: TOOL TRANSFORMATION ---
            // Example: Spoon → SpoonSoap (no task involved)
            if (!string.IsNullOrEmpty(action.newToolID))
            {
                TransformTool(obj, ingredient, action);
                return;
            }

            // --- CASE 2: NORMAL INTERACTION ---
            // Example: SpoonSoap → Flask (completes a task)
            if (!HandleTask(action))
                return;

            // After successful task completion, reset tool if needed
            ResetToolIfNeeded(obj, ingredient);
            return;
        }

        // --- WRONG INGREDIENT ---
        Debug.Log("Wrong ingredient added.");
        if (taskManager != null)
            taskManager.AddPenalty();
    }

    /// <summary>
    /// Transforms a tool into another state (e.g., Spoon → SpoonSoap).
    /// </summary>
    private void TransformTool(GameObject obj, Ingredient ingredient, IngredientAction action)
    {
        ingredient.SetIngredient(action.newToolID, null);

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null && action.newTool != null)
        {
            renderer.sprite = action.newTool;
        }

        Debug.Log("Tool now holds: " + action.newToolID);
    }

    /// <summary>
    /// Validates and completes a task using TaskManager.
    /// </summary>
    private bool HandleTask(IngredientAction action)
    {
        if (action.taskToComplete == null)
            return true;

        bool canProceed = taskManager.TryCompleteTask(action.taskToComplete);

        if (!canProceed)
            return false;

        action.taskToComplete.CompleteTask();
        return true;
    }

    /// <summary>
    /// Resets tool back to its base state after use (e.g., SpoonSoap → Spoon).
    /// </summary>
    private void ResetToolIfNeeded(GameObject obj, Ingredient ingredient)
    {
        if (string.IsNullOrEmpty(ingredient.baseIngredientID))
            return;

        if (ingredient.ingredientID == ingredient.baseIngredientID)
            return;

        ingredient.ResetIngredient();

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null && defaultToolSprite != null)
        {
            renderer.sprite = defaultToolSprite;
        }

        Debug.Log("Tool reset to: " + ingredient.baseIngredientID);
    }
}