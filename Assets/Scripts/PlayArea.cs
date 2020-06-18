using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {
    public float width;
    public float height;
    public float camerawidth;
    public float cameraheight;

    private SpriteRenderer renderer;

    private void Awake() {
        //reset to native scale, the scale will be different to aid in visualization during development
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
    }

    // Start is called before the first frame update
    void Start() {
        
        renderer = GetComponent<SpriteRenderer>();
        width = renderer.bounds.size.x;
        height = renderer.bounds.size.y;
    }

    // Update is called once per frame
    void Update() {
        /*
        RectTransform rect = (RectTransform)transform;
        width = rect.rect.width;
        height = rect.rect.height;
        */


        cameraheight = (float)(Camera.main.orthographicSize * 2.0);
        camerawidth = cameraheight / Screen.height * Screen.width;

        float widthval = Mathf.Round((camerawidth / width) * 100f) /100f;
        float heightval = Mathf.Round((cameraheight / height) * 100f) / 100f;

        if (transform.localScale.x != widthval || transform.localScale.y != heightval) { 
            transform.localScale = new Vector3(widthval, heightval, transform.localScale.z);
        }
    }
}
