using System.Numerics;
using System.Text;
using static SlyMultiTrainer.Sly2_3_Savefile;
using static SlyMultiTrainer.Util;

namespace SlyMultiTrainer
{
    public class Sly2Handler : GameBase_t
    {
        public string ReloadAddress = "";
        public string ReloadValuesPointer = "";
        public string FKXListCount = "";
        public string ActiveCharacterPointer = "";
        public string ActiveCharacterIdAddress = "";
        public string ActiveCharacterHealthAddress = "";
        public string StringTableCountAddress = "";
        public string IsLoadingAddress = "";
        public DAG_t DAG;
        public Sly2_3_Savefile Savefile;

        private string _offsetTransformationOrigin = "54";
        private string _offsetTransformationLocal = "";
        private string _offsetTransformationWorld = "";
        private string _offsetController = "150";
        private string _offsetControllerBinds = "30";
        private string _offsetInvulnerable = "298";
        private string _offsetInfiniteDbJump = "2E8";
        private string _offsetSpeedMultiplier = "2F8";
        private string _offsetSavefileHealth = "E00";
        private string _offsetSavefileGadgetPower = "";
        private string _offsetGadgetBinds = "1180";
        private string _offsetUndetectable = "11AC";

        private Memory.Mem _m;
        private Encoding _encoding;

