using System;

namespace Processor
{
    /// <summary>
    /// Represents a simple class that runs a process
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProcessorAttribute : Attribute
    {
        /// <summary>
        /// The key that should be used to represent this process on a menu
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Specifies the name of the process the class represents
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the category of the process
        /// </summary>
        public string Category { get; set; }
    }
}
