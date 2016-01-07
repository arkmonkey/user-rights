
namespace userrights
{
    /// <summary>
    /// Represents a right of a specific ID/Role
    /// </summary>
    public class ContextRight
    {
        public Right Right { get; set; }
        public int Id { get; set; }
        bool Value { get; set; }
    }
}
