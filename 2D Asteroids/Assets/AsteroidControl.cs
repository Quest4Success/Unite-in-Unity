using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidControl : MonoBehaviour
{
    public int RADIUS = 5;
    public Material MAT;

    // Start is called before the first frame update
    void Start()
    {
        GenerateShape();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A pressed");
            transform.localPosition += Vector3.left;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("W pressed");
            transform.localPosition = SceneManager.GetActiveScene().;

        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("D pressed");
            transform.localPosition += Vector3.right;

        }

        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("S pressed");
            transform.localPosition += Vector3.down;

        }
    }

    void GenerateShape()
    {

        float radians = Random.Range(Mathf.PI, 2 * (float)Mathf.PI);
        sbyte numVertices = 10;
        float[] shapex = new float[numVertices];
        float[] shapey = new float[numVertices];
        float[] dists = new float[numVertices];

        /*
         * Give 10 random distances from the
         * center of the object. Each end represents 
         * a what will be a vertex
         */
        for (int i = 0; i < numVertices; i++)
        {
            dists[i] = Random.Range(RADIUS / 2, RADIUS);
        }

        float angle = 0;
        Vector3[] vertices = new Vector3[numVertices];

         /*
         * At some angle around the center of the GO
         * and at the currently selected distance
         * get save the coordinates as 1 vertex
         */
        for (int i = 0; i < numVertices; i++)
        {
            shapex[i] = Mathf.Cos(angle + radians) * dists[i];
            shapey[i] = Mathf.Sin(angle + radians) * dists[i];

            angle += 2 * ((float)Mathf.PI) / numVertices;

            vertices[i] = new Vector3(shapex[i], shapey[i], 0);
        }

        /*
        * Need to draw each segment separately.
        * LineRender does not keep equal lengths 
        * when of each vertex is drawn
        */
        Vector3[] edge = new Vector3[2];

        //Draw each vertex
        for (int i = 0; i < vertices.Length; i++)
        {
            //Generate a new child for each segment
            GameObject child = new GameObject();
            child.name = "Edge" + i;
            child.transform.SetParent(this.transform);

            //Get next vertex to draw
            edge[0] = vertices[i];
            edge[1] = vertices[(i + 1) % vertices.Length];

            Debug.Log(edge[0] + " " + edge[1]);
            LineRenderer lr = child.AddComponent<LineRenderer>();
            lr.positionCount = edge.Length;

            lr.SetPositions(edge);
            lr.enabled = true;
            lr.material = MAT;
            lr.startColor =  Color.white;
            lr.endColor =  Color.white;
            lr.alignment = LineAlignment.View;
            lr.startWidth = .01f;
            lr.endWidth = .01f;

            MeshRenderer meshRenderer = child.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = MAT;

            MeshFilter meshFilter = child.gameObject.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            lr.BakeMesh(mesh);
            meshFilter.sharedMesh = mesh;
            Destroy(lr);
        }

         /*
         * Get Max and Min coordinate values to shape
         * and offset the bounding box. Some boxes
         * are closer to the edges than others but the 
         * box is more or less cenetered 
         */
        float maxX = shapex.Max(x => Mathf.Abs(x)),
        minX = shapex.Min(x => Mathf.Abs(x)),
        offsetX = (shapex.Max() + shapex.Min()) / 2f,
        
        maxY = shapey.Max(x => Mathf.Abs(x)), 
        minY = shapey.Min(x => Mathf.Abs(x)), 
        offsetY = (shapey.Max() + shapey.Min()) / 2f;

        BoxCollider2D b = gameObject.AddComponent<BoxCollider2D>();
        b.size = new Vector3(maxX * 2, maxY * 2, 0);
        b.offset = new Vector2(offsetX, offsetY);
    }
}
