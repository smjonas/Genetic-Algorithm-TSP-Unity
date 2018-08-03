using UnityEngine;

public class City : MonoBehaviour {

    [Tooltip("The position of the city in the DNA / route")]
    public int index;
    public new LineRenderer renderer;

    void Start() {
        renderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
    }

    /// <summary>
    /// Positions a city at a given location.
    /// </summary>
    public void SetPosition(float x, float y) {
        transform.position = new Vector3(x-5.75f, y-1f);
    }

    /// <summary>
    /// Returns the distance between this city and another city.
    /// </summary>
    public float GetDistance(City city) {
        return (city.transform.position - transform.position).magnitude;
    }

    /// <summary>
    /// Draws a line between this city and another city.
    /// </summary>
    public void DrawLine(City city) {
        
        renderer.SetPosition(0, transform.position);
        renderer.SetPosition(1, city.transform.position);
    }
}
