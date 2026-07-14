using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Script.Core.MeshProcessing
{
    
    public class NormalBaking : EditorWindow
    {
        [MenuItem("NgocDev/AssetProcessing/NormalBaking")]
        public static void OpenWindow() => EditorWindow.GetWindow<NormalBaking>();

        private void CreateGUI()
        {
            var root = rootVisualElement;
            var meshObjectField = new ObjectField("Mesh");
            meshObjectField.objectType = typeof(GameObject);
            meshObjectField.searchContext = SearchService.CreateContext("p: t:Model");
            root.Add(meshObjectField);


            var bakeButton = new Button();
            bakeButton.text = "Bake";
            bakeButton.clicked += () => BakeModel(meshObjectField.value as GameObject);
            root.Add(bakeButton);

        }
        public static void BakeModel(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            foreach (var obj in renderers)
            {
                var meshFilter = obj.GetComponent<MeshFilter>();
                var skinned = obj.GetComponent<SkinnedMeshRenderer>();
                Mesh mesh = meshFilter ? meshFilter.sharedMesh : (skinned ? skinned.sharedMesh : null);

                if (mesh == null) continue;

                BakeSmoothNormals(mesh);
                EditorUtility.SetDirty(mesh);
            }
            
            AssetDatabase.SaveAssets();
        }
        public static void BakeSmoothNormals(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            // Group vertices by position (for averaging)
            var groups = new Dictionary<Vector3, List<int>>(new Vector3Comparer());
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 pos = vertices[i];
                if (!groups.ContainsKey(pos)) groups[pos] = new List<int>();
                groups[pos].Add(i);
            }

            Color[] colors = new Color[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 avgNormal = Vector3.zero;
                foreach (int idx in groups[vertices[i]])
                    avgNormal += normals[idx];

                avgNormal.Normalize();

                // Encode normal [-1..1] → [0..1] for vertex color (like normal map)
                colors[i] = new Color(
                    avgNormal.x * 0.5f + 0.5f,
                    avgNormal.y * 0.5f + 0.5f,
                    avgNormal.z * 0.5f + 0.5f,
                    1f
                );
            }

            mesh.colors = colors;
            Debug.Log($"Baked smooth normals to vertex colors on {mesh.name}");
        }

        // Simple Vector3 comparer for Dictionary
        class Vector3Comparer : IEqualityComparer<Vector3>
        {
            public bool Equals(Vector3 a, Vector3 b) =>
                Mathf.Approximately(a.x, b.x) &&
                Mathf.Approximately(a.y, b.y) &&
                Mathf.Approximately(a.z, b.z);
            public int GetHashCode(Vector3 v) =>
                v.x.GetHashCode() ^ v.y.GetHashCode() ^ v.z.GetHashCode();
        }

    }
}