        public Sly2Handler(Memory.Mem m, Form1 form, string region) : base(m, form, region)
        {
            _m = m;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _encoding = Encoding.GetEncoding(1252);
            DAG = new(m);
            DAG.SetVersion(DAG_VERSION.V1);
            Savefile = new(m);
            Savefile.SetVersion(SAVEFILE_VERSION.V1);

            DAG.OffsetNextNodePointer = "20";
            DAG.OffsetState = "54";
            DAG.OffsetGoalDescription = "5C";
            DAG.OffsetFocusCount = "64";
            DAG.OffsetCompleteCount = "68";
            DAG.OffsetMissionName = "6C";
            DAG.OffsetMissionDescription = "70";
            DAG.OffsetClusterPointer = "7C";
            DAG.OffsetChildrenCount = "A0";
            DAG.OffsetCheckpointEntranceValue = "B8";
            DAG.OffsetAttributes = "C8";
            DAG.OffsetAttributesForCluster = "D0";
            DAG.GetStringFromId = GetStringFromId;
            DAG.LoadMap = LoadMap;
            DAG.WriteActCharId = WriteActCharId;

            if (region == "NTSC")
            {
                ReloadAddress = "3E1080";
                ReloadValuesPointer = "3E1C40";
                FKXListCount = "3E1394";
                ClockAddress = "2DDED8";
                CoinsAddress = "3D4B00";
                GadgetAddress = "3D4AF8";
                DrawDistanceAddress = "2DDF5C";
                FOVAddress = "2DDF64";
                ResetCameraAddress = "2DE240";
                MapIdAddress = "3E1110";
                GuardAIAddress = "3E1214";
                ActiveCharacterPointer = "3E138C";
                ActiveCharacterIdAddress = "3D4A6C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3E0B04";
                DAG.CurrentCheckpointNodePointer = "3E0FA4";
                DAG.ClusterIdAddress = "2DEB40";
                Savefile.SavefileStartAddress = "3D4A60";
                Savefile.SavefileKeyAddressTablePointer = "3E0EAC";
                Savefile.SavefileKeyStringTablePointer = "3E0F88";
                StringTableCountAddress = "3E1AD0";
                IsLoadingAddress = "3D3980";
            }
            else if (region == "PAL (v1.00)"
                  || region == "PAL (v2.01)"
                  || region == "PAL September 11")
            {
                ReloadAddress = "3E8880";
                ReloadValuesPointer = "3E9430";
                FKXListCount = "3E8B94";
                ClockAddress = "2E52D8";
                CoinsAddress = "3DC300";
                GadgetAddress = "3DC2F8";
                DrawDistanceAddress = "2E535C";
                FOVAddress = "2E5364";
                ResetCameraAddress = "2E5640";
                MapIdAddress = "3E8910";
                GuardAIAddress = "3E8A14";
                ActiveCharacterPointer = "3E8B8C";
                ActiveCharacterIdAddress = "3DC26C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3E8304";
                DAG.CurrentCheckpointNodePointer = "3E87A4";
                DAG.ClusterIdAddress = "2E5F40";
                Savefile.SavefileStartAddress = "3DC260";
                Savefile.SavefileKeyAddressTablePointer = "3E86AC";
                Savefile.SavefileKeyStringTablePointer = "3E8788";
                StringTableCountAddress = "3E92D0";
                IsLoadingAddress = "3DB180";
            }
            else if (region == "NTSC-J")
            {
                _encoding = Encoding.Unicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DF0";
                _offsetController = "140";
                _offsetInfiniteDbJump = "2D8";
                _offsetSpeedMultiplier = "2E8";
                _offsetInvulnerable = "288";
                _offsetGadgetBinds = "1170";
                _offsetUndetectable = "119C";

                ReloadAddress = "3EAA80";
                ReloadValuesPointer = "3EB630";
                FKXListCount = "3EAD94";
                ClockAddress = "2E7158";
                CoinsAddress = "3DE300";
                GadgetAddress = "3DE2F8";
                DrawDistanceAddress = "2E71DC";
                FOVAddress = "2E71E4";
                ResetCameraAddress = "2E74C0";
                MapIdAddress = "3EAB10";
                GuardAIAddress = "3EAC14";
                ActiveCharacterPointer = "3EAD8C";
                ActiveCharacterIdAddress = "3DE26C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3EA304";
                DAG.CurrentCheckpointNodePointer = "3EA9A4";
                DAG.ClusterIdAddress = "2E7DC0";
                Savefile.SavefileStartAddress = "3DE260";
                Savefile.SavefileKeyAddressTablePointer = "3EA8AC";
                Savefile.SavefileKeyStringTablePointer = "3EA988";
                StringTableCountAddress = "3EB4D0";
                IsLoadingAddress = "3DD180";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";
            }
            else if (region == "NTSC-K")
            {
                _encoding = Encoding.Unicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DF0";
                _offsetController = "140";
                _offsetInfiniteDbJump = "2D8";
                _offsetSpeedMultiplier = "2E8";
                _offsetInvulnerable = "288";
                _offsetGadgetBinds = "1170";
                _offsetUndetectable = "119C";

                ReloadAddress = "3EA100";
                ReloadValuesPointer = "3EACB0";
                FKXListCount = "3EA414";
                ClockAddress = "2E6758";
                CoinsAddress = "3DD980";
                GadgetAddress = "3DD978";
                DrawDistanceAddress = "2E67DC";
                FOVAddress = "2E67E4";
                ResetCameraAddress = "2E6AC0";
                MapIdAddress = "3EA190";
                GuardAIAddress = "3EA294";
                ActiveCharacterPointer = "3EA40C";
                ActiveCharacterIdAddress = "3DD8EC";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3E9984";
                DAG.CurrentCheckpointNodePointer = "3EA024";
                DAG.ClusterIdAddress = "2E73C0";
                Savefile.SavefileStartAddress = "3DD8E0";
                Savefile.SavefileKeyAddressTablePointer = "3E9F2C";
                Savefile.SavefileKeyStringTablePointer = "3EA008";
                StringTableCountAddress = "3EAB50";
                IsLoadingAddress = "3DC800";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";
            }
            else if (region == "NTSC E3 Demo")
            {
                DAG.SetVersion(DAG_VERSION.V0);
                Savefile.SetVersion(SAVEFILE_VERSION.V0);
                _offsetSavefileHealth = "FD0";
                _offsetController = "140";
                _offsetInfiniteDbJump = "338";
                _offsetSpeedMultiplier = "344";

                DAG.OffsetClusterPointer = "74";
                DAG.OffsetChildrenCount = "94";
                DAG.OffsetCheckpointEntranceValue = "AC";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "BC";

                ReloadAddress = "39A860";
                ReloadValuesPointer = "39CEC4";
                FKXListCount = "39AB34";
                ClockAddress = "2D1F58";
                CoinsAddress = "2D2B08";
                DrawDistanceAddress = "2D1FDC";
                FOVAddress = "2D1FE4";
                ResetCameraAddress = "2D2324";
                MapIdAddress = "39A8F0";
                GuardAIAddress = "";
                ActiveCharacterPointer = "39AB2C";
                ActiveCharacterIdAddress = "3CA7C2";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth}";
                DAG.RootNodePointer = "39A3E8";
                DAG.CurrentCheckpointNodePointer = "39A854";
                DAG.ClusterIdAddress = "2D2B20";
                Savefile.SavefileStartAddress = "3CA7C0";
                Savefile.SavefileKeyAddressTablePointer = "39A75C";
                Savefile.SavefileKeyStringTablePointer = "39A838";
                StringTableCountAddress = "39CE64";
                IsLoadingAddress = "393700";

                var tmp = Maps[1];
                Maps.RemoveAt(1);
                Maps.Insert(0, tmp);
                Maps[0].IsVisible = false;
                Maps[3].IsVisible = false;
                Maps[5].IsVisible = false;
                Maps.Skip(7).ToList().ForEach(m => m.IsVisible = false);

                // nightclub door entrance
                Maps[4].Warps[0].Position = new(-4200, 5400, -200);
            }
            else if (region == "NTSC PlayStation Magazine Demo Disc 089")
            {
                _offsetSavefileHealth = "E60";
                _offsetController = "160";
                _offsetInfiniteDbJump = "348";
                _offsetSpeedMultiplier = "354";
                //_offsetInvulnerable = "298"; // TO FIND
                _offsetGadgetBinds = "11E0";
                //_offsetUndetectable = "11AC"; // TO FIND

                ReloadAddress = "3F0EC8";
                ReloadValuesPointer = "3F1A80";
                FKXListCount = "3F11B4";
                ClockAddress = "302ED8";
                CoinsAddress = "3E494C";
                GadgetAddress = "3E4944";
                DrawDistanceAddress = "302F5C";
                FOVAddress = "302F64";
                ResetCameraAddress = "303240";
                MapIdAddress = "3F0F58";
                GuardAIAddress = "3F105C";
                ActiveCharacterPointer = "3F11AC";
                ActiveCharacterIdAddress = "3E48BC";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3F0958";
                DAG.CurrentCheckpointNodePointer = "3F0E04";
                DAG.ClusterIdAddress = "303B2C";
                Savefile.SavefileStartAddress = "3E48B0";
                Savefile.SavefileKeyAddressTablePointer = "3F0D0C";
                Savefile.SavefileKeyStringTablePointer = "3F0DE8";
                StringTableCountAddress = "3F1920";
                IsLoadingAddress = "3E4800";

                Maps[1].IsVisible = false; // dvd_menu
                Maps[3].IsVisible = false; // wine cellar
                Maps[5].IsVisible = false; // print room
                Maps.Skip(7).ToList().ForEach(m => m.IsVisible = false);

                //Gadgets[0].RemoveRange(18, 2); // remove tom and timerush
            }
            else if (region == "NTSC July 11")
            {
                _offsetSavefileHealth = "1040";
                _offsetController = "160";
                _offsetInfiniteDbJump = "348";
                _offsetSpeedMultiplier = "354";
                //_offsetInvulnerable = "298"; // TO FIND
                _offsetGadgetBinds = "1390";
                //_offsetUndetectable = "11AC"; // TO FIND

                ReloadAddress = "3FBF60";
                ReloadValuesPointer = "3FE6B0";
                FKXListCount = "3FC244";
                ClockAddress = "2F9A28";
                CoinsAddress = "3EF6C0";
                GadgetAddress = "3EF6B8";
                DrawDistanceAddress = "2F9AAC";
                FOVAddress = "2F9AB4";
                ResetCameraAddress = "2F9D90";
                MapIdAddress = "3EF628";
                GuardAIAddress = "3FC0F0";
                ActiveCharacterPointer = "3FC23C";
                ActiveCharacterIdAddress = "3EF62C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3FBAB4";
                DAG.CurrentCheckpointNodePointer = "3FBF34";
                DAG.ClusterIdAddress = "2FA688";
                Savefile.SavefileStartAddress = "3EF620";
                Savefile.SavefileKeyAddressTablePointer = "3FBE3C";
                Savefile.SavefileKeyStringTablePointer = "3FBF18";
                StringTableCountAddress = "3FE560";
                IsLoadingAddress = "3EF600";

                DAG.OffsetAttributes = "C4";
                DAG.OffsetAttributesForCluster = "AC";

                // Remove ep8, vault room and dvd menu
                Maps.RemoveRange(38, 6);
                Maps.RemoveRange(16, 1);
                Maps.RemoveRange(1, 1);

                Maps.Insert(0, new("Splash", new(), false));
                Maps.Insert(12, new($"{SubMapNamePrefix}i_palace_heist", new(), false));
                Maps.Insert(15, new($"{SubMapNamePrefix}i_temple_heist", new(), false));
                Maps.Insert(18, new($"{SubMapNamePrefix}p_prison_heist", new(), false));

                for (int i = 0; i < 3; i++)
                {
                    Gadgets[i].Insert(1, new("Clue Finder", 0x6));
                }
            }
            else if (region == "PAL August 2")
            {
                _offsetSavefileHealth = "E10";
                _offsetInfiniteDbJump = "2F8";
                _offsetInvulnerable = "2A8";
                _offsetUndetectable = "11BC";
                _offsetController = "160";
                _offsetSpeedMultiplier = "308";
                _offsetGadgetBinds = "1190";

                ReloadAddress = "3F53E8";
                ReloadValuesPointer = "3F5F90";
                FKXListCount = "3F56D4";
                ClockAddress = "2F3258";
                CoinsAddress = "3E8E2C";
                GadgetAddress = "3E8E24";
                DrawDistanceAddress = "2F32DC";
                FOVAddress = "2F32E4";
                ResetCameraAddress = "2F35C0";
                MapIdAddress = "3F5478";
                GuardAIAddress = "3F557C";
                ActiveCharacterPointer = "3F56CC";
                ActiveCharacterIdAddress = "3E8D9C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "3F4E7C";
                DAG.CurrentCheckpointNodePointer = "3F5324";
                DAG.ClusterIdAddress = "2F3EB0";
                Savefile.SavefileStartAddress = "3E8D90";
                Savefile.SavefileKeyAddressTablePointer = "3F522C";
                Savefile.SavefileKeyStringTablePointer = "3F5308";
                StringTableCountAddress = "3F5E40";
                IsLoadingAddress = "3E8D00";

                // Remove ep8
                Maps.RemoveRange(38, 6);

                Maps.Insert(12, new($"{SubMapNamePrefix}i_palace_heist", new(), false));
                Maps.Insert(15, new($"{SubMapNamePrefix}i_temple_heist", new(), false));
                Maps.Insert(18, new($"{SubMapNamePrefix}p_prison_heist", new(), false));
            }
            else if (region == "NTSC March 17")
            {
                DAG.SetVersion(DAG_VERSION.V0);
                Savefile.SetVersion(SAVEFILE_VERSION.V0);
                _offsetInfiniteDbJump = "328";
                _offsetSavefileHealth = "ED0";
                _offsetController = "140";
                _offsetSpeedMultiplier = "334";
                //_offsetInvulnerable = "298";
                //_offsetUndetectable = "11AC";

                ReloadAddress = "3EE978";
                ReloadValuesPointer = "3F0F34"; // 325FF8
                FKXListCount = "3EEBF8";
                ClockAddress = "303E18";
                CoinsAddress = "304A04";
                GadgetAddress = "";
                DrawDistanceAddress = "303E9C";
                FOVAddress = "303EA4";
                ResetCameraAddress = "3041F4";
                MapIdAddress = "3EEA08";
                GuardAIAddress = "3EEB00";
                ActiveCharacterPointer = "3EEBF0";
                ActiveCharacterIdAddress = "41FC82";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth}";
                DAG.RootNodePointer = "3EE52C";
                DAG.CurrentCheckpointNodePointer = "3EE974";
                DAG.ClusterIdAddress = "304A1C";
                Savefile.SavefileStartAddress = "41FC80";
                Savefile.SavefileKeyAddressTablePointer = "3EE87C";
                Savefile.SavefileKeyStringTablePointer = "3EE958";
                StringTableCountAddress = "3F0F14";
                IsLoadingAddress = "3E7380";

