namespace Blog.Portfolio.Host;

/// <summary>
/// Lets architecture tests resolve this assembly via <c>typeof(HostAssemblyMarker).Assembly</c> instead of a
/// string-based <see cref="System.Reflection.Assembly.Load(string)"/>, so a project rename fails the build
/// instead of silently discovering zero endpoints at runtime.
/// </summary>
#pragma warning disable S2094 // Intentionally empty: exists only as a reflection anchor for this assembly
public static class HostAssemblyMarker;
#pragma warning restore S2094
