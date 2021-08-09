using System;

namespace Repo.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UnimappedManyToManyAttribute : Attribute
    {
        public UnimappedManyToManyAttribute() { }

        public UnimappedManyToManyAttribute(string table)
        {
            Table = table;
        }

        public string Table { get; private set; }
    }
}
