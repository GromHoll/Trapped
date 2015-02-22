using System;
using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using TrappedGame.Model.Elements;
using TrappedGame.Utils;

namespace TrappedGame.Model.LevelUtils {
    public class LevelBuilder {

        private string name;

        public string TutorialName { private get; set; }
        public string TutorialMessage { private get; set; }
        public Tutorial LevelTutorial {
            get {
                if (TutorialName != null && TutorialMessage != null) {
                    return new Tutorial { Name = TutorialName, Message = TutorialMessage };
                }
                return null;
            }
        }

        private readonly IntVector2 size;
        private readonly Cell[,] cells;
        
        private IntVector2 start;
        private IntVector2 finish;

        private readonly IList<Platform> platforms   = new List<Platform>();
        private readonly IList<IntVector2> bonuses = new List<IntVector2>();
        private readonly IDictionary<IntVector2, LevelTick> timeBonuses = new Dictionary<IntVector2, LevelTick>();
        private readonly IDictionary<String, PortalCell> portalsWithoutPair = new Dictionary<String, PortalCell>();
        private readonly IDictionary<String, IList<DoorCell>> doors = new Dictionary<String, IList<DoorCell>>();
        private readonly IDictionary<String, IList<Key>> keys = new Dictionary<String, IList<Key>>();
        
        public LevelBuilder(string name, int xSize, int ySize) {
            Validate.CheckArgument(xSize > 0, "xSize should be positive");
            Validate.CheckArgument(ySize > 0, "ySize should be positive");
            
            size = new IntVector2(xSize, ySize);
            cells = new Cell[size.x, size.y];
            for (var x = 0; x < size.x; x++) {
                for (var y = 0; y < size.y; y++) {
                    AddCell(new UnknownCell(x, y));
                }
            }
        }

        public void AddCell(Cell cell) {
            var x = cell.X;
            var y = cell.Y;
            cells[x, y] = cell;
        }

        public void AddBonus(IntVector2 coordinate) {
            bonuses.Add(coordinate);
        }
        
        public void AddTimeBonus(IntVector2 coordinate, LevelTick levelTick) {
            timeBonuses.Add(coordinate, levelTick);
        }

        public void AddPlatform(IntVector2 coordinate) {
            platforms.Add(new Platform(coordinate.x, coordinate.y));    
        }

        public void AddPortal(IntVector2 coordinate, string key) {
            PortalCell newPortal;
            if (portalsWithoutPair.ContainsKey(key)) {
                var oldPortal = portalsWithoutPair[key];
                portalsWithoutPair.Remove(key);
                newPortal = new PortalCell(coordinate.x, coordinate.y, oldPortal);
            } else {
                newPortal = new PortalCell(coordinate.x, coordinate.y);
                portalsWithoutPair[key] = newPortal;
            }
            AddCell(newPortal);
        }

        public void AddDoor(IntVector2 coordinate, string doorKey) {
            // TODO Maybe create miltimap class?
            var doorList = doors.ContainsKey(doorKey) ? doors[doorKey] : doors[doorKey] = new List<DoorCell>();
            var door = new DoorCell(coordinate.x, coordinate.y);
            doorList.Add(door);

            if (keys.ContainsKey(doorKey)) {
                var keysList = keys[doorKey];
                foreach (var key in keysList) {
                    door.AddKey(key);
                }
            }
            AddCell(door);
        }

        public void AddKey(IntVector2 coordinate, string doorKey) {
            // TODO Maybe create miltimap class?
            var keyList = keys.ContainsKey(doorKey) ? keys[doorKey] : keys[doorKey] = new List<Key>();
            var key = new Key(coordinate.x, coordinate.y);
            keyList.Add(key);

            if (doors.ContainsKey(doorKey)) {
                var doorsList = doors[doorKey];
                foreach (var door in doorsList) {
                    door.AddKey(key);
                }
            }
        }

        public void SetStart(int x, int y) {
            start.Set(x, y);
        }
        
        public void SetFinish(int x, int y) {
            finish.Set(x, y);
        }
        
        public IntVector2 GetSize() {
            return size;
        }

        public IntVector2 GetStart() {
            return start;
        }

        public IntVector2 GetFinish() {
            return finish;
        }

        public Cell[,] GetCells() {
            return cells;
        }
        
        public IList<IntVector2> GetBonuses() {
            return bonuses;
        }

        public IList<Platform> GetPlatforms() {
            return platforms;
        }

        public IList<Key> GetKeys() {
            var allKeys = new List<Key>();
            foreach (var keysList in keys.Values) {
                allKeys.AddRange(keysList);   
            }
            return allKeys;
        }

        public IDictionary<IntVector2, LevelTick> GetTimeBonuses() {
            return timeBonuses;
        }

        public Level Build() {
            return new Level(this);
        }              
    }
}