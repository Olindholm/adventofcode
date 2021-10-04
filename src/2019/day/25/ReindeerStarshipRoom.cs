using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class ReindeerStarshipRoom {

        string[] FORBIDDEN_ITEMS = {
            "giant electromagnet",
            "escape pod",
            "molten lava",
            "photons",
            "infinite loop",
        };

        string Name;
        string Description;
        List<string> Directions;
        List<string> Items;
        public ReindeerStarshipRoom(string name, string description, IEnumerable<string> directions, IEnumerable<string> items) {
            Name = name;
            Description = description;
            Directions = directions.ToList();
            Items = items.ToList();
        }

        public string GetName() {
            return Name;
        }

        public bool HasItems() {
            return GetItems().Count() > 0;
        }

        public IEnumerable<string> GetItems() {
            return Items.Except(FORBIDDEN_ITEMS);
        }

        public bool HasDoorTo(string direction) {
            return Directions.Contains(direction);
        }

        override public bool Equals(object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            ReindeerStarshipRoom that = (ReindeerStarshipRoom) obj;
            if (!that.GetName().Equals(this.GetName())) return false;

            return true;
        }

        override public int GetHashCode() {
            return GetName().GetHashCode();
        }

        override public string ToString() {
            return String.Format("Name: {0}\nDescription: {1}\nDirections: {2}\nItems: {3}", Name, Description, String.Join(", ", Directions), String.Join(", ", Items));
        }

        public static ReindeerStarshipRoom Parse(string str) {
            Match match;
            MatchCollection matches;

            match = Regex.Match(str, "== ([A-z0-9 ]+) ==\n([A-z0-9., -]+)");
            string name = match.Groups[1].ToString();
            string description = match.Groups[2].ToString();

            if (String.IsNullOrEmpty(name))
                throw new Exception("No starship room!: " + str);

            matches = Regex.Matches(str, "- (north|south|east|west)");
            var directions = matches.Select(m => m.Groups[1].ToString());

            var items = Enumerable.Empty<string>();
            int index = str.IndexOf("Items here:");
            if (index > 0) {
                matches = Regex.Matches(str.Substring(index), "- ([A-z0-9 -]+)");
                items = items.Concat(matches.Select(m => m.Groups[1].ToString()));
            }

            return new ReindeerStarshipRoom(name, description, directions, items);
        }
    }
}
