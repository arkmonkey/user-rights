
namespace userrightslib
{
    /// <summary>
    /// Represents a right of a specific ID/Role
    /// </summary>
    public class UserRight
    {
        public Right Right { get; set; }
        public int Id { get; set; }
        public bool Value { get; set; }
    }
}
