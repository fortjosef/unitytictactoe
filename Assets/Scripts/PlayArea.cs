using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {
    public float backgroundSpriteWidth;
    public float backgroundSpriteHeight;
    public float camerawidth;
    public float cameraheight;
    public Sprite XSprite;
    public Sprite lineSprite;
    public int[,] playField;
    public float lineDimensionScaled;
    int boardSize;

    private float scaleFactor;

    private SpriteRenderer renderer;

    private void Awake() {
        //reset to native scale, the scale will be different to aid in visualization during development
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
    }

    // Start is called before the first frame update
    void Start() {
        renderer = GetComponent<SpriteRenderer>();
        backgroundSpriteWidth = renderer.bounds.size.x;
        backgroundSpriteHeight = renderer.bounds.size.y;
        /*
        Vector3 foo = new Vector3(1, 1, 0);
        Quaternion blah = Quaternion.Euler(0, 0, 90);
        foo = blah * foo;
        Debug.Log(foo);
        */

    }

    // Update is called once per frame
    void Update() {
        cameraheight = (float)(Camera.main.orthographicSize * 2.0);
        camerawidth = cameraheight / Screen.height * Screen.width;

        float widthval = Mathf.Round((camerawidth / backgroundSpriteWidth) * 100f) /100f;
        float heightval = Mathf.Round((cameraheight / backgroundSpriteHeight) * 100f) / 100f;

        if (transform.localScale.x != widthval || transform.localScale.y != heightval) { 
            transform.localScale = new Vector3(widthval, heightval, transform.localScale.z);
            transform.position = new Vector3(0, 0, 0);
            //layout board here, do i want to contain it in a gameobject?
            //when it is in a game object i can just scale the container rather than each object
            GameObject boardContainer = new GameObject();
            boardContainer.name = "boardContainer";
            boardSize = 3;
            var boardHeight = cameraheight - 40;
            var lineLongDimension = boardHeight / boardSize;
            playField = new int[boardSize, boardSize];
            

            GameObject go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = lineSprite;
            var lineWidth = sr.bounds.size.x;
            var lineHeight = sr.bounds.size.y;
            //Debug.Log("height" + lineHeight);
            //Debug.Log("width" + lineWidth);
            scaleFactor = lineLongDimension / lineWidth;

            //will need to rotate on the z
            //add verticals
            lineDimensionScaled = lineWidth * scaleFactor;
            //Debug.Log(lineDimensionScaled);
            //Debug.Log((boardSize / 2f) - .5f);
            float yCoord = ((boardSize / 2f) - .5f) * lineDimensionScaled;
            float xCoord = -(((boardSize - 1) / 2f) - .5f) * lineDimensionScaled;

            for (var y = 0; y < boardSize - 1; y++) {
                for (var x = 0; x < boardSize; x++) {
                    //Debug.Log(yCoord);
                    if (y != 0 || (y == 0 && x != 0)) {
                        go = new GameObject();
                        go.AddComponent<SpriteRenderer>();
                        sr = go.GetComponent<SpriteRenderer>();
                        sr.sprite = lineSprite;
                    }

                    go.transform.rotation = Quaternion.Euler(0, 0, 90);
                    go.transform.localScale = new Vector3(scaleFactor, scaleFactor, 0);
                    go.transform.position = new Vector3(xCoord, yCoord, 0);
                    go.transform.parent = boardContainer.transform;

                    yCoord -= lineDimensionScaled;
                }

                yCoord = ((boardSize / 2f) - .5f) * lineDimensionScaled;
                xCoord += lineDimensionScaled;
            }

            //horizontal lines

            yCoord = (((boardSize - 1) / 2f) - .5f) * lineDimensionScaled; 
            xCoord = -((boardSize / 2f) - .5f) * lineDimensionScaled;

            for (var y = 0; y < boardSize - 1; y++) {
                for (var x = 0; x < boardSize; x++) {
                    go = new GameObject();
                    go.AddComponent<SpriteRenderer>();
                    sr = go.GetComponent<SpriteRenderer>();
                    sr.sprite = lineSprite;
                    

                    go.transform.rotation = Quaternion.Euler(0, 0, 0);
                    go.transform.localScale = new Vector3(scaleFactor, scaleFactor, 0);
                    go.transform.position = new Vector3(xCoord, yCoord, 0);
                    go.transform.parent = boardContainer.transform;

                    xCoord += lineDimensionScaled;
                }

                xCoord = -((boardSize / 2f) - .5f) * lineDimensionScaled;
                yCoord -= lineDimensionScaled;
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float boardBound = (boardSize / 2f) * lineDimensionScaled;
            //Debug.Log(mousepos.x.ToString() + " " + mousepos.y.ToString() + " " + boardBound.ToString() + "\n");
            

            if (mousepos.x > -boardBound && mousepos.x < boardBound
                && mousepos.y > -boardBound && mousepos.y < boardBound) {

                //want to make sure there is only one per box and normalize the position to the center of the box
                float amountToAdd = (boardSize / 2f) * lineDimensionScaled; //maybe just reuse BoardBound
                float mouseXadj = mousepos.x + amountToAdd;
                float mouseYadj = -(mousepos.y - amountToAdd);
                int xIndex = (int)Mathf.Floor(mouseXadj / lineDimensionScaled);
                int yIndex = (int)Mathf.Floor(mouseYadj / lineDimensionScaled);
                Debug.Log(xIndex.ToString() + " " + yIndex.ToString());
                GameObject go = new GameObject();
                go.AddComponent<SpriteRenderer>();
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                Sprite sprite = XSprite;
            
                sr.sprite = sprite;
                go.transform.localScale = new Vector3(scaleFactor, scaleFactor, 0);
                go.transform.position = new Vector3(mousepos.x, mousepos.y, transform.position.z);
            }
            
        }
    }
}
