using System;

namespace ResourceShortageManager.Models
{
    public class Shortage
    {
        /// <summary>
        /// Title of the shortage
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// User who registered the shortage
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Room where the shortage is located
        /// </summary>
        /// <value></value>
        public required Room Room { get; set; }
        /// <summary>
        /// Category of the shortage
        /// </summary>
        /// <value></value>
        public required Category Category { get; set; }
        /// <summary>
        /// Shortage priority
        /// </summary>
        /// <value></value>
        public required int Priority { get; set; }
        /// <summary>
        /// Date and time when the shortage was registered
        /// </summary>
        /// <value></value>
        public required DateTime CreatedOn { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return string.Format("| {0, -20} | {1, -10} | {2, -15} | {3, -12} | {4, -8} | {5, -20} |",
                Title, Name, Room, Category, Priority, CreatedOn);
        }
    }
}
