using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float WIDTH = 1f;
    private float HEIGHT = 2f;
    public Material MAT;

    // Start is called before the first frame update
    void Start()
    {
        GenerateShape();
    }

     /*
     * The shape for the player is generated everytime
     * the game is started just to make things interesting
     */
    void GenerateShape()
    {
        //Holds all vertices for shape
        Vector3[] vertices = new Vector3[4]
        {

            new Vector3(WIDTH/2, HEIGHT, 0),
            new Vector3(0, 0, 0),
            new Vector3(WIDTH/2, HEIGHT/3, 0),
            new Vector3(WIDTH, 0, 0),
        };

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
            child.name = "Edge_" + i;
            child.transform.SetParent(transform);

            //Get next vertex to draw
            edge[0] = vertices[i];
            edge[1] = vertices[(i + 1) % vertices.Length];

            //Draw line
            LineRenderer lr = child.AddComponent<LineRenderer>();
            lr.positionCount = edge.Length;
            lr.SetPositions(edge);
            
            /*
            * Set attributes of material that will be added
            * to current segment
            */
            lr.enabled = true;
            lr.material = MAT;
            lr.startColor = Color.white;
            lr.endColor = Color.white;
            lr.alignment = LineAlignment.View;
            lr.startWidth = .01f;
            lr.endWidth = .01f;
        }

        /*
        * This is a way to generate a image of the line and render it as if 
        * it's a gameObject. Keeping just as a reference

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();
            lr.BakeMesh(mesh);
            meshFilter.sharedMesh = mesh;
            Destroy(lr);
        */
    }
}
