using System.Collections.Generic;

namespace Quill
{
    public class BeetleRegistry
    {
        private readonly Dictionary<string, int> _nameToId = new Dictionary<string, int>();

        public void RegisterNameToIdCache()
        {
            RegisterClass(0, "dung", "d", "scarab", "0");
            RegisterClass(1, "goliath", "g", "1");
            RegisterClass(2, "ladybug", "lb", "2");
            RegisterClass(3, "rhino", "r", "3");
            RegisterClass(4, "earthboring", "earth", "ebb", "e", "4");
            RegisterClass(5, "bombardier", "bomba", "b", "5");
            RegisterClass(6, "tiger", "t", "6");
            RegisterClass(7, "click", "c", "7");
            RegisterClass(8, "fungus", "f", "fung", "8");
            RegisterClass(9, "firefly", "ff", "9");
            RegisterClass(10, "cyborg", "cy", "borg", "10");
            RegisterClass(11, "weevil", "longnosed", "w", "11");
            RegisterClass(12, "potato", "p", "12");
        }
        
        private void RegisterClass(int id, string canonicalName, params string[] aliases)
        {
            _nameToId[canonicalName.ToLowerInvariant()] = id;
            
            foreach (var alias in aliases)
            {
                _nameToId[alias.ToLowerInvariant()] = id;
            }
        }
        
        public bool TryGetId(string name, out int id)
        {
            if (_nameToId.TryGetValue(name.ToLowerInvariant(), out id))
                return true;

            id = -1; //not found
            return false;
        }
    }
}