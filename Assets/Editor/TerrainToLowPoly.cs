using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TerrainToLowPoly : EditorWindow
{
    Terrain terrain;
    int resolution = 64;          // celdas por lado: menos = más low poly
    float jitter = 0.3f;
    float shadeVariation = 0.04f;
    bool blendColors = false;
    Material material;
    Color[] layerColors = new Color[0];

    [MenuItem("Tools/Terrain a Low Poly")]
    static void Open() => GetWindow<TerrainToLowPoly>("Low Poly Terrain");

    void OnGUI()
    {
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
        resolution = EditorGUILayout.IntSlider("Resolución (celdas)", resolution, 8, 200);
        jitter = EditorGUILayout.Slider("Irregularidad", jitter, 0f, 0.45f);
        shadeVariation = EditorGUILayout.Slider("Variación de tono", shadeVariation, 0f, 0.15f);
        blendColors = EditorGUILayout.Toggle("Mezclar colores", blendColors);
        material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material), false);
        if (terrain == null) return;

        var layers = terrain.terrainData.terrainLayers;
        if (layerColors.Length != layers.Length)
        {
            var old = layerColors;
            layerColors = new Color[layers.Length];
            for (int i = 0; i < layers.Length; i++)
                layerColors[i] = i < old.Length ? old[i] : Color.white;
        }

        EditorGUILayout.LabelField("Color de cada layer", EditorStyles.boldLabel);
        for (int i = 0; i < layers.Length; i++)
            layerColors[i] = EditorGUILayout.ColorField(layers[i] ? layers[i].name : "Layer " + i, layerColors[i]);

        if (GUILayout.Button("Generar")) Generate();
    }

    void Generate()
    {
        TerrainData td = terrain.terrainData;
        Vector3 size = td.size;
        int n = resolution;
        int aw = td.alphamapWidth, ah = td.alphamapHeight, L = td.alphamapLayers;
        float[,,] alpha = td.GetAlphamaps(0, 0, aw, ah);

        var verts = new List<Vector3>();
        var cols = new List<Color>();
        var tris = new List<int>();

        Vector3 P(int x, int z)
        {
            float u = (float)x / n, v = (float)z / n;
            if (x > 0 && x < n && z > 0 && z < n)   // los bordes no se mueven
            {
                Random.InitState(x * 73856093 ^ z * 19349663);
                u += Random.Range(-jitter, jitter) / n;
                v += Random.Range(-jitter, jitter) / n;
            }
            return new Vector3(u * size.x, td.GetInterpolatedHeight(u, v), v * size.z);
        }

        Color ColorAt(Vector3 p)
        {
            if (L == 0) return Color.white;
            int ax = Mathf.Clamp((int)(p.x / size.x * (aw - 1)), 0, aw - 1);
            int az = Mathf.Clamp((int)(p.z / size.z * (ah - 1)), 0, ah - 1);
            if (blendColors)
            {
                Color mix = Color.black;
                for (int l = 0; l < L; l++) mix += layerColors[l] * alpha[az, ax, l];
                mix.a = 1f;
                return mix;
            }
            int best = 0; float bw = -1f;
            for (int l = 0; l < L; l++)
            {
                float w = alpha[az, ax, l];
                if (w > bw) { bw = w; best = l; }
            }
            return layerColors[best];
        }

        void Tri(Vector3 a, Vector3 b, Vector3 c)
        {
            Color col = ColorAt((a + b + c) / 3f);
            float k = 1f + Random.Range(-shadeVariation, shadeVariation);
            col = new Color(col.r * k, col.g * k, col.b * k, 1f);
            int i = verts.Count;
            verts.Add(a); verts.Add(b); verts.Add(c);
            cols.Add(col); cols.Add(col); cols.Add(col);
            tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
        }

        for (int z = 0; z < n; z++)
            for (int x = 0; x < n; x++)
            {
                Vector3 a = P(x, z), b = P(x + 1, z), c = P(x, z + 1), d = P(x + 1, z + 1);
                Tri(a, c, b);
                Tri(b, c, d);
            }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.SetVertices(verts);
        mesh.SetColors(cols);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();   // vértices no compartidos = caras planas
        mesh.RecalculateBounds();

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/TerrainLowPoly.asset");
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();

        GameObject go = new GameObject("Terrain_LowPoly");
        go.transform.position = terrain.transform.position;
        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        go.AddComponent<MeshRenderer>().sharedMaterial = material;
        go.AddComponent<MeshCollider>().sharedMesh = mesh;
        Undo.RegisterCreatedObjectUndo(go, "Low Poly Terrain");

        terrain.gameObject.SetActive(false);   // el original queda oculto, no borrado
    }
}
