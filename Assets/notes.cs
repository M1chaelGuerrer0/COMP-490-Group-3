using UnityEngine;


public class notes : MonoBehaviour
{
    bool isZoomed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isZoomed = false;
        transform.position = new Vector3(5f,-3.3f,0f);
        transform.localScale = new Vector3(0.3f,0.4f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        isZoomed = !isZoomed;
        if (isZoomed) {
            transform.position = new Vector3(0f,0f,0f);
            transform.localScale = new Vector3(1f,1.4f,1f);
        }
        else {
            isZoomed = false;
            transform.position = new Vector3(5f,-3.3f,0f);
            transform.localScale = new Vector3(0.3f,0.4f,1f);
        }
    }
}