                DAG.OffsetNextNodePointer = "20";
                DAG.OffsetState = "54";
                DAG.OffsetGoalDescription = "5C";
                DAG.OffsetMissionName = "BC";
                DAG.OffsetMissionDescription = "70";
                DAG.OffsetClusterPointer = "74";
                DAG.OffsetChildrenCount = "94";
                DAG.OffsetCheckpointEntranceValue = "AC";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "B8";

                // Remove ep8, vault room and dvd menu
                Maps.RemoveRange(14, 30);
                Maps.RemoveAt(1);
                Maps[0].IsVisible = false; // cairo

                Maps.Insert(0, new("Splash", new() { new() }));
                Maps.Insert(12, new($"{SubMapNamePrefix}i_palace_heist", new(), false));

                // nightclub door entrance
                Maps[4].Warps[0].Position = new(-4200, 5400, -200);
            }
            else if (region == "NTSC (PS3)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DE0";
                _offsetController = "140";
                _offsetControllerBinds = "32";
                _offsetInfiniteDbJump = "2C8";
                _offsetSpeedMultiplier = "2D8";
                _offsetInvulnerable = "278";
                _offsetUndetectable = "118C";
                _offsetGadgetBinds = "1160";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";

                ReloadAddress = "74134C";
                ReloadValuesPointer = "741EE0";
                FKXListCount = "741654";
                ClockAddress = "428A80";
                CoinsAddress = "734AAC";
                GadgetAddress = "734AA4";
                DrawDistanceAddress = "428B4C";
                FOVAddress = "428B54";
                ResetCameraAddress = "428E30";
                MapIdAddress = "7413DC";
                GuardAIAddress = "7414E0";
                ActiveCharacterPointer = "74164C";
                ActiveCharacterIdAddress = "734A0C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "740BC4";
                DAG.CurrentCheckpointNodePointer = "741264";
                DAG.ClusterIdAddress = "4896F4";
                Savefile.SavefileStartAddress = "734A00";
                Savefile.SavefileKeyAddressTablePointer = "74116C";
                Savefile.SavefileKeyStringTablePointer = "741248";
                StringTableCountAddress = "741D90";
                IsLoadingAddress = "733900";
            }
            else if (region == "PAL (PS3)"
                  || region == "UK (PS3)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DE0";
                _offsetController = "140";
                _offsetControllerBinds = "32";
                _offsetInfiniteDbJump = "2C8";
                _offsetSpeedMultiplier = "2D8";
                _offsetInvulnerable = "278";
                _offsetUndetectable = "118C";
                _offsetGadgetBinds = "1160";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";

                ReloadAddress = "74124C";
                ReloadValuesPointer = "741DE0";
                FKXListCount = "741554";
                ClockAddress = "428980";
                CoinsAddress = "7349AC";
                GadgetAddress = "7349A4";
                DrawDistanceAddress = "428A4C";
                FOVAddress = "428A54";
                ResetCameraAddress = "428D30";
                MapIdAddress = "7412DC";
                GuardAIAddress = "7413E0";
                ActiveCharacterPointer = "74154C";
                ActiveCharacterIdAddress = "73490C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "740AC4";
                DAG.CurrentCheckpointNodePointer = "741164";
                DAG.ClusterIdAddress = "4895F4";
                Savefile.SavefileStartAddress = "734900";
                Savefile.SavefileKeyAddressTablePointer = "74106C";
                Savefile.SavefileKeyStringTablePointer = "741148";
                StringTableCountAddress = "741C90";
                IsLoadingAddress = "733800";
            }
            else if (region == "NTSC-J (PS3)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DE0";
                _offsetController = "140";
                _offsetControllerBinds = "32";
                _offsetInfiniteDbJump = "2C8";
                _offsetSpeedMultiplier = "2D8";
                _offsetInvulnerable = "278";
                _offsetUndetectable = "118C";
                _offsetGadgetBinds = "1160";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";

                ReloadAddress = "7412CC";
                ReloadValuesPointer = "741E60";
                FKXListCount = "7415D4";
                ClockAddress = "428A00";
                CoinsAddress = "734A2C";
                GadgetAddress = "734A24";
                DrawDistanceAddress = "428ACC";
                FOVAddress = "428AD4";
                ResetCameraAddress = "428DB0";
                MapIdAddress = "74135C";
                GuardAIAddress = "741460";
                ActiveCharacterPointer = "7415CC";
                ActiveCharacterIdAddress = "73498C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "740B44";
                DAG.CurrentCheckpointNodePointer = "7411E4";
                DAG.ClusterIdAddress = "489674";
                Savefile.SavefileStartAddress = "734980";
                Savefile.SavefileKeyAddressTablePointer = "7410EC";
                Savefile.SavefileKeyStringTablePointer = "7411A0";
                StringTableCountAddress = "741D10";
                IsLoadingAddress = "733880";
            }
            else if (region == "NTSC (PS3 PSN)"
                  || region == "NTSC-K (PS3 PSN)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DE0";
                _offsetController = "140";
                _offsetControllerBinds = "32";
                _offsetInfiniteDbJump = "2C8";
                _offsetSpeedMultiplier = "2D8";
                _offsetInvulnerable = "278";
                _offsetUndetectable = "118C";
                _offsetGadgetBinds = "1160";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";

                ReloadAddress = "7B4C50";
                ReloadValuesPointer = "7B57F0";
                FKXListCount = "7B4F64";
                ClockAddress = "49DF80";
                CoinsAddress = "7A83B0";
                GadgetAddress = "7A83A8";
                DrawDistanceAddress = "49E04C";
                FOVAddress = "49E054";
                ResetCameraAddress = "49E330";
                MapIdAddress = "7B4CE0";
                GuardAIAddress = "7B4DE4";
                ActiveCharacterPointer = "7B4F5C";
                ActiveCharacterIdAddress = "7A830C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "7B44C4";
                DAG.CurrentCheckpointNodePointer = "7B4B64";
                DAG.ClusterIdAddress = "4FEBF4";
                Savefile.SavefileStartAddress = "7A8300";
                Savefile.SavefileKeyAddressTablePointer = "7B4A6C";
                Savefile.SavefileKeyStringTablePointer = "7B4B48";
                StringTableCountAddress = "7B56A0";
                IsLoadingAddress = "7A7200";
            }
            else if (region == "PAL (PS3 PSN)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetTransformationOrigin = "44";
                _offsetSavefileHealth = "DE0";
                _offsetController = "140";
                _offsetControllerBinds = "32";
                _offsetInfiniteDbJump = "2C8";
                _offsetSpeedMultiplier = "2D8";
                _offsetInvulnerable = "278";
                _offsetUndetectable = "118C";
                _offsetGadgetBinds = "1160";

                DAG.OffsetState = "44";
                DAG.OffsetGoalDescription = "4C";
                DAG.OffsetFocusCount = "54";
                DAG.OffsetCompleteCount = "58";
                DAG.OffsetMissionName = "5C";
                DAG.OffsetMissionDescription = "60";
                DAG.OffsetClusterPointer = "6C";
                DAG.OffsetChildrenCount = "90";
                DAG.OffsetCheckpointEntranceValue = "A8";
                DAG.OffsetAttributes = "B8";
                DAG.OffsetAttributesForCluster = "C0";

                ReloadAddress = "7B4BD0";
                ReloadValuesPointer = "7B5770";
                FKXListCount = "7B4EE4";
                ClockAddress = "49DF00";
                CoinsAddress = "7A8330";
                GadgetAddress = "7A8328";
                DrawDistanceAddress = "49DFCC";
                FOVAddress = "49DFD4";
                ResetCameraAddress = "49E2B0";
                MapIdAddress = "7B4C60";
                GuardAIAddress = "7B4D64";
                ActiveCharacterPointer = "7B4EDC";
                ActiveCharacterIdAddress = "7A828C";
                ActiveCharacterHealthAddress = $"{ActiveCharacterPointer},{_offsetSavefileHealth},0";
                DAG.RootNodePointer = "7B4444";
                DAG.CurrentCheckpointNodePointer = "7B4AE4";
                DAG.ClusterIdAddress = "4FEB74";
                Savefile.SavefileStartAddress = "7A8280";
                Savefile.SavefileKeyAddressTablePointer = "7B49EC";
                Savefile.SavefileKeyStringTablePointer = "7B4AC8";
                StringTableCountAddress = "7B5620";
                IsLoadingAddress = "7A7180";
            }

