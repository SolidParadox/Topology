using UnityEngine;

public class SpherePointsGenerator : MonoBehaviour {
  public int numPoints = 100;
  public float radius = 1.0f;

  void Start () {
    GenerateFibonacciLattice ();
  }

  void GenerateFibonacciLattice () {
    var points = new Vector3[numPoints];
    var phi = Mathf.PI * (3 - Mathf.Sqrt(5)); // Golden ratio

    for ( int i = 0; i < numPoints; i++ ) {
      float y = 1 - (i / (float)(numPoints - 1)) * 2; // Vertical position
      float radiusAtY = Mathf.Sqrt(1 - y * y); // Radius at this height

      float theta = phi * i; // Horizontal angle based on Fibonacci sequence

      float x = Mathf.Cos(theta) * radiusAtY;
      float z = Mathf.Sin(theta) * radiusAtY;

      points [ i ] = new Vector3 ( x, y, z );
    }

    // Draw points or use them as needed
    Vector3 delta = transform.position;
    foreach ( Vector3 point in points ) {
      delta = transform.position + point * radius;
      Debug.DrawLine ( delta, delta + delta * 0.1f, Color.red );
      //Debug.DrawLine ( transform.position, delta );
      // Draw or use the point as needed
    }
  }

  private void Update () {
    GenerateFibonacciLattice();
  }
}
