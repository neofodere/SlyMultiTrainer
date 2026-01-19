using System.Numerics;
using System.Text;
using static SlyMultiTrainer.Sly2_3_Savefile;
using static SlyMultiTrainer.Util;

namespace SlyMultiTrainer
{
    public class Sly3Handler : GameBase_t
    {
        public string ReloadAddress = "";
        public string ReloadValuesAddress = "";
        public string FKXListCount = "";
        public string CameraPointer = "";
        public string DeathBarriersAddress = "";
        public string ActiveCharacterPointer = "";
        public string ActiveCharacterIdAddress = "";
        public string StringTableCountAddress = "";
        public string IsLoadingAddress = "";
        public DAG_t DAG;
        public Sly2_3_Savefile Savefile;

        private string _offsetTransformationOrigin = "44";
        private string _offsetTransformationLocal = "";
        private string _offsetTransformationWorld = "";
        private string _offsetHealth = "16C";
        private string _offsetGadgetPower = "174";
        private string _offsetController = "134";
        private string _offsetControllerBinds = "18";
        private string _offsetInfiniteDbJump = "33C";
        private string _offsetSpeedMultiplier = "354";
        private string _offsetInvulnerable = "180";
        private string _offsetGadgetBinds = "1250";
        private string _offsetUndetectable = "1160";

        private Memory.Mem _m;
        private Encoding _encoding;

        public Sly3Handler(Memory.Mem m, Form1 form, string region) : base(m, form, region)
        {
            _m = m;
            DAG = new(m);
            DAG.SetVersion(DAG_VERSION.V3);
            Savefile = new(m);
            Savefile.SetVersion(SAVEFILE_VERSION.V1);
            _encoding = Encoding.Unicode;

            DAG.OffsetNextNodePointer = "20";
            DAG.OffsetState = "44";
            DAG.OffsetGoalDescription = "4C";
            DAG.OffsetFocusCount = "54";
            DAG.OffsetCompleteCount = "58";
            DAG.OffsetMissionName = "60";
            DAG.OffsetMissionDescription = "64";
            DAG.OffsetClusterPointer = "6C";
            DAG.OffsetChildrenCount = "90";
            DAG.OffsetCheckpointEntranceValue = "A8";
            DAG.OffsetAttributes = "D0";
            DAG.OffsetAttributesForCluster = "D0";
            DAG.GetStringFromId = GetStringFromId;
            DAG.LoadMap = LoadMap;
            DAG.WriteActCharId = WriteActCharId;

            if (region == "NTSC")
            {
                ReloadAddress = "4797C4";
                ReloadValuesAddress = "2EDFD8";
                FKXListCount = "479AAC";
                ClockAddress = "36BBA0";
                CoinsAddress = "468DDC";
                GadgetAddress = "468DCC";
                CameraPointer = "47933C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},2F4";
                MapIdAddress = "47989C";
                DeathBarriersAddress = "478BF4,2C";
                GuardAIAddress = "370A8C";
                ActiveCharacterPointer = "36F84C";
                ActiveCharacterIdAddress = "36C710";
                DAG.RootNodePointer = "478C8C";
                DAG.CurrentCheckpointNodePointer = "4794CC";
                DAG.ClusterIdAddress = "36DB98";
                Savefile.SavefileStartAddress = "468D30";
                Savefile.SavefileKeyAddressTablePointer = "4793CC";
                Savefile.SavefileKeyStringTablePointer = "4794A8";
                DAG.Sly3Time = "36BC20";
                DAG.Sly3Flag = "479754";
                StringTableCountAddress = "47A2D4";
                IsLoadingAddress = "467B00";
            }
            else if (region == "PAL")
            {
                ReloadAddress = "47AE44";
                ReloadValuesAddress = "2EE658";
                FKXListCount = "47B12C";
                ClockAddress = "36C620";
                CoinsAddress = "46A45C";
                GadgetAddress = "46A44C";
                CameraPointer = "47A9BC";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},2F4";
                MapIdAddress = "47AF1C";
                DeathBarriersAddress = "47A274,2C";
                GuardAIAddress = "37150C";
                ActiveCharacterPointer = "3702CC";
                ActiveCharacterIdAddress = "36D190";
                DAG.RootNodePointer = "47A30C";
                DAG.CurrentCheckpointNodePointer = "47AB4C";
                DAG.ClusterIdAddress = "36E618";
                Savefile.SavefileStartAddress = "46A3B0";
                Savefile.SavefileKeyAddressTablePointer = "47AA4C";
                Savefile.SavefileKeyStringTablePointer = "47AB28";
                DAG.Sly3Time = "36C6A0";
                DAG.Sly3Flag = "47ADD4";
                StringTableCountAddress = "47B954";
                IsLoadingAddress = "469180";
            }
            else if (region == "NTSC-K")
            {
                ReloadAddress = "47B8C4";
                ReloadValuesAddress = "2EEF58";
                FKXListCount = "47BBAC";
                ClockAddress = "36D0A0";
                CoinsAddress = "46AEDC";
                GadgetAddress = "46AECC";
                CameraPointer = "47B43C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},2F4";
                MapIdAddress = "47B99C";
                DeathBarriersAddress = "47ACF4,2C";
                GuardAIAddress = "371F8C";
                ActiveCharacterPointer = "370D4C";
                ActiveCharacterIdAddress = "36DC10";
                DAG.RootNodePointer = "47AD8C";
                DAG.CurrentCheckpointNodePointer = "47B5CC";
                DAG.ClusterIdAddress = "36F098";
                Savefile.SavefileStartAddress = "46AE30";
                Savefile.SavefileKeyAddressTablePointer = "47B4CC";
                Savefile.SavefileKeyStringTablePointer = "47B5A8";
                DAG.Sly3Time = "36D120";
                DAG.Sly3Flag = "47B854";
                StringTableCountAddress = "47C3D4";
                IsLoadingAddress = "469C00";
            }
            else if (region == "NTSC July 16")
            {
                _offsetUndetectable = "1180";
                _offsetSpeedMultiplier = "358";
                _offsetInfiniteDbJump = "348";
                _offsetGadgetBinds = "1270";

                ReloadAddress = "46BB24";
                ReloadValuesAddress = "2DEB08";
                FKXListCount = "46BE0C";
                ClockAddress = "35F6A0";
                CoinsAddress = "45B0A8";
                GadgetAddress = "45B09C";
                CameraPointer = "46B5AC";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},324";
                MapIdAddress = "46BBFC";
                DeathBarriersAddress = "46AE74,2C";
                GuardAIAddress = "362F1C";
                ActiveCharacterPointer = "361D5C";
                ActiveCharacterIdAddress = "45AFC4";
                DAG.RootNodePointer = "46AEF4";
                DAG.CurrentCheckpointNodePointer = "46B738";
                DAG.ClusterIdAddress = "3600D0";
                Savefile.SavefileStartAddress = "45AFB0";
                Savefile.SavefileKeyAddressTablePointer = "46B63C";
                Savefile.SavefileKeyStringTablePointer = "46B718";
                DAG.Sly3Time = "35F720";
                DAG.Sly3Flag = "46BAC8";
                StringTableCountAddress = "46C624";
                IsLoadingAddress = "459D80";

