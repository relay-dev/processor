namespace Processor.ConsoleApp
{
    public class MenuItem
    {
        public string Key { get; }
        public string Name { get; }

        public MenuItem(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
