using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class KeyState {

        List<char> Keys = new List<char>();
        
        public KeyState() {}

        public KeyState(IEnumerable<char> keys) {
            Keys.AddRange(keys);
            Keys.Sort();
        }

        public bool PossessesKey(char key) {
            return Keys.Contains(key);
        }

        public KeyState Add(char key) {
            if (Keys.Contains(key)) return this;

            // Otherwise, create copy and add key
            // (this obj is immutable)
            var keys = new List<char>(Keys);
            keys.Add(key);

            return new KeyState(keys);
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            KeyState k = (KeyState) obj;
            if (k.Keys.Count != this.Keys.Count) return false;
            for (int i = 0; i < Keys.Count; i++) if (k.Keys[i] != this.Keys[i]) return false;
            
            return true;
        }
        
        override public int GetHashCode() {
            return ToString().GetHashCode();
        }

        override public string ToString() {
            return String.Join("", Keys);
        }
    }
}
