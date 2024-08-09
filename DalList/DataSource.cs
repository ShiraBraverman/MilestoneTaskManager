namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal const int startDependencyId = 40;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal const int startTaskId = 20;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++;}

        internal static DateTime? startProject = new DateTime(2024,1,1);
        internal static DateTime? deadlineProject = new DateTime(2024, 6, 1);
    }

    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();
    internal static List<DO.Task?> Tasks { get; } = new();
}