                for (int i = 23; i < 35; i++)
                {
                    Maps[i].IsVisible = false;
                }

                Maps.RemoveRange(36, 4);

                Gadgets = new()
                {
                    new()
                    {
                        new("Smoke Bomb", 0x1A),
                        new("Combat Dodge", 0x1B),
                        new("Feral Pounce", 0x1E),
                        new("Mega Jump", 0x1F),
                        new("Knockout Dive", 0x20),
                        new("Unknown Rocket Boots", 0x21),
                        new("Shadow Power Level 1", 0x22),
                        new("Thief Reflexes", 0x23),
                        new("Shadow Power Level 2", 0x24),
                        new("Rocket Boots", 0x25),
                        new("Treasure Map", 0x26),
                        new("Shield", 0x27),
                        new("Venice Disguise", 0x28),
                        new("Photographer Disguise", 0x29),
                        new("Pirate Disguise", 0x2A),
                    },

                    new()
                    {
                        new("Trigger Bomb", 0x6),
                        new("Fishing Pole", 0x7),
                        new("Alarm Clock", 0x8),
                        new("Adrenaline Burst", 0x9),
                        new("Health Extractor", 0xA),
                        new("Insanity Strike", 0xC),
                        new("WebCam Bomb", 0xD),
                        new("Size Destabilizer", 0xE),
                        new("Rage Bomb", 0xF),
                        new("Reduction Bomb", 0x10),
                    },

                    new()
                    {
                        new("Be The Ball", 0x11),
                        new("Berserker Charge", 0x12),
                        new("Guttural Roar", 0x14),
                        new("Fists of Flame", 0x15),
                        new("Temporal Lock", 0x16),
                        new("Raging Inferno Flop", 0x17),
                        new("Diablo Fire Slam", 0x18),
                        new("Cutscene Puppet", 0x19),
                    }
                };
            }
            else if (region == "NTSC E3 Demo")
            {
                DAG.SetVersion(DAG_VERSION.V2);
                _offsetSpeedMultiplier = "328";
                _offsetInfiniteDbJump = "318";
                _offsetGadgetBinds = "1240";

                ReloadAddress = "460C60";
                ReloadValuesAddress = "461900,0"; // pointer
                FKXListCount = "460F6C";
                ClockAddress = "36FA00";
                CoinsAddress = "453F0C";
                GadgetAddress = "453F04";
                CameraPointer = "46080C";
                DrawDistanceAddress = $"{CameraPointer},234";
                FOVAddress = $"{CameraPointer},23C";
                ResetCameraAddress = $"{CameraPointer},444";
                MapIdAddress = "453E28";
                DeathBarriersAddress = "";
                GuardAIAddress = "37215C";
                ActiveCharacterPointer = "37211C";
                ActiveCharacterIdAddress = "453E2C";
                StringTableCountAddress = "461790";
                IsLoadingAddress = "452380";

                DAG.RootNodePointer = "460158";
                DAG.CurrentCheckpointNodePointer = "460998";
                DAG.ClusterIdAddress = "370488";
                Savefile.SavefileStartAddress = "453E20";
                Savefile.SavefileKeyAddressTablePointer = "46089C";
                Savefile.SavefileKeyStringTablePointer = "460978";
                DAG.Sly3Time = "36FA80";
                DAG.Sly3Flag = "460C00";
                DAG.OffsetCheckpointEntranceValue = "A4";
                DAG.OffsetMissionName = "";
                DAG.OffsetClusterPointer = "68";
                DAG.OffsetChildrenCount = "8C";
                DAG.OffsetAttributesForCluster = "C0";
                DAG.OffsetAttributes = "C4";

                Maps[0].IsVisible = false;
                Maps[1].Name = "dvd_menu";
                Maps[1].IsVisible = true;
                Maps.RemoveAt(2);
                Maps.RemoveAt(6);
                Maps[3].IsVisible = false;
                Maps[4].IsVisible = false;
                Maps[5].IsVisible = false;
                Maps[7].IsVisible = false;
                Maps[8].IsVisible = false;
                Maps[9].IsVisible = false;
                Maps[10].IsVisible = false;
                Maps[12].IsVisible = false;
                Maps[13].IsVisible = false;
                Maps[14].IsVisible = false;
                Maps[15].IsVisible = false;
                Maps[16].IsVisible = false;
                Maps[17].IsVisible = false;
                Maps[18].IsVisible = false;
                Maps.RemoveRange(20, 18);

                Gadgets = new()
                {
                    new()
                    {
                        new("Smoke Bomb", 0x15),
                        new("Combat Dodge", 0x16),
                        new("Unknown Rocket Boots", 0x17),
                        new("Thief Reflexes", 0x1B),
                        new("Feral Pounce", 0x1C),
                        new("Mega Jump", 0x1D),
                        new("Insanity Strike", 0x20),
                        new("Voltage Attack", 0x21),
                        new("Rage Bomb", 0x23),
                        new("Music Box", 0x24),
                        new("Shadow Power Level 1", 0x26),
                        new("Time Rush", 0x27),
                        new("Crash Disguise 1", 0x28),
                        new("Crash Disguise 2", 0x29),
                        new("Rocket Boots", 0x2E),
                        new("Shadow Power Level 2", 0x2F),
                        new("Shield", 0x31),
                    },

                    new()
                    {
                        new("Trigger Bomb", 0x6),
                        new("Fishing Pole", 0x2A),
                        new("Cube", 0x7),
                        new("Snooze Bomb", 0x8),
                        new("Unknown", 0xD), // Insanity Strike or Size Destabilizer
                        new("Adrenaline Burst", 0x9),
                        new("Health Extractor", 0xA),
                        new("Grapple-Cam", 0x2D),
                        new("Reduction Bomb", 0xC),
                    },

                    new()
                    {
                        new("Be The Ball", 0x2B),
                        new("Berserker Charge", 0x12),
                        new("Guttural Roar", 0x13),
                        new("Fists of Flame", 0xE),
                        new("Raging Inferno Flop", 0x14),
                        new("Diablo Fire Slam", 0x11),
                        new("Turnbuckle Launch", 0xF),
                        new("Butterfly Net", 0x2C),
                    }
                };
            }
            else if (region == "NTSC Regular Demo")
            {
                _offsetSpeedMultiplier = "358";
                _offsetInfiniteDbJump = "348";
                _offsetUndetectable = "1180";
                _offsetGadgetBinds = "1270";

                ReloadAddress = "46E97C";
                ReloadValuesAddress = "2D83A0";
                FKXListCount = "46EC60";
                ClockAddress = "37A460";
                CoinsAddress = "45DF34";
                GadgetAddress = "45DF28";
                CameraPointer = "46E42C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},324";
                MapIdAddress = "46EA54";
                DeathBarriersAddress = "46DCF0,2C";
                GuardAIAddress = "37CB5C";
                ActiveCharacterPointer = "37CB1C";
                ActiveCharacterIdAddress = "45DE50";
                StringTableCountAddress = "46F484";
                IsLoadingAddress = "45CC00";

                DAG.RootNodePointer = "46DD74";
                DAG.CurrentCheckpointNodePointer = "46E5B8";
                DAG.ClusterIdAddress = "37AE90";
                Savefile.SavefileStartAddress = "45DE40";
                Savefile.SavefileKeyAddressTablePointer = "46E4BC";
                Savefile.SavefileKeyStringTablePointer = "46E598";
                DAG.Sly3Time = "37A4E0";
                DAG.Sly3Flag = "46E918";
                DAG.OffsetMissionName = "58";
                DAG.OffsetAttributesForCluster = "C8";

                Maps[0].IsVisible = false;
                Maps[1].IsVisible = true;
                Maps[2].IsVisible = false;
                Maps[4].IsVisible = false;
                Maps[5].IsVisible = false;
                Maps[7].IsVisible = false;
                Maps[9].IsVisible = false;
                Maps[10].IsVisible = false;
                Maps[11].IsVisible = false;
                Maps[12].IsVisible = false;
                Maps[14].IsVisible = false;
                Maps[15].IsVisible = false;
                Maps[16].IsVisible = false;
                Maps[17].IsVisible = false;
                Maps[18].IsVisible = false;
                Maps[19].IsVisible = false;
                Maps[20].IsVisible = false;
                Maps.RemoveRange(22, 18);

                Gadgets = new()
                {
                    new()
                    {
                        new("Smoke Bomb", 0x1A),
                        new("Combat Dodge", 0x1B),
                        new("Thief Reflexes", 0x23),
                        new("Feral Pounce", 0x1E),
                        new("Mega Jump", 0x1F),
                        new("Shadow Power Level 1", 0x22),
                        new("Unknown Rocket Boots", 0x21),
                        new("Rocket Boots", 0x25),
                        new("Shadow Power Level 2", 0x24),
                        new("Shield", 0x27),
                    },

                    new()
                    {
                        new("Trigger Bomb", 0x6),
                        new("Fishing Pole", 0x7),
                        new("Alarm Clock", 0x8),
                        new("Adrenaline Burst", 0x9),
                        new("Health Extractor", 0xA),
                        new("Insanity Strike", 0xC),
                        new("Grapple-Cam", 0xD),
                        new("Size Destabilizer", 0xE),
                        new("Rage Bomb", 0xF),
                        new("Reduction Bomb", 0x10),
                    },

                    new()
                    {
                        new("Be The Ball", 0x11),
                        new("Berserker Charge", 0x12),
                        new("Guttural Roar", 0x14),
                        new("Fists of Flame", 0x15),
                        new("Temporal Lock", 0x16),
                        new("Raging Inferno Flop", 0x17),
                        new("Diablo Fire Slam", 0x18),
                        new("Cutscene Puppet", 0x19),
                    }
                };
            }
            else if (region == "PAL Demo")
            {
                ReloadAddress = "485744";
                ReloadValuesAddress = "2E9DD8";
                FKXListCount = "485A2C";
                ClockAddress = "38F660";
                CoinsAddress = "474D7C";
                GadgetAddress = "474D6C";
                CameraPointer = "4852BC";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},2F4";
                MapIdAddress = "474CD8";
                DeathBarriersAddress = "484B74,2C";
                GuardAIAddress = "39338C";
                ActiveCharacterPointer = "39330C";
                ActiveCharacterIdAddress = "3901D0";
                DAG.RootNodePointer = "484C0C";
                DAG.CurrentCheckpointNodePointer = "48544C";
                DAG.ClusterIdAddress = "391658";
                Savefile.SavefileStartAddress = "474CD0";
                Savefile.SavefileKeyAddressTablePointer = "48534C";
                Savefile.SavefileKeyStringTablePointer = "485428";
                DAG.Sly3Time = "38F6E0";
                DAG.Sly3Flag = "4856D4";
                StringTableCountAddress = "486254";
                IsLoadingAddress = "473A80";

