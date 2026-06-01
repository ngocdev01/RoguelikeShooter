using UnityEditor;
using UnityEditor.Overlays;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Scene Stats", true)]
public class SceneStatsOverlay : Overlay
{
    private Label fpsLabel;
    private Label cpuLabel;
    private Label drawCallsLabel;

    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement { style = { flexDirection = FlexDirection.Column } };

        fpsLabel = new Label("FPS: Calculating...");
        cpuLabel = new Label("CPU: Calculating...");
        drawCallsLabel = new Label("Draw Calls: Calculating...");

        root.Add(fpsLabel);
        root.Add(cpuLabel);
        root.Add(drawCallsLabel);

        // Schedule updates every frame
        root.schedule.Execute(UpdateStats).Every(100); // Update every 100ms

        return root;
    }

    private void UpdateStats()
    {
        // FPS calculation (approximate for editor)
        float fps = 1f / Time.unscaledDeltaTime;
        fpsLabel.text = $"FPS: {fps:F1}";

        // CPU time from Profiler (main thread)
        var cpuTime = ProfilerDriver.GetOverviewText(ProfilerArea.CPU, ProfilerDriver.lastFrameIndex);
        cpuLabel.text = $"CPU: {ExtractCpuTime(cpuTime)} ms"; // Parse as needed

        // Draw calls (requires Play mode or custom tracking in editor)
        var renderStats = ProfilerDriver.GetOverviewText(ProfilerArea.Rendering, ProfilerDriver.lastFrameIndex);
        drawCallsLabel.text = $"Draw Calls: {ExtractDrawCalls(renderStats)}";
    }

    // Helper methods to parse Profiler text (customize based on format)
    private string ExtractCpuTime(string profilerText)
    {
        // Simple parsing; adjust regex or string search as needed
        var lines = profilerText.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains("Main Thread"))
                return line.Substring(line.IndexOf(':') + 1).Trim(); // Example extraction
        }
        return "N/A";
    }

    private string ExtractDrawCalls(string profilerText)
    {
        // Similar parsing
        var lines = profilerText.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains("Draw Calls"))
                return line.Substring(line.IndexOf(':') + 1).Trim();
        }
        return "N/A";
    }
}