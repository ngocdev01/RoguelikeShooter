using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading;
using UnityEngine.SceneManagement;



public abstract class LoadingJob {
    public float Progress { get; protected set; }
    public string Name { get; protected set; }
    public bool IsCompleted { get; protected set; } = false;
    public int Weight { get; protected set; } = 1;    
    public abstract Awaitable ExecuteAsync(CancellationToken cancelationToken = default);
}


public abstract class BackThreadLoadingJob : LoadingJob
{
    private Func<IProgress<float> , CancellationToken, Awaitable> _executeHeavyJobAsync;
    public BackThreadLoadingJob(string name, Func<IProgress<float>, CancellationToken, Awaitable> executeHeavyJobAsync)
    {
        Name = name;
        _executeHeavyJobAsync = executeHeavyJobAsync;
    }
    public override async Awaitable ExecuteAsync(CancellationToken cancelationToken = default)
    {
        var progress = new Progress<float>(p => Progress = p);

        await Awaitable.BackgroundThreadAsync();
        await _executeHeavyJobAsync(progress,cancelationToken);
        await Awaitable.MainThreadAsync();
    }
}


[System.Serializable]
public class SceneReference : AssetReference
{
    public SceneReference(string guid) : base(guid) { }

    public override bool ValidateAsset(UnityEngine.Object obj)
    {
        return obj is SceneAsset;
    }

    public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
        return type == typeof(SceneAsset);
#else
        return false;
#endif

    }
}



public class SceneLoader : MonoBehaviour
{

    public void LoadScene(string adress, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        Addressables.LoadSceneAsync(adress, loadSceneMode);
    }

    public void LoadScene(SceneReference sceneReference, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        Addressables.LoadSceneAsync(sceneReference, loadSceneMode);
    }


}