                Maps[0].IsVisible = false;
                Maps[1].IsVisible = true;
                Maps[2].IsVisible = false;
                Maps[4].IsVisible = false;
                Maps[5].IsVisible = false;
                Maps[7].IsVisible = false;
                Maps[9].IsVisible = false;
                Maps[10].IsVisible = false;
                Maps[11].IsVisible = false;
                Maps[12].IsVisible = false;
                Maps[14].IsVisible = false;
                Maps[15].IsVisible = false;
                Maps[16].IsVisible = false;
                Maps[17].IsVisible = false;
                Maps[18].IsVisible = false;
                Maps[19].IsVisible = false;
                Maps[20].IsVisible = false;
                Maps.RemoveRange(22, 18);
            }
            else if (region == "PAL August 2")
            {
                _offsetHealth = "170";
                _offsetGadgetPower = "178";
                _offsetInfiniteDbJump = "34C";
                _offsetInvulnerable = "184";
                _offsetUndetectable = "1190";
                _offsetSpeedMultiplier = "368";
                _offsetGadgetBinds = "1280";

                ReloadAddress = "4AF7CC";
                ReloadValuesAddress = "2F8068";
                FKXListCount = "4AFAB4";
                ClockAddress = "38B1A0";
                CoinsAddress = "49E750";
                GadgetAddress = "49E740";
                CameraPointer = "4AF33C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},324";
                MapIdAddress = "4AF8A4";
                DeathBarriersAddress = "4AEBF4,2C";
                GuardAIAddress = "38EEA4";
                ActiveCharacterPointer = "38EE5C";
                ActiveCharacterIdAddress = "38BD40";
                DAG.RootNodePointer = "4AEC8C";
                DAG.CurrentCheckpointNodePointer = "4AF4C8";
                DAG.ClusterIdAddress = "38D1B8";
                Savefile.SavefileStartAddress = "49E6B0";
                Savefile.SavefileKeyAddressTablePointer = "4AF3CC";
                Savefile.SavefileKeyStringTablePointer = "4AF4A8";
                DAG.Sly3Time = "38B220";
                DAG.Sly3Flag = "4AF770";
                StringTableCountAddress = "4B02C4";
                IsLoadingAddress = "49D480";
            }
            else if (region == "PAL September 2")
            {
                _offsetHealth = "170";
                _offsetGadgetPower = "178";
                _offsetInfiniteDbJump = "34C";
                _offsetInvulnerable = "184";
                _offsetUndetectable = "1170";
                _offsetSpeedMultiplier = "364";
                _offsetGadgetBinds = "1260";

                ReloadAddress = "4BE9C4";
                ReloadValuesAddress = "304248";
                FKXListCount = "4BECAC";
                ClockAddress = "39A1A0";
                CoinsAddress = "4AD95C";
                GadgetAddress = "4AD94C";
                CameraPointer = "4BE53C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},2F4";
                MapIdAddress = "4BEA9C";
                DeathBarriersAddress = "4BDDF4,2C";
                GuardAIAddress = "39DF54";
                ActiveCharacterPointer = "39DECC";
                ActiveCharacterIdAddress = "39AD40";
                DAG.RootNodePointer = "4BDE8C";
                DAG.CurrentCheckpointNodePointer = "4BE6CC";
                DAG.ClusterIdAddress = "39C1C8";
                Savefile.SavefileStartAddress = "4AD8B0";
                Savefile.SavefileKeyAddressTablePointer = "4BE5CC";
                Savefile.SavefileKeyStringTablePointer = "4BE6A8";
                DAG.Sly3Time = "39A220";
                DAG.Sly3Flag = "4BE954";
                StringTableCountAddress = "4BF4D4";
                IsLoadingAddress = "4AC680";
            }
            else if (region == "NTSC (PS3)"
                  || region == "PAL (PS3)"
                  || region == "UK (PS3)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetHealth = "168";
                _offsetGadgetPower = "170";
                _offsetController = "130";
                _offsetControllerBinds = "1E";
                _offsetInfiniteDbJump = "32C";
                _offsetSpeedMultiplier = "344";
                _offsetInvulnerable = "17C";
                _offsetUndetectable = "1150";
                _offsetGadgetBinds = "1240";

                ReloadAddress = "70FD3C";
                ReloadValuesAddress = "4930D0";
                FKXListCount = "710024";
                ClockAddress = "514338";
                CoinsAddress = "657284";
                GadgetAddress = "657274";
                CameraPointer = "70F8AC";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},304";
                MapIdAddress = "70FE14";
                DeathBarriersAddress = "70F164,2C";
                GuardAIAddress = "57714C";
                ActiveCharacterPointer = "5770CC";
                ActiveCharacterIdAddress = "574A80";
                DAG.RootNodePointer = "70F1FC";
                DAG.CurrentCheckpointNodePointer = "70FA3C";
                DAG.ClusterIdAddress = "575F08";
                Savefile.SavefileStartAddress = "6571D0";
                Savefile.SavefileKeyAddressTablePointer = "70F93C";
                Savefile.SavefileKeyStringTablePointer = "70FA18";
                DAG.Sly3Time = "5143B0";
                DAG.Sly3Flag = "70FCCC";
                StringTableCountAddress = "710854";
                IsLoadingAddress = "656080";
            }
            else if (region == "NTSC-J (PS3)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetHealth = "168";
                _offsetGadgetPower = "170";
                _offsetController = "130";
                _offsetControllerBinds = "1E";
                _offsetInfiniteDbJump = "32C";
                _offsetSpeedMultiplier = "344";
                _offsetInvulnerable = "17C";
                _offsetUndetectable = "1150";
                _offsetGadgetBinds = "1240";

                ReloadAddress = "717DBC";
                ReloadValuesAddress = "493110";
                FKXListCount = "7180A4";
                ClockAddress = "5143B8";
                CoinsAddress = "657304";
                GadgetAddress = "6572F4";
                CameraPointer = "71792C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},304";
                MapIdAddress = "717E94";
                DeathBarriersAddress = "7171E4,2C";
                GuardAIAddress = "5771CC";
                ActiveCharacterPointer = "57714C";
                ActiveCharacterIdAddress = "574B00";
                DAG.RootNodePointer = "71727C";
                DAG.CurrentCheckpointNodePointer = "717ABC";
                DAG.ClusterIdAddress = "575F88";
                Savefile.SavefileStartAddress = "657250";
                Savefile.SavefileKeyAddressTablePointer = "7179BC";
                Savefile.SavefileKeyStringTablePointer = "717A98";
                DAG.Sly3Time = "514430";
                DAG.Sly3Flag = "717D4C";
                StringTableCountAddress = "7188D4";
                IsLoadingAddress = "656100";
            }
            else if (region == "NTSC (PS3 PSN)"
                  || region == "PAL (PS3 PSN)"
                  || region == "NTSC-K (PS3 PSN)")
            {
                _encoding = Encoding.BigEndianUnicode;
                _offsetHealth = "168";
                _offsetGadgetPower = "170";
                _offsetController = "130";
                _offsetControllerBinds = "1E";
                _offsetInfiniteDbJump = "32C";
                _offsetSpeedMultiplier = "344";
                _offsetInvulnerable = "17C";
                _offsetUndetectable = "1150";
                _offsetGadgetBinds = "1240";

                ReloadAddress = "78D2C0";
                ReloadValuesAddress = "508650";
                if (region == "NTSC (PS3 PSN)")
                {
                    ReloadValuesAddress = "508630";
                }
                else if (region == "NTSC-K (PS3 PSN)")
                {
                    ReloadValuesAddress = "508610";
                }

                FKXListCount = "78D5A8";
                ClockAddress = "5898B8";
                CoinsAddress = "6CC808";
                GadgetAddress = "6CC7F8";
                CameraPointer = "78CE2C";
                DrawDistanceAddress = $"{CameraPointer},114";
                FOVAddress = $"{CameraPointer},11C";
                ResetCameraAddress = $"{CameraPointer},304";
                MapIdAddress = "78D398";
                DeathBarriersAddress = "78C6E4,2C";
                GuardAIAddress = "5EC6CC";
                ActiveCharacterPointer = "5EC64C";
                ActiveCharacterIdAddress = "5EA000";
                DAG.RootNodePointer = "78C77C";
                DAG.CurrentCheckpointNodePointer = "78CFBC";
                DAG.ClusterIdAddress = "5EB488";
                Savefile.SavefileStartAddress = "6CC750";
                Savefile.SavefileKeyAddressTablePointer = "78CEBC";
                Savefile.SavefileKeyStringTablePointer = "78CF98";
                DAG.Sly3Time = "589930";
                DAG.Sly3Flag = "78D250";
                StringTableCountAddress = "78DDD4";
                IsLoadingAddress = "6CB600";
            }

            _offsetTransformationLocal = $"{_offsetTransformationOrigin}+4";
            _offsetTransformationWorld = $"{_offsetTransformationOrigin}+8";
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
            return _m.ReadInt($"{ActiveCharacterPointer},{_offsetGadgetPower}");
        }

        public void WriteActCharGadgetPower(int value)
        {
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetGadgetPower}", "int", value.ToString());
        }

        public override void FreezeActCharGadgetPower(int value)
        {
            if (value == 0)
            {
                value = ReadActCharGadgetPower();
            }

            _m.FreezeValue($"{ActiveCharacterPointer},{_offsetGadgetPower}", "int", value.ToString());
        }

        public override void UnfreezeActCharGadgetPower()
        {
            _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetGadgetPower}");
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

        public void WriteActCharId(int id, int id2 = -1)
        {
            _m.WriteMemory($"{ActiveCharacterIdAddress}", "int", id.ToString());
            _m.WriteMemory($"{ActiveCharacterIdAddress}+4", "int", id2.ToString());
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
            return _m.ReadInt($"{ActiveCharacterPointer},{_offsetHealth}");
        }

        public override void WriteActCharHealth(int value)
        {
            _m.WriteMemory($"{ActiveCharacterPointer},{_offsetHealth}", "int", value.ToString());
        }

        public override void FreezeActCharHealth(int value)
        {
            if (value == 0)
            {
                value = ReadActCharHealth();
            }

            _m.FreezeValue($"{ActiveCharacterPointer},{_offsetHealth}", "int", value.ToString());
        }

        public override void UnfreezeActCharHealth()
        {
            _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetHealth}");
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
            // int tmp = _m.ReadInt($"{ActiveCharacterPointer},D4");
            // _m.WriteMemory($"{ActiveCharacterPointer},D4", "int", "0");
            WriteEntityLocalTranslation(ActiveCharacterPointer, value);
            // Thread.Sleep(10);
            // _m.WriteMemory($"{ActiveCharacterPointer},D4", "int", tmp.ToString());
        }

        public override void FreezeActCharLocalTranslationX(string value)
        {
            FreezeEntityLocalTranslationX(ActiveCharacterPointer, value);
        }

        public override void FreezeActCharLocalTranslationY(string value)
        {
            FreezeEntityLocalTranslationY(ActiveCharacterPointer, value);
        }

        public override void FreezeActCharLocalTranslationZ(string value )
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
        #endregion

        #region Toggles
        public override void ToggleUndetectable(bool enableUndetectable)
        {
            if (enableUndetectable)
            {
                _m.FreezeValue($"{ActiveCharacterPointer},{_offsetUndetectable},1C", "int", "1");
            }
            else
            {
                _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetUndetectable},1C");
                _m.WriteMemory($"{ActiveCharacterPointer},{_offsetUndetectable},1C", "int", "0");
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
            if (Region == "NTSC July 16"
             || Region == "NTSC E3 Demo"
             || Region == "NTSC Regular Demo")
            {
                if (enableInfDbJump)
                {
                    _m.FreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}", "int", "1");
                }
                else
                {
                    _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}");
                }
                return;
            }

            if (enableInfDbJump)
            {
                _m.FreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}", "int", "0");
                _m.FreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}+C", "int", "0");
            }
            else
            {
                _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}");
                _m.UnfreezeValue($"{ActiveCharacterPointer},{_offsetInfiniteDbJump}+C");
            }
        }

        public void ToggleDeathBarriers(bool removeDeathBarriers)
        {
            if (removeDeathBarriers)
            {
                _m.FreezeValue(DeathBarriersAddress, "int", "0");
            }
            else
            {
                _m.UnfreezeValue(DeathBarriersAddress);
                _m.WriteMemory(DeathBarriersAddress, "int", "0x0901F0FF");
            }
        }

        #endregion

        #region Maps
        public override void LoadMap(int mapId)
        {
            byte[] data = _m.ReadBytes($"{ReloadValuesAddress}+{mapId * 0x40:X8}", 0x40);
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

        public void LoadMapFull(int mapId)
        {
            _m.WriteMemory($"{ReloadAddress}+A0", "int", "-1");
            LoadMap(mapId);
        }

        public void ReloadMap()
        {
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
                str = str.Replace("\n", "\r\n");
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
                new("Sly", 24, "jt", "sly"),
                new("Bentley", 25, "bentley", "bentley"),
                new("Murray", 26, "murray", "murray"),
                new("Guru", 29, "shaman"),
                new("Panda King", 30, "panda_king"),
                new("Penelope", 31, "penelope"),
                //new("Dimitri", 0x7443, "dimitri"),
                new("Dimitri swimmer", 0x3DDF, "dmitri_swimmer"), // 0x6C12 0x7440
                new("RC car", 0x9692, "c_rccar"), // 0x3AD7
            };
        }

        protected override List<List<Gadget_t>> GetGadgets()
        {
            return new()
            {
                new()
                {
                    new("Smoke Bomb", 0x19),
                    new("Combat Dodge", 0x1A),
                    new("Paraglide", 0x1B),
                    new("Silent Obliteration", 0x1C),
                    new("Feral Pounce", 0x1D),
                    new("Mega Jump", 0x1E),
                    new("Knockout Dive", 0x1F),
                    new("Shadow Power Level 1", 0x20),
                    new("Thief Reflexes", 0x21),
                    new("Shadow Power Level 2", 0x22),
                    new("Rocket Boots", 0x23),
                    new("Treasure Map", 0x24),
                    new("Shield", 0x25),
                    new("Venice Disguise", 0x26),
                    new("Photographer Disguise", 0x27),
                    new("Pirate Disguise", 0x28),
                },

                new()
                {
                    new("Trigger Bomb", 0x6),
                    new("Fishing Pole", 0x7),
                    new("Alarm Clock", 0x8),
                    new("Adrenaline Burst", 0x9),
                    new("Health Extractor", 0xA),
                    new("Hover Pack", 0xB),
                    new("Insanity Strike", 0xC),
                    new("Grapple-Cam", 0xD),
                    new("Size Destabilizer", 0xE),
                    new("Rage Bomb", 0xF),
                    new("Reduction Bomb", 0x10),
                },

                new()
                {
                    new("Be The Ball", 0x11),
                    new("Berserker Charge", 0x12),
                    new("Juggernaut Throw", 0x13),
                    new("Guttural Roar", 0x14),
                    new("Fists of Flame", 0x15),
                    new("Temporal Lock", 0x16),
                    new("Raging Inferno Flop", 0x17),
                    new("Diablo Fire Slam", 0x18),
                },
            };
        }

        protected override List<Map_t> GetMaps()
        {
            return new()
            {
                new("DVD Menu",
                    new()
                    {
                        new(),
                    }
                ),
                new("sampler_menu",
                    new()
                    {
                        new(),
                    },
                    false
                ),
                new("Hazard Room",
                    new()
                    {
                        new("Center", new(3550, 440, 150)),
                        new("Top", new(3580, 630, 3600)),
                        new("Safehouse", new(6640, 680, 150)),
                    }
                ),
                new("Venice Hub",
                    new()
                    {
                        new("Safehouse", new(200, -2090, 273)),
                        new("Safehouse (Top)", new(863, -1420, 1366)),
                        new("Police HQ", new(-7570, 1670, 2062)),
                        new("Ferris Wheel", new(6900, 1480, 260)),
                        new("Stage", new(6250, 8210, 360)),
                        new("Fountain", new(-6670, 8550, 800)),
                        new("Aquarium", new(8040, -4365, 260)),
                    }
                ),
                new($"{SubMapNamePrefix}Canal Chase",
                    new()
                    {
                        new("Boat", new(0, 0, 230)),
                        new("Intersection 1", new(665, -12555, 240)),
                        new("Intersection 2", new(27250, 28580, 240)),
                    }
                ),
                new($"{SubMapNamePrefix}Coffeehouses",
                    new()
                    {
                        new("Entrance 1", new(710, -5000, 225)),
                        new("Entrance 2", new(1070, 100, 225)),
                        new("Entrance 3", new(1160, 5000, 225)),
                        new("Safe 1", new(-1710, -4990, 225)),
                        new("Safe 2", new(-1750, 10, 225)),
                        new("Safe 3", new(-3245, 4990, 225)),
                        new("Roof", new(-1780, -4540, 1275)),
                    }
                ),
                new($"{SubMapNamePrefix}Gauntlet / Opera House",
                    new()
                    {
                        new("Main Entrance", new(-7130, -11340, 1130)),
                        new("Basement Entrance", new(14440, -4000, 1115)),
                        new("Pump Room", new(-885, -2230, 280)),
                        new("Worlitzer-700", new(-2100, 4890, 730)),
                        new("Underground Canal", new(8720, -6490, 175)),
                        new("Overlook", new(8770, -5830, 1750)),
                    }
                ),
                new($"{SubMapNamePrefix}Police Station",
                    new()
                    {
                        new("Dimitri's Cell", new(-60, 7600, 220)),
                        new("Cell Key", new(-685, 3250, 225)),
                    }
                ),
                new("Outback Hub",
                    new()
                    {
                        new("Safehouse", new(-4570, -7190, 1625)),
                        new("Safehouse (Top)", new(-4590, -7820, 2750)),
                        new("Crane", new(-700, -1290, 4420)),
                        new("Truck", new(9820, -550, 1340)),
                        new("Guru's Hut", new(-8230, 4365, 2860)),
                        new("Guru's Cell", new(8665, 5620, 2920)),
                        new("Treeline", new(-8360, -3400, 5160)),
                        new("Plateau", new(6360, 7645, 7030)),
                    }
                ),
                new($"{SubMapNamePrefix}Quarry / Ayers Rock",
                    new()
                    {
                        new("Drill Controls", new(270, 160, 340)),
                        new("Drill Controls (Top)", new(420, 15, 2290)),
                        new("Truck Spawn", new(-16350, 8310, 4330)),
                        new("Mine Entrance", new(3830, 13920, 170)),
                        new("Clifftop", new(16260, 12890, 12760)),
                    }
                ),
                new($"{SubMapNamePrefix}Oil Field",
                    new()
                    {
                        new("The Claw", new(320, 10000, 170)),
                        new("Catapult", new(4820, -4470, 170)),
                        new("Drill Platform", new(-360, 620, 1335)),
                    }
                ),
                new($"{SubMapNamePrefix}Cave 1 (Sly)",
                    new()
                    {
                        new("Entrance", new(-9345, 330, 120)),
                        new("Safe", new(6545, 125, 1211)),
                        new("Drills", new(-780, -3420, 1220)),
                    }
                ),
                new($"{SubMapNamePrefix}Cave 2 (Guru)",
                    new()
                    {
                        new("Entrance", new(-8945, 370, -1760)),
                        new("Safe", new(-100, -4960, -510)),
                        new("Hook Conveyor Belt", new(-5970, -1800, -1235)),
                    }
                ),
                new($"{SubMapNamePrefix}Bar",
                    new()
                    {
                        new(),
                    }
                ),
                new($"{SubMapNamePrefix}Cave 3 (Murray)",
                    new()
                    {
                        new("Entrance", new(-10230, -1445, -1040)),
                        new("Piston", new(3380, -1870, -920)),
                        new("Triple Piston", new(-2300, -8000, 250)),
                    }
                ),
                new("Holland Hub",
                    new()
                    {
                        new("Safehouse", new(12180, -540, 1280)),
                        new("Baron's Hangar", new(-6015, 6880, 2855)),
                        new("Forest", new(-2770, 3020, 530)),
                        new("Ramp", new(-4645, -9100, 1780)),
                        new("Barn", new(3680, -6000, 700)),
                    }
                ),
                new($"{SubMapNamePrefix}Hotel",
                    new()
                    {
                        new("Safehouse Entrance", new(2620, 280, 700)),
                        new("Ham", new(-535, 420, 100)),
                        new("Viking Helmet", new(830, 2950, 690)),
                        new("Outside", new(60, -6590, -445)),
                    }
                ),
                new($"{SubMapNamePrefix}Hangar (team Belgium)",
                    new()
                    {
                        new(),
                    }
                ),
                new($"{SubMapNamePrefix}Hangar (team Black Baron)",
                    new()
                    {
                        new(),
                    }
                ),
                new($"{SubMapNamePrefix}Hangar (team Cooper)",
                    new()
                    {
                        new("Center", new(-180, -125, 175)),
                        new("Control Room", new(-1890, -130, 175)),
                        new("Truck", new(-340, 2220, 1130)),
                    }
                ),
                new($"{SubMapNamePrefix}Sewers",
                    new()
                    {
                        new("Entrance", new(20150, -9850, 310)),
                        new("Iceland Hotel Path", new(16490, 7280, 310)),
                        new("Exit to Surface", new(7960, -12750, 310)),
                        new("Iceland Hotel Entrance", new(7425, 9500, 310)),
                        new("Platform", new(200, 0, 200)),
                    }
                ),
                new($"{SubMapNamePrefix}Dogfight / Biplane Battlefield",
                    new()
                    {
                        new("Barn", new(-1890, 380, 970)),
                        new("Crop Squares", new(17800, 3210, 1000)),
                        new("Bridge 1", new(-140, -14670, 720)),
                        new("Bridge 2", new(-4444, 16170, 550)),
                        new("Bridge 3", new(10460, 13260, 600)),
                        new("Plane", new(251764, -186, 100)),
                    }
                ),
                new("Two Player Hackathon",
                    new()
                    {
                        new(),
                    }
                ),
                new("China Hub",
                    new()
                    {
                        new("Safehouse", new(-5440, -7500, 2120)),
                        new("Turret Tower", new(-5330, -8415, 3600)),
                        new("Walk Across the Heavens", new(7310, -8370, 5080)),
                        new("Graveyard", new(8570, 10150, 5940)),
                        new("Statue", new(795, -2980, 2015)),
                        new("Palace", new(940, 2255, 4890)),
                    }
                ),
                new($"{SubMapNamePrefix}Intro",
                    new()
                    {
                        new("Entrance", new(-2085, -54630, 950)),
                        new("Panda King's Perch", new(400, -50485, 1988)),
                        new("House", new(3470, -51845, 920)),
                        new("Clifftop", new(-2820, -57675, 5520)),
                    }
                ),
                new($"{SubMapNamePrefix}Panda King's Flashback",
                    new()
                    {
                        new(),
                    }
                ),
                new($"{SubMapNamePrefix}Tsao's Battleground",
                    new()
                    {
                        new("Top", new(-50, 3060, 840)),
                        new("Bottom", new(130, 30410, 150)),
                        new("Overlook", new(-4545, 35970, 4775)),
                    }
                ),
                new($"{SubMapNamePrefix}Panda King's House",
                    new()
                    {
                        new("Yang", new(-240, -100, 20095)),
                        new("Yin", new(-1855, -100, 20095)),
                    }
                ),
                new($"{SubMapNamePrefix}Tsao's Business Center",
                    new()
                    {
                        new("Entrance", new(-3200, 0, 100)),
                        new("Second Floor", new(1050, 1580, 800)),
                        new("Computer", new(1075, -1515, 800)),
                        new("Outside", new(-4210, -140, 0)),
                        new("Overlook", new(-10480, -4100, 2900)),
                    }
                ),
                new($"{SubMapNamePrefix}Palace",
                    new()
                    {
                        new("Vases", new(-5270, -60, -50)),
                        new("Computer", new(-2250, 1445, 150)),
                        new("Jing King's Room", new(470, -1500, 150)),
                        new("Drill Site", new(2075, 15000, 700)),
                    }
                ),
                new($"{SubMapNamePrefix}Treasure Temple",
                    new()
                    {
                        new("Entrance", new(-6300, -130, 500)),
                        new("Treasure Area", new(1725, 730, -200)),
                        new("Crawlspace", new(-560, 140, 1800)),
                    }
                ),
                new("Pirate Hub",
                    new()
                    {
                        new("Safehouse", new(4900, 1345, 1225)),
                        new("Safehouse (Top)", new(5590, 2310, 2780)),
                        new("Skull Keep (Top)", new(-9600, -1880, 4510)),
                        new("Waterfall (Top)", new(3625, 16360, 4535)),
                        new("Fireplace", new(-530, 7030, 2070)),
                        new("Monkeys?", new(-7415, 11565, 1620)),
                        new("Cooper Gang Ship", new(11390, -9290, 1650)),
                        new("Archipelago", new(-26550, -19930, 2200)),
                    }
                ),
                new($"{SubMapNamePrefix}Sailing Map",
                    new()
                    {
                        new(),
                    }
                ),
                new($"{SubMapNamePrefix}Underwater Shipwreck",
                    new()
                    {
                        new("Spawn", new(28980, -100, 2800)),
                        new("Ship (Top)", new(21020, 14180, 6030)),
                        new("Shipwreck", new(22460, 12380, -3280)),
                        new("Depths", new(21910, 8690, -7085)),
                        new("Ocean Current", new(20860, 22410, -6920)),

                    }
                ),
                new($"{SubMapNamePrefix}Dagger Island",
                    new()
                    {
                        new("Cooper Gang Ship", new(-16760, 2940, 1000)),
                        new("Palm Tree Circle", new(-8040, -970, 1200)),
                        new("Flipped Ship", new(1620, -5250, 1240)),
                        new("Pirate Ship", new(15680, 5290, 870)),
                        new("Mountain Peak", new(2215, 10860, 8040)),
                    }
                ),
                new("Kaine Island",
                    new()
                    {
                        new("Spawn", new(-6715, -14380, -2800)),
                        new("Wall Sneak (Top)", new(-6285, -2220, -2080)),
                        new("Ventilation Shaft", new(-1870, 4765, -3755)),
                        new("Vault Entrance", new(-1085, -100, 2460)),
                        new("Ship Dock", new(7855, -24360, -3670)),
                        new("RC Car Track", new(-13650, -14110, -2090)),
                        new("Random Rope", new(-16480, 1520, -2730)),
                        new("Rock Formation", new(13730, 20650, 2200)),
                    }
                ),
                new($"{SubMapNamePrefix}Underwater",
                    new()
                    {
                        new("Spawn", new(51570, 23850, -5745)),
                        new("Water Tube", new(745, -34265, -720)),
                        new("Boss Area", new(10200, -59780, 0)),
                    }
                ),
                new($"{SubMapNamePrefix}Cooper Vault (entrance)",
                    new()
                    {
                        new("Center", new(0, 0, 140)),
                        new("Entrance Door", new(4350, -45, 560)),
                    }
                ),
                new($"{SubMapNamePrefix}Cooper Vault (gauntlet)",
                    new()
                    {
                        new("Slytunkhamen II", new(-28690, 21665, -2075)),
                        new("Sir Galleth Cooper", new(-24715, 13995, -2160)),
                        new("Salim Al-Kupar", new(-13760, 12485, -2100)),
                        new("Slaigh MacCooper", new(-15325, 24680, -2090)),
                        new("Rioichi Cooper", new(-21130, 19955, -80)),
                        new("Henriette Cooper", new(-10530, 13200, 220)),
                        new("Tennesee 'Kid' Cooper", new(2010, 13275, -2180)),
                        new("Thaddeus Winslow Cooper III", new(9360, 1820, -2085)),
                        new("Otto Van Cooper", new(-2050, 2740, 100)),
                        new("Conner Cooper", new(7515, 5030, 250)),
                        new("Inner Sanctum Entrance", new(16645, -2260, 220)),
                    }
                ),
                new($"{SubMapNamePrefix}Dr. M's Arena",
                    new()
                    {
                        new("Center", new(0, 0, 130)),
                        new("Top", new(-3840, 1600, 2970)),
                    }
                ),
            };
        }
    }
}