            _offsetTransformationLocal = $"{_offsetTransformationOrigin}+4";
            _offsetTransformationWorld = $"{_offsetTransformationOrigin}+8";
            _offsetSavefileGadgetPower = $"{_offsetSavefileHealth},4";
        }

        public override void CustomTick()
        {

        }

        public override bool IsLoading()
        {
            if (_m.ReadInt(IsLoadingAddress) == 3
                && _m.ReadInt(Savefile.SavefileKeyAddressTablePointer) != 0)
            {
                return false;
            }

            return true;
        }

        #region Gadgets
        public int ReadActCharGadgetPower()
        {
            return _m.ReadInt($"{ActiveCharacterPointer},{_offsetSavefileGadgetPower}");
        }

        public void WriteActCharGadgetPower(int value)
        {
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetSavefileGadgetPower}", "int", value.ToString());
        }

        public override void FreezeActCharGadgetPower(int value)
        {
            if (value == 0)
            {
                value = ReadActCharGadgetPower();
            }

            _m.FreezeValue($"{ActiveCharacterPointer},{_offsetSavefileGadgetPower}", "int", value.ToString());
        }

        public override void UnfreezeActCharGadgetPower()
        {
            _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetSavefileGadgetPower}");
        }

        public override int ReadActCharGadgetId(GADGET_BIND bind)
        {
            return _m.ReadInt($"{ActiveCharacterPointer},{_offsetGadgetBinds}+{(int)bind * 0xC:X}");
        }

        public override void WriteActCharGadgetId(GADGET_BIND bind, int value)
        {
            // Write to character's struct so that the change is immediate
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetGadgetBinds}+{(int)bind * 0xC:X}", "int", value.ToString());
            
            // When we use an invalid gadget, the game writes -1 to +4, which makes all the other gadgets unusable
            // We write 0 to +4 to prevent this
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetGadgetBinds}+{(int)bind * 0xC + 4:X}", "int", "0");

            // Write to savefile too so that it persists after reload
            var addr = Savefile.GetSavefileAddress(ActiveCharacter.NameForSavefile, "apukCur");
            _m.WriteMemory($"{addr:X}+{(int)bind * 0x4:X}", "int", value.ToString());
        }
        #endregion

        #region Entities
        public override bool EntityHasTransformation(string pointerToEntity)
        {
            if (_m.ReadInt($"{pointerToEntity},{_offsetTransformationOrigin}") == -1
             || _m.ReadInt($"{pointerToEntity},{_offsetTransformationOrigin}") == 0)
            {
                return false;
            }

            return true;
        }

        #region Origin
        public override Matrix4x4 ReadEntityOriginTransformation(string pointerToEntity)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return Matrix4x4.Identity;
            }

            return _m.ReadMatrix4($"{pointerToEntity},{_offsetTransformationOrigin},0");
        }

        #endregion

        #region Local
        public override Matrix4x4 ReadEntityLocalTransformation(string pointerToEntity)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return Matrix4x4.Identity;
            }

            return _m.ReadMatrix4($"{pointerToEntity},{_offsetTransformationLocal},0");
        }

        public override Vector3 ReadEntityLocalTranslation(string pointerToEntity)
        {
            return ReadEntityLocalTransformation(pointerToEntity).Translation;
        }

        public override void WriteEntityLocalTranslation(string pointerToEntity, Vector3 value)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return;
            }

            _m.WriteMemory($"{pointerToEntity},{_offsetTransformationLocal},30", "vec3", value.ToString());
        }

        public override void FreezeEntityLocalTranslationX(string pointerToEntity, string value)
        {
            if (value == "")
            {
                Vector3 trans = ReadEntityLocalTranslation(pointerToEntity);
                value = trans.X.ToString();
            }

            _m.FreezeValue($"{pointerToEntity},{_offsetTransformationLocal},30", "float", value);
        }

        public override void FreezeEntityLocalTranslationY(string pointerToEntity, string value)
        {
            if (value == "")
            {
                Vector3 trans = ReadEntityLocalTranslation(pointerToEntity);
                value = trans.Y.ToString();
            }

            _m.FreezeValue($"{pointerToEntity},{_offsetTransformationLocal},34", "float", value);
        }

        public override void FreezeEntityLocalTranslationZ(string pointerToEntity, string value)
        {
            if (value == "")
            {
                Vector3 trans = ReadEntityLocalTranslation(pointerToEntity);
                value = trans.Z.ToString();
            }

            _m.FreezeValue($"{pointerToEntity},{_offsetTransformationLocal},38", "float", value);
        }

        public override void UnfreezeEntityLocalTranslationX(string pointerToEntity)
        {
            _m.UnfreezeValue($"{pointerToEntity},{_offsetTransformationLocal},30");
        }

        public override void UnfreezeEntityLocalTranslationY(string pointerToEntity)
        {
            _m.UnfreezeValue($"{pointerToEntity},{_offsetTransformationLocal},34");
        }

        public override void UnfreezeEntityLocalTranslationZ(string pointerToEntity)
        {
            _m.UnfreezeValue($"{pointerToEntity},{_offsetTransformationLocal},38");
        }

        public override float ReadEntityLocalScale(string pointerToEntity)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return 1f;
            }

            return _m.ReadFloat($"{pointerToEntity},{_offsetTransformationLocal},0");
        }

        public override void WriteEntityLocalScale(string pointerToEntity, float scale)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return;
            }

            _m.WriteMemory($"{pointerToEntity},{_offsetTransformationLocal},0", "float", scale.ToString());
            _m.WriteMemory($"{pointerToEntity},{_offsetTransformationLocal},14", "float", scale.ToString());
            _m.WriteMemory($"{pointerToEntity},{_offsetTransformationLocal},28", "float", scale.ToString());
        }
        #endregion

        #region World
        public override Matrix4x4 ReadEntityWorldTransformation(string pointerToEntity)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return Matrix4x4.Identity;
            }

            return _m.ReadMatrix4($"{pointerToEntity},{_offsetTransformationWorld},0");
        }

        public override void WriteEntityWorldTransformation(string pointerToEntity, Matrix4x4 value)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return;
            }

            _m.WriteMemory($"{pointerToEntity},{_offsetTransformationWorld},0", "mat4", value.ToString());
        }

        #region Final
        public override Vector3 ReadEntityFinalTranslation(string pointerToEntity)
        {
            if (!EntityHasTransformation(pointerToEntity))
            {
                return Vector3.Zero;
            }

            return _m.ReadVector3($"{pointerToEntity},{_offsetTransformationWorld},70");
        }
        #endregion

        #endregion

        public List<FKXEntry_t> GetFKXList()
        {
            int fkxCount = _m.ReadInt(FKXListCount);
            string fkxPointer = _m.ReadInt($"{FKXListCount}+4").ToString("X");
            List<FKXEntry_t> fkxList = new(fkxCount);
            for (int i = 0; i < fkxCount; i++)
            {
                string address = (Convert.ToInt32(fkxPointer, 16) + i * 0x6C).ToString("X");
                var data = _m.ReadBytes(address, 0x5C);
                FKXEntry_t fkx = new(address, data);
                for (int j = 0; j < fkx.Count; j++)
                {
                    fkx.EntityAddress.Add(_m.ReadInt($"{fkx.PoolPointer:X}+{j * 4:X}"));
                }

                fkxList.Add(fkx);
            }

            fkxList = fkxList.OrderBy(x => x.Name).ToList();
            return fkxList;
        }

        #endregion

        #region Active character
        public override bool IsActCharAvailable()
        {
            return _m.ReadInt(ActiveCharacterPointer) != 0;
        }

        public override string GetActCharPointer()
        {
            return ActiveCharacterPointer;
        }

        public override int ReadActCharId()
        {
            return _m.ReadInt(ActiveCharacterIdAddress);
        }

        public override void WriteActCharId(int id)
        {
            _m.WriteMemory($"{ActiveCharacterIdAddress}", "int", id.ToString());
        }

        // This method is used for loading jobs in the dag to make it compatible with the sly 3 version
        // Sly 2 only has 1 player
        public void WriteActCharId(int id, int id2)
        {
            WriteActCharId(id);
        }

        public override void FreezeActCharId(string value)
        {
            if (value == "")
            {
                value = ReadActCharId().ToString();
            }

            _m.FreezeValue($"{ActiveCharacterIdAddress}", "int", value.ToString());
        }

        public override void UnfreezeActCharId()
        {
            _m.UnfreezeValue($"{ActiveCharacterIdAddress}");
        }

        public override int ReadActCharHealth()
        {
            return _m.ReadInt(GetActCharHealthAddress());
        }

        public override void WriteActCharHealth(int value)
        {
            _m.WriteMemory(GetActCharHealthAddress(), "int", value.ToString());
        }

        public override void FreezeActCharHealth(int value)
        {
            if (value == 0)
            {
                value = ReadActCharHealth();
            }

            _m.FreezeValue(GetActCharHealthAddress(), "int", value.ToString());
        }

        public override void UnfreezeActCharHealth()
        {
            _m.UnfreezeValue(GetActCharHealthAddress());
        }

        public override Matrix4x4 ReadActCharOriginTransformation()
        {
            return ReadEntityOriginTransformation(ActiveCharacterPointer);
        }

        public override Vector3 ReadActCharLocalTranslation()
        {
            return ReadEntityLocalTranslation(ActiveCharacterPointer);
        }

        public override void WriteActCharLocalTranslation(Vector3 value)
        {
            WriteEntityLocalTranslation(ActiveCharacterPointer, value);
        }

        public override void FreezeActCharLocalTranslationX(string value)
        {
            FreezeEntityLocalTranslationX(ActiveCharacterPointer, value);
        }

        public override void FreezeActCharLocalTranslationY(string value)
        {
            FreezeEntityLocalTranslationY(ActiveCharacterPointer, value);
        }

        public override void FreezeActCharLocalTranslationZ(string value)
        {
            FreezeEntityLocalTranslationZ(ActiveCharacterPointer, value);
        }

        public override void UnfreezeActCharLocalTranslationX()
        {
            UnfreezeEntityLocalTranslationX(ActiveCharacterPointer);
        }

        public override void UnfreezeActCharLocalTranslationY()
        {
            UnfreezeEntityLocalTranslationY(ActiveCharacterPointer);
        }

        public override void UnfreezeActCharLocalTranslationZ()
        {
            UnfreezeEntityLocalTranslationZ(ActiveCharacterPointer);
        }

        public override void FreezeActCharVelocityZ(string value)
        {
            if (value == "")
            {
                Vector3 trans = ReadActCharVelocity();
                value = trans.Z.ToString();
            }

            _m.FreezeValue($"{ActiveCharacterPointer},{_offsetTransformationLocal},B8", "float", value);
        }

        public override void UnfreezeActCharVelocityZ()
        {
            _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetTransformationLocal},B8");
        }

        public override float ReadActCharSpeedMultiplier()
        {
            return _m.ReadFloat($"{ActiveCharacterPointer},{_offsetSpeedMultiplier}");
        }

        public override void WriteActCharSpeedMultiplier(float value)
        {
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetSpeedMultiplier}", "float", value.ToString());
        }

        public override void FreezeActCharSpeedMultiplier(float value)
        {
            if (value == 0)
            {
                value = ReadActCharSpeedMultiplier();
            }

            _m.FreezeValue($"{ActiveCharacterPointer},{_offsetSpeedMultiplier}", "float", value.ToString());
        }

        public override void UnfreezeActCharSpeedMultiplier()
        {
            _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetSpeedMultiplier}");
        }

        public Vector3 ReadActCharVelocity()
        {
            return _m.ReadVector3($"{ActiveCharacterPointer},{_offsetTransformationLocal},B0");
        }

        public void WriteActCharVelocity(Vector3 value)
        {
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetTransformationLocal},B0", "vec3", value.ToString());
        }

        private string GetActCharHealthAddress()
        {
            string address = ActiveCharacterHealthAddress;
            if (_m.ReadInt($"{ActiveCharacterPointer},{_offsetSavefileHealth}") == 0)
            {
                address = $"{ActiveCharacterPointer},184";
            }

            return address;
        }
        #endregion

        #region Toggles
        public override void ToggleUndetectable(bool enableUndetectable)
        {
            if (enableUndetectable)
            {
                _m.FreezeValue($"{ActiveCharacterPointer},{_offsetUndetectable}", "int", "1");
            }
            else
            {
                _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetUndetectable}");
                _m.WriteMemory($"{ActiveCharacterPointer},{_offsetUndetectable}", "int", "0");
            }
        }

        public override void ToggleInvulnerable(bool enableInvulnerable)
        {
            if (enableInvulnerable)
            {
                _m.FreezeValue($"{ActiveCharacterPointer},{_offsetInvulnerable}", "int", "1");
            }
            else
            {
                _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetInvulnerable}");
                _m.WriteMemory($"{ActiveCharacterPointer},{_offsetInvulnerable}", "int", "0");
            }
        }

        public override void ToggleInfiniteDbJump(bool enableInfDbJump)
        {
            if (enableInfDbJump)
            {
                _m.FreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}", "int", "1");
            }
            else
            {
                _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}");
            }
        }
        #endregion

        #region Maps
        public override void LoadMap(int mapId)
        {
            if (mapId == -1)
            {
                // Current map
                mapId = GetMapId();
            }

            byte[] data = _m.ReadBytes($"{ReloadValuesPointer},{mapId * 0x40:X8}", 0x40);
            _m.WriteBytes($"{ReloadAddress}+8", data);
            ReloadMap();
        }

        public override void LoadMap(int mapId, int entranceValue)
        {
            _m.WriteMemory($"{ReloadAddress}+48", "int", $"{entranceValue}");
            LoadMap(mapId);
        }

        public override void LoadMap(int mapId, int entranceValue, int mode)
        {
            _m.WriteMemory($"{ReloadAddress}+4", "int", $"{mode}");
            LoadMap(mapId, entranceValue);
        }

        public void ReloadMap()
        {
            _m.WriteMemory($"{ReloadAddress}+4", "int", "0"); // mode
            _m.WriteMemory(ReloadAddress, "int", "1");
        }
        #endregion

        public string GetStringFromId(int id)
        {
            if (id == -1)
            {
                return "";
            }

            int count = _m.ReadInt($"{StringTableCountAddress}");
            string address = _m.ReadInt($"{StringTableCountAddress}+4").ToString("X");
            for (int i = 0; i < count; i++)
            {
                int stringId = _m.ReadInt($"{address}+{i * 8:X}");
                if (id == stringId)
                {
                    int stringPointer = _m.ReadInt($"{address}+{i * 8 + 4:X}");
                    string str = _m.ReadNullTerminatedString(stringPointer.ToString("X"), _encoding);
                    return str;
                }
            }

            return "";
        }

        public List<(int id, string str)> GetStringTable(bool ordered)
        {
            int count = _m.ReadInt($"{StringTableCountAddress}");
            List<(int id, string str)> table = new(count);

            string address = _m.ReadInt($"{StringTableCountAddress}+4").ToString("X");
            for (int i = 0; i < count; i++)
            {
                int stringId = _m.ReadInt($"{address}+{i * 8:X}");
                int stringPointer = _m.ReadInt($"{address}+{i * 8 + 4:X}");
                string str = _m.ReadNullTerminatedString(stringPointer.ToString("X"), _encoding);
                table.Add((stringId, str));
            }

            if (ordered)
            {
                table.Sort((a, b) => a.id.CompareTo(b.id));
            }

            return table;
        }

        public override Controller_t GetController()
        {
            return new(_m, $"{ActiveCharacterPointer},{_offsetController},{_offsetControllerBinds}");
        }

        protected override List<Character_t> GetCharacters()
        {
            return new()
            {
                new("Sly", 7, "jt", "sly"),
                new("Bentley", 8, "bentley", "bentley"),
                new("Murray", 9, "murray", "murray"),
            };
        }

        protected override List<List<Gadget_t>> GetGadgets()
        {
            return new()
            {
                new()
                {
                    new("Smoke Bomb", 0x17),
                    new("Combat Dodge", 0x18),
                    new("Stealth Slide", 0x19),
                    new("Alarm Clock", 0x1A),
                    new("Paraglide", 0x1B),
                    new("Silent Obliteration", 0x1C),
                    new("Thief Reflexes", 0x1D),
                    new("Feral Pounce", 0x1E),
                    new("Mega Jump", 0x1F),
                    new("Tornado Strike", 0x20),
                    new("Knockout Dive", 0x21),
                    new("Insanity Strike", 0x22),
                    new("Voltage Attack", 0x23),
                    new("Rage Bomb", 0x25),
                    new("Music Box", 0x26),
                    new("Lightning Spin", 0x27),
                    new("Shadow Power", 0x28),
                    new("TOM", 0x29),
                    new("Time Rush", 0x2A),
                },

                new()
                {
                    new("Trigger Bomb", 0x7),
                    new("Size Destabilizer", 0x8),
                    new("Snooze Bomb", 0x9),
                    new("Adrenaline Burst", 0xA),
                    new("Health Extractor", 0xB),
                    new("Hover Pack", 0xC),
                    new("Reduction Bomb", 0xD),
                    new("Temporal Lock", 0xE),
                    new("Long Toss", 0x24),
                },

                new()
                {
                    new("Fists of Flame", 0xF),
                    new("Turnbuckle Launch", 0x10),
                    new("Juggernaut Throw", 0x11),
                    new("Atlas Strength", 0x12),
                    new("Diablo Fire Slam", 0x13),
                    new("Berserker Charge", 0x14),
                    new("Guttural Roar", 0x15),
                    new("Raging Inferno Flop", 0x16),
                },
            };
        }

        protected override List<Map_t> GetMaps()
        {
            return new()
            {
                new("Cairo",
                    new()
                    {
                        new("Museum", new(4910, -5210, 580)),
                        new("Computer", new(5400, -700, 1100)),
                        new("Balcony 1", new(11560, -1050, 1110)),
                        new("Balcony 2", new(-12870, 15500, 1370)),
                        new("Murray rendezvous", new(18790, 80, 1500)),
                        new("Warehouse", new(12500, 5790, 1600)),
                        new("Chase start", new(10000, 8150, 1860)),
                        new("Pickup point", new(-26400, 4350, 80)),
                    }
                ),
                new("DVD menu",
                    new()
                    {
                        new(),
                    }
                ),
                new("Paris hub",
                    new()
                    {
                        new("Safehouse", new(-1800, -4100, 535)),
                        new("Safehouse (top)", new(-2665, -3640, 1256)),
                        new("Dimitri's boat", new(-7090, -6320, -30)),
                        new("Satellite dish 1", new(-6000, 4700, 1100)),
                        new("Satellite dish 2", new(-5000, -4800, 1300)),
                        new("Satellite dish 3", new(6100, -7500, 1300)),
                        new("Nightclub (door)", new(-1645, 5655, 60)),
                        new("Nightclub (window)", new(-3455, 5510, 1100)),
                        new("Nightclub (top)", new(0, 0, 5000)),
                        new("Courtyard", new(1860, 5630, -50)),
                        new("Clock tower", new(8600, 2080, 2070)),
                        new("Hotel", new(4430, -8000, 2420)),
                    }
                ),
                new($"{SubMapNamePrefix}Wine cellar",
                    new()
                    {
                        new("Entrance", new(14500, -6700, 470)),
                        new("Lasers", new(10340, -3900, 60)),
                        new("Office", new(5800, -2900, -150)),
                        new("Music room", new(95, 2850, 125)),
                    }
                ),
                new($"{SubMapNamePrefix}Nightclub",
                    new()
                    {
                        new("Entrance (door)", new(-2670, 5880, -540)),
                        new("Entrance (window)", new(-7780, 1440, 810)),
                        new("Dancefloor", new(-7030, 8845, -1000)),
                        new("Dimitri's office", new(-3800, 6460, 200)),

                    }
                ),
                new($"{SubMapNamePrefix}Print room",
                    new()
                    {
                        new("Entrance (recon)", new(-1100, 4180, 1470)),
                        new("Bottom floor", new(0, 0, -50)),
                        new("Money printer", new(0, 900, 740)),
                        new("Top floor", new(0, 1800, 1570)),
                    }
                ),
                new($"{SubMapNamePrefix}Theater",
                    new()
                    {
                        new("Entrance", new(-40, 4220, 910)),
                        new("Fan control", new(7000, 5110, 730)),
                        new("TV", new(3800, 8490, 895)),
                        new("Spotlight control", new(2560, 5820, 1560)),
                    }
                ),
                new($"{SubMapNamePrefix}Water pump room",
                    new()
                    {
                        new("Entrance", new(-13060, 6580, -170)),
                        new("Fireplace", new(-9670, 2470, -540)),
                        new("Water pump", new(-5560, 3850, 130)),
                    }
                ),
                new("India 1 hub",
                    new()
                    {
                        new("Safehouse", new(-4600, 2180, 460)),
                        new("Safehouse (inside)", new(-6670, -1470, 2270)),
                        new("Palace (door)", new(10000, 2100, 1690)),
                        new("Guesthouse (top)", new(3160, -10420, 1770)),
                        new("Cobra statue", new(6950, 8680, 1770)),
                        new("Drain pipe (basement entrance)", new(14150, 2160, 780)),
                        new("Drawbridge control", new(200, 1980, 960)),
                    }
                ),
                new($"{SubMapNamePrefix}Hotel",
                    new()
                    {
                        new("Entrance", new(-80, 1800, -780)),
                        new("Room 101", new(-6400, -1700, 100)),
                        new("Room 102", new(-3300, 120, -170)),
                        new("Room 103", new(-110, -1540, -480)),
                        new("Room 104", new(3320, 50, -170)),
                        new("Room 105", new(6540, -1880, 110)),
                    }
                ),
                new($"{SubMapNamePrefix}Basement",
                    new()
                    {
                        new("Entrance", new(2470, -1850, 40)),
                        new("Vault", new(2640, 0, 390)),
                        new("Boardroom", new(1360, 0, 1170)),
                    }
                ),
                new($"{SubMapNamePrefix}Ballroom",
                    new()
                    {
                        new("Entrance", new(1160, 5380, 910)),
                        new("Dance floor", new(1460, 2600, 70)),
                        new("Guests (left)", new(3200, -1000, 1400)),
                        new("Guests (right)", new(-500, -800, 1400)),
                    }
                ),
                new("India 2 hub",
                    new()
                    {
                        new("Safehouse", new(-2930, -5540, 1980)),
                        new("Safehouse (top)", new(-2700, -5600, 3070)),
                        new("Watermill ", new(2500, -700, 1300)),
                        new("Tilting temple", new(-9100, -1600, 2500)),
                        new("Temple entrance", new(0, 3400, 2040)),
                        new("Waterfall", new(-9040, 7430, 0)),
                        new("Dam", new(1390, 8200, 8010)),
                    }
                ),
                new($"{SubMapNamePrefix}Spice factory",
                    new()
                    {
                        new("Factory entrance", new(-10200, 9100, -30)),
                        new("Factory entrance (top)", new(-10000, 9880, 1600)),
                        new("Factory recon area", new(-14160, 12000, 2200)),
                        new("Spice grinder entrance", new(-5700, 1760, -1290)),
                        new("Spice grinder", new(-8200, -2560, -850)),
                        new("Rajan's office", new(5100, 11260, 2200)),
                    }
                ),
                new("Prague 1 hub",
                    new()
                    {
                        new("Safehouse", new(-7100, -10500, 235)),
                        new("Safehouse (top)", new(-7490, -11010, 1440)),
                        new("Bridge", new(6120, -5800, 730)),
                        new("Prison (center)", new(-1430, 1120, 3690)),
                        new("Prison (sly)", new(1930, -4480, 1960)),
                        new("Rooftop 1", new(-9060, 310, 1295)),
                        new("Rooftop 2", new(2370, -12450, 850)),
                        new("The Contessa's house", new(6840, -590, 1850)),
                    }
                ),
                new($"{SubMapNamePrefix}Prison",
                    new()
                    {
                        new("Entrance", new(540, -4040, 500)),
                        new("Murray's cell", new(7700, -2100, -1130)),
                        new("Hypno arena", new(-3660, 1950, -320)),
                        new("Control room", new(700, -1300, 250)),
                    }
                ),
                new($"{SubMapNamePrefix}Vault room",
                    new()
                    {
                        new("Entrance", new(-280, 390, 20)),
                        new("Behind the wall", new(1330, -1000, 490)),
                    }
                ),
                new("Prague 2 hub",
                    new()
                    {
                        new("Safehouse", new(11560, 2350, 800)),
                        new("Sewer", new(7980, 2270, -610)),
                        new("Graveyard", new(-2150, -7300, 420)),
                        new("Castle main entrance", new(0, 0, 150)),
                        new("Castle back entrance", new(-4700, -400, -300)),
                        new("Castle top 1", new(-6690, -2720, 1300)),
                        new("Castle top 2", new(-800, 4680, 2150)),
                        new("Castle top 3", new(-3980, 1920, 4530)),
                        new("Guillotine", new(-6445, 4333, 180)),
                        new("Re-education tower (entrance)", new(-1025, -4400, 3330)),
                        new("Re-education tower (balcony)", new(-175, -4485, 4900)),
                    }
                ),
                new("p_castle_int", // used to skip map id 18
                    new(),
                    false
                ),
                new($"{SubMapNamePrefix}Crypt 3 (Stealing Voices)",
                    new()
                    {
                        new("Entrance", new(-25200, -180, 75)),
                        new("End", new(-25220, -9320, 190)),
                    }
                ),
                new($"{SubMapNamePrefix}Crypts 1 & 2 (Stealing Voices)",
                    new()
                    {
                        new("Entrance (crypt 1)", new(-16570, -11490, -380)),
                        new("Vault (crypt 1)", new(-12270, -11550, -210)),
                        new("Entrance (crypt 2)", new(-14750, 560, 540)),
                        new("End (crypt 2)", new(-19480, 5450, 1180)),
                    }
                ),
                new($"{SubMapNamePrefix}Crypt 4 (Ghost Capture)",
                    new()
                    {
                        new("Entrance", new(3530, -11640, -380)),
                        new("Tomb", new(3040, -6300, -1180)),
                    }
                ),
                new($"{SubMapNamePrefix}Re-education tower & Hacking Crypt",
                    new()
                    {
                        new("Entrance (Re-education Tower)", new(-16020, -11490, -40)),
                        new("Re-education cell", new(-14300, -11720, -340)),
                        new("Entrance (hack)", new(-6690, -4700, 470)),
                        new("End (hack)", new(-1000, 8300, -380)),
                        new("Unused area 1", new(9050, -2630, 100)),
                        new("Unused area 2", new(10200, -7430, -400)),
                        new("Unused area 3", new(13330, -7390, -380)),
                    }
                ),
                new($"{SubMapNamePrefix}Crypt 1 (Mojo Trap Action)",
                    new()
                    {
                        new("Entrance", new(-16197, -11483, -307)),
                    }
                ),
                new($"{SubMapNamePrefix}Crypt 3 (Mojo Trap Action)",
                    new()
                    {
                        new("Entrance", new(4852, -6280, -1303)),
                    }
                ),
                new($"{SubMapNamePrefix}Crypt 2 (Mojo Trap Action)",
                    new()
                    {
                        new("Entrance", new(9069, -2627, 118)),
                    }
                ),
                new($"{SubMapNamePrefix}Crypt 4 (Mojo Trap Action)",
                    new()
                    {
                        new("Entrance", new(-25210, -5428, 133)),
                    }
                ),
                new("Canada hub",
                    new()
                    {
                        new("Safehouse", new(-1060, -11040, 30)),
                        new("Safehouse (top)", new(-50, -10560, 1440)),
                        new("Cabin 1 (Jean Bison)", new(-3940, 6870, 2030)),
                        new("Cabin 2", new(-12220, -3630, 1960)),
                        new("Cabin 3", new(5960, 6275, 890)),
                        new("Satellite dish", new(660, 5330, 4740)),
                        new("Plane", new(-1840, 9260, 20)),
                    }
                ),
                new($"{SubMapNamePrefix}Cabins",
                    new()
                    {
                        new("Cabin 1", new(-8820, -8470, 130)),
                        new("Cabin 2", new(8370, -8500, 130)),
                        new("Cabin 3", new(8370, 6260, 130)),
                    }
                ),
                new($"{SubMapNamePrefix}Train (Aerial Assault / Theft on Rails)",
                    new()
                    {
                        new("Back", new(0, -8400, 120)),
                        new("Front", new(40, 21300, 120)),
                    }
                ),
                new($"{SubMapNamePrefix}Train (Operation)",
                    new()
                    {
                        new("Back", new(0, -8400, 120)),
                        new("Front", new(0, 25100, 120)),
                        new("Jean Bison", new(0, 2380, 120)),
                    }
                ),
                new($"{SubMapNamePrefix}Train (Ride the Iron Horse)",
                    new()
                    {
                        new("Back", new(0, -8400, 120)),
                        new("Front", new(0, 25100, 120)),
                    }
                ),
                new("Canada 2 hub",
                    new()
                    {
                        new("Safehouse", new(2290, -3380, 560)),
                        new("Van", new(8420, -970, -810)),
                        new("Sawmill 1", new(520, 7270, 1825)),
                        new("Sawmill 2 / Laser Redirection", new(-5140, 7200, 1470)),
                        new("Sawmill 3 / RC Combat Club", new(-5700, -6390, 1480)),
                        new("Bomb fishing spot", new(-4670, -1190, 920)),
                        new("Battery silo", new(-9480, -3050, 1490)),
                        new("Lighthouse", new(-12750, -3500, -330)),
                        new("Lighthouse (top)", new(-13700, -3850, 4060)),
                    }
                ),
                new($"{SubMapNamePrefix}RC Combat Club",
                    new()
                    {
                        new("Moose head", new(19390, -1080, 1720)),
                        new("Sawblade crawl", new(22040, -2520, 1470)),
                    }
                ),
                new($"{SubMapNamePrefix}Sawmill",
                    new()
                    {
                        new("Entrance", new(-1400, 22123, 1200)),
                        new("Vault", new(760, 20930, 880)),
                        new("Lasers", new(-268, 20348, 1828)),
                        new("Lever", new(690, 22320, 1190)),
                    }
                ),
                new($"{SubMapNamePrefix}Lighthouse",
                    new()
                    {
                        new("Top entrance", new(-290, 375, 5845)),
                        new("Bottom", new(0, 0, 960)),
                        new("Recon", new(680, 1060, 1015)),
                    }
                ),
                new($"{SubMapNamePrefix}Bear cave",
                    new()
                    {
                        new("Entrance", new(-2525, 2960, 30)),
                        new("Large ice wall", new(1000, 5255, 165)),
                    }
                ),
                new($"{SubMapNamePrefix}Sawmill (Boss)",
                    new()
                    {
                        new("Arena", new(1140, -520, 75)),
                        new("Control room", new(620, 840, 1800)),
                    }
                ),
                new("Blimp hub",
                    new()
                    {
                        new("Safehouse", new(11400, -25, 960)),
                        new("Safehouse (top)", new(10840, -485, 3100)),
                        new("Balloon 1", new(6540, -2710, 2360)),
                        new("Balloon 2", new(6480, 2690, 2360)),
                        new("Engine 1", new(-10220, 4055, 3960)),
                        new("Engine 2", new(-10270, -4060, 3960)),
                        new("Center", new(0, 0, 2810)),
                    }
                ),
                new($"{SubMapNamePrefix}Blimp HQ",
                    new()
                    {
                        new("Entrance", new(-3890, 0, 730)),
                        new("Clockwerk", new(135, 0, 360)),
                        new("Neyla", new(5160, 0, 100)),
                        new("Center", new(0, 0, -670)),
                    }
                ),
                new($"{SubMapNamePrefix}Engine room 1 (Bentley & Murray)",
                    new()
                    {
                        new("Entrance", new(-2190, 130, 10)),
                        new("Half", new(2300, 0, 100)),
                        new("Control room", new(-2250, -20, 710)),
                    }
                ),
                new($"{SubMapNamePrefix}Engine room 2 (Sly & Bentley)",
                    new()
                    {
                        new("Entrance", new(-2140, 140, 10)),
                        new("Half", new(2300, 0, 100)),
                        new("Control room", new(-2250, -20, 710)),
                    }
                ),
                new($"{SubMapNamePrefix}Engine room 3 (Murray & Sly)",
                    new()
                    {
                        new("Entrance", new(-2110, 430, 10)),
                        new("Half", new(2300, 0, 100)),
                        new("Control room", new(-2250, -85, 710)),
                    }
                ),
                new($"{SubMapNamePrefix}Paris (Clock-la)",
                    new()
                    {
                        new("Spawn (sky)", new(-12560, 580, 86720)),
                        new("Clock-la (sky)", new(22803, -1340, 76170)),
                        new("Ground level", new(-100, -666, -130)),
                        new("Destroyed walkway", new(920, -7480, 1580)),
                    }
                )
            };
        }
    }
}
