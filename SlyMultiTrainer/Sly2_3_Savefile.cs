namespace SlyMultiTrainer
{
    public class Sly2_3_Savefile
    {
        public SAVEFILE_VERSION Version;
        public string SavefileStartAddress = "";
        public string SavefileKeyAddressTablePointer = "";
        public string SavefileKeyStringTablePointer = "";
        public Dictionary<int, int> SavefileKeyAddressTable; // Id + SubId combined, absolute address
        public Dictionary<short, string> SavefileKeyStringTable;

        private Memory.Mem _m;

        public Sly2_3_Savefile(Memory.Mem m)
        {
            _m = m;
        }

        public void SetVersion(SAVEFILE_VERSION version)
        {
            Version = version;
        }

        public void Init()
        {
            SavefileKeyAddressTable = ReadSavefileKeyAddressTable();
            SavefileKeyStringTable = ReadSavefileKeyStringTable();
        }

        public Dictionary<int, int> ReadSavefileKeyAddressTable()
        {
            Dictionary<int, int> table = new();

            int tableStart = _m.ReadInt($"{SavefileKeyAddressTablePointer}");
            if (tableStart == 0)
            {
                return table;
            }

            int offset1 = 0;
            short nextIdOffset = 1;
            do
            {
                int current = tableStart + nextIdOffset * 0xA;
                short id = _m.ReadShort($"{current:X}");
                short length = _m.ReadShort($"{current + 4:X}"); // ntsc e3 demo 001CBAC4
                short childOffset = _m.ReadShort($"{current + 6:X}");
                nextIdOffset = _m.ReadShort($"{current + 8:X}");

                int offset2 = 0;
                while (childOffset != 0)
                {
                    current = tableStart + childOffset * 0xA;

                    // NOTE: Instead of adding the offset to the dictionary,
                    // we already convert it to an absolute address by adding the offset to SavefileStartAddress
                    // In sly 2 ntsc, this sum is done at 1D814C, where s3 is 3D4A60 and v0 is the offset (16A0 for t1_follow_intro)
                    short subId = _m.ReadShort($"{current:X}");
                    short savefileValueOffset = 0;
                    if (Version == SAVEFILE_VERSION.V1)
                    {
                        savefileValueOffset = _m.ReadShort($"{current + 4:X}");
                    }
                    else if (Version == SAVEFILE_VERSION.V0)
                    {
                        savefileValueOffset = (short)(offset1 + offset2);
                        offset2 += _m.ReadShort($"{current + 4:X}");
                    }

                    int address = Convert.ToInt32(SavefileStartAddress, 16) + savefileValueOffset;
                    int combined = CombineShorts(id, subId);
                    table.TryAdd(combined, address);
                    childOffset = _m.ReadShort($"{current + 8:X}");
                }

                offset1 += length;

            } while (nextIdOffset != 0);

            return table;
        }

        public Dictionary<short, string> ReadSavefileKeyStringTable()
        {
            int i = 0;
            short stringId = 0;
            Dictionary<short, string> table = new();

            while (stringId != 0x100)
            {
                stringId = (short)_m.ReadInt($"{SavefileKeyStringTablePointer},{i * 0x10 + 8:X}");
                int stringPointer = _m.ReadInt($"{SavefileKeyStringTablePointer},{i * 0x10 + 4:X}");
                string name = _m.ReadNullTerminatedString(stringPointer.ToString("X"));
                table.TryAdd(stringId, name);
                i++;
            }

            return table;
        }

        public int CombineShorts(short id, short subId)
        {
            return (id << 16) | ((ushort)subId);
        }

        public (short, short) SplitIntToShorts(int combined)
        {
            short id = (short)(combined >> 16);
            short subId = (short)(combined & 0xFFFF);
            return (id, subId);
        }

        public T ReadSavefileValue<T>(string name, string property)
        {
            int addr = GetSavefileAddress(name, property);
            return _m.ReadMemory<T>(addr.ToString("X"));
        }

        public void WriteSavefileValue(string name, string property, string dataType, string value)
        {
            int addr = GetSavefileAddress(name, property);
            _m.WriteMemory(addr.ToString("X"), dataType, value);
        }

        public int GetSavefileAddress(string group, string property)
        {
            short id = SavefileKeyStringTable.FirstOrDefault(x => x.Value == group).Key;
            short subId = SavefileKeyStringTable.FirstOrDefault(x => x.Value == property).Key;
            var addr = GetSavefileAddress(id, subId);
            return addr;
        }

        public int GetSavefileAddress(short id, short subId)
        {
            int combined = CombineShorts(id, subId);
            SavefileKeyAddressTable.TryGetValue(combined, out int address);
            return address;
        }

        public List<(int, string)> GetSavefileKeyAddressTable(bool ordered)
        {
            List<(int address, string text)> list = new(SavefileKeyAddressTable.Count);
            foreach (var entry in SavefileKeyAddressTable)
            {
                var (id, subId) = SplitIntToShorts(entry.Key);
                string name = SavefileKeyStringTable.GetValueOrDefault(id);
                string property = SavefileKeyStringTable.GetValueOrDefault(subId);
                list.Add((entry.Value, $"{entry.Key:X8} - {entry.Value:X} - {name} - {property}"));
            }

            // Order the list by address
            if (ordered)
            {
                list = list.OrderBy(x => x.address).ToList();
            }

            return list;
        }

        public enum SAVEFILE_VERSION
        {
            V0 = 0, // Sly 2 ntsc e3 demo, march
            V1 = 1 // Sly 2/3 retail
        }
    }
}
