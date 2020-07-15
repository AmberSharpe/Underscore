using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoundedTracker : MonoBehaviour
{

    public Transform target;
    public Rect bounds = new Rect(0,0,256, 144);
    
    public int tileSize = 16;
    public float horizontalTileBound = 8f;
    public float verticalTileBound = 4.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector2(bounds.xMin, bounds.yMin), new Vector2(bounds.xMax, bounds.yMin));
        Debug.DrawLine(new Vector2(bounds.xMin, bounds.yMax), new Vector2(bounds.xMax, bounds.yMax));   
        Debug.DrawLine(new Vector2(bounds.xMin, bounds.yMin), new Vector2(bounds.xMin, bounds.yMax));
        Debug.DrawLine(new Vector2(bounds.xMax, bounds.yMin), new Vector2(bounds.xMax, bounds.yMax));

        Debug.DrawLine(new Vector2(bounds.xMin + 16 * horizontalTileBound, bounds.yMin + 16 * verticalTileBound), new Vector2(bounds.xMax - 16 * horizontalTileBound, bounds.yMin + 16 * verticalTileBound), Color.red);
        Debug.DrawLine(new Vector2(bounds.xMin + 16 * horizontalTileBound, bounds.yMax - 16 * verticalTileBound), new Vector2(bounds.xMax - 16 * horizontalTileBound, bounds.yMax - 16 * verticalTileBound), Color.red);
        Debug.DrawLine(new Vector2(bounds.xMin + 16 * horizontalTileBound, bounds.yMin + 16 * verticalTileBound), new Vector2(bounds.xMin + 16 * horizontalTileBound, bounds.yMax - 16 * verticalTileBound), Color.red);
        Debug.DrawLine(new Vector2(bounds.xMax - 16 * horizontalTileBound, bounds.yMin + 16 * verticalTileBound), new Vector2(bounds.xMax - 16 * horizontalTileBound, bounds.yMax - 16 * verticalTileBound), Color.red);

        if (target != null)
        {

            transform.position = new Vector3(
                Mathf.Clamp(target.position.x,bounds.xMin + 16 * horizontalTileBound, bounds.xMax - 16 * horizontalTileBound),
                Mathf.Clamp(target.position.y, bounds.yMin + 16 * verticalTileBound, bounds.yMax - 16 * verticalTileBound),
                0);
            
            Debug.DrawLine(target.position, transform.position);
        }
  
    }
}
