using UnityEngine;

public class TileScript : MonoBehaviour
{
    // Private
    Vector3 correctPosition;
    SpriteRenderer _sprite;
    private Color correctPosColor;

    // Public
    public Vector3 targetPosition;
    public int number;
    public bool inRightPlace;
    
    // Start is called before the first frame update
    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
        // Convert hex color to a Unity Color
        ColorUtility.TryParseHtmlString("#EDAE49", out correctPosColor);

    }

    // Update is called once per frame
    void Update()
    {
        // Changing the position of the tiles after the Mouse click to the Empty Space

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
        if (targetPosition == correctPosition)
        {
            _sprite.color = correctPosColor;
            inRightPlace = true;
        }
        else
        {
            _sprite.color = Color.white;
            inRightPlace = false;
            
        }
    }
}
