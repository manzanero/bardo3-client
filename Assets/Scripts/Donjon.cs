using System.Collections.Generic;
using Newtonsoft.Json;

public class Donjon
{
    public class MapModel
    {
        [JsonProperty("cell_bit")] public CellBit cellBit;
        public List<List<string>> cells;
        [JsonProperty("corridor_features")] public Dictionary<string, CorridorFeature> corridorFeatures;
        public Details details;
        public List<Egress> egress;
        public List<Room> rooms;
        public List<Stair> stairs;
        public MapSettings settings;
        [JsonProperty("wandering_monsters")] public Dictionary<int, string> wanderingMonsters;

        public class CellBit
        {
            public string aperture;
            public string arch;
            public string block;
            public string corridor;
            public string door;
            public string label;
            public string locked;
            public string nothing;
            public string perimeter;
            public string portcullis;
            public string room;
            [JsonProperty("room_id")] public string roomId;
            public string secret;
            [JsonProperty("stair_down")] public string stairDown;
            [JsonProperty("stair_up")] public string stairUp;
            public string trapped;
        }

        public class CorridorFeature
        {
            public string key;
            public string summary;
            public string detail;
            public List<Mark> marks;

            public class Mark
            {
                public int col;
                public int row;
            }
        }

        public class Details
        {
            public string floor;
            public string history;
            public string illumination;
            public string special;
            public string temperature;
            public string walls;
        }

        public class Egress
        {
            public int col;
            public int row;
            public string dir;
            public int depth;
            public string type;
            [JsonProperty("room_id")] public string roomId;
        }

        public class Room
        {
            public string id;
            public int col;
            public int row;
            public int south;
            public int north;
            public int east;
            public int west;
            public int height;
            public int width;
            public int area;
            public string shape;
            public int polygon;
            public string size;
            public Contents contents;
            public Doors doors;

            public class Contents
            {
                public string summary;
                public string inhabited;
                public Detail detail;

                public class Detail
                {
                    public List<string> monster;
                    [JsonProperty("hidden_treasure")] public List<string> hiddenTreasure;
                    public List<string> trap;
                    [JsonProperty("room_features")] public string roomFeatures;
                }
            }

            public class Doors
            {
                public List<Door> south;
                public List<Door> north;
                public List<Door> east;
                public List<Door> west;

                public class Door
                {
                    public int col;
                    public int row;
                    [JsonProperty("desc")] public string description;
                    public string type;
                    public string trap;
                    [JsonProperty("out_id")] public string outId;
                }
            }
        }

        public class MapSettings
        {
            [JsonProperty("add_stairs")] public string addStairs;
            public int bleed;
            [JsonProperty("cell_size")] public int cellSize;
            [JsonProperty("corridor_layout")] public string corridorLayout;
            [JsonProperty("door_set")] public string doorSet;
            [JsonProperty("dungeon_layout")] public string dungeonLayout;
            [JsonProperty("dungeon_size")] public string dungeonSize;
            public string grid;
            [JsonProperty("image_size")] public string imageSize;
            public string infest;
            [JsonProperty("last_room_id")] public int lastRoomId;
            public int level;
            [JsonProperty("map_cols")] public int mapCols;
            [JsonProperty("map_rows")] public int mapRows;
            [JsonProperty("map_style")] public string mapStyle;
            [JsonProperty("max_col")] public int maxCol;
            [JsonProperty("max_row")] public int maxRow;
            public string motif;
            [JsonProperty("n_cols")] public int nCols;
            [JsonProperty("n_i")] public int nI;
            [JsonProperty("n_j")] public int nJ;
            [JsonProperty("n_pc")] public int nPC;
            [JsonProperty("n_rooms")] public int nRooms;
            [JsonProperty("n_rows")] public int nRows;
            public string name;
            [JsonProperty("peripheral_egress")] public string peripheralEgress;
            [JsonProperty("remove_arcs")] public string removeArcs;
            [JsonProperty("remove_deadends")] public string removeDeadends;
            [JsonProperty("room_layout")] public string roomLayout;
            [JsonProperty("room_polymorph")] public string roomPolymorph;
            [JsonProperty("room_size")] public string roomSize;
            public int seed;
        }

        public class Stair
        {
            public int col;
            public int row;
            public string key;
            public string dir;
        }
    }
}