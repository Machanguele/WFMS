namespace Domain
{
    public class ActivityDependency
    {
        public int Id { get; set; }
        public virtual Activity MainActivity { get; set; }
        public virtual Activity DependencyActivity { get; set; }
    }
}