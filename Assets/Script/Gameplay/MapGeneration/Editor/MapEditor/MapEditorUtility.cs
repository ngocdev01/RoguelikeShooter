using NgocDev.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace NgocDev.Gameplay.MapGeneration.Editor.MapEditor
{
    internal class MapEditorUtility
    {
        public static void DrawPreview(GameObject gameObject, Vector3 position)
        {
            var previewMaterial = MapEditorSetting.previewMaterial;
            if (previewMaterial != null)
            {

                var mesh = gameObject.GetComponent<MeshFilter>()?.sharedMesh;
                DrawPreview(mesh, position);
            }
        }

        public static void DrawPreview(Mesh mesh, Vector3 position)
        {
            var previewMaterial = MapEditorSetting.previewMaterial;
            if (previewMaterial != null)
            {
                previewMaterial.SetPass(0);
                Graphics.DrawMeshNow(mesh, Matrix4x4.Translate(position));
            }
        }

        public static void DrawPreview(Mesh mesh, Vector3[] positions)
        {
            RenderParams rp = new RenderParams(MapEditorSetting.previewMaterial);
            Matrix4x4[] instData = new Matrix4x4[positions.Length];
            for (int i = 0; i < positions.Length; ++i)
                instData[i] = Matrix4x4.Translate(positions[i]);
            Graphics.RenderMeshInstanced(rp, mesh, 0, instData);
        }


        public static Vector3Int GetMouseGridPosition( Grid grid)
        {
            var e = Event.current;
            Plane plane = new Plane(Vector3.up, grid.origin);
            if (HandleUtility.PlaceObject(e.mousePosition, out Vector3 hit, out Vector3 normal))
            {
                hit += normal * grid.cellSize / 2;
            }
            else if (HandlesExtensions.ScreenRaycastPlane(plane, out hit))
            {
                hit += plane.normal * grid.cellSize / 2;

            }    
            return Grid.WorldToGrid(grid, hit);
        }

    }
}
