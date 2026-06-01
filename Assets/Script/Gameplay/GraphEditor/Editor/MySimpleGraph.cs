using System;
using UnityEditor;
using UnityEngine;
using Unity.GraphToolkit.Editor;

namespace NgocDev.Gameplay.Graph.Editor
{

    [Graph(AssetExtension)]
    [Serializable]
    class MySimpleGraph : Unity.GraphToolkit.Editor.Graph
    {
        public const string AssetExtension = "simpleg";

        [MenuItem("Assets/Create/Graph Toolkit Samples/Simple Graph", false)]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<MySimpleGraph>();
        }
        public override void OnEnable()
        {
            
        }
    }

    

    [Serializable]
    public class CustomNode : Node
    {
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption("test node",typeof(UnityEngine.Object));
        }
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {

            context.AddInputPort("in").Build();
            context.AddOutputPort("out").Build();
           
        }
    }

    
}
