namespace Processor.ConsoleApp
{
    public abstract class ConsoleMenuItemBase
    {
        public string Key { get; }
        public string Name { get; }

        protected ConsoleMenuItemBase(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
