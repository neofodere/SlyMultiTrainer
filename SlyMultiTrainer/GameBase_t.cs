using System.Numerics;
using static SlyMultiTrainer.Util;

namespace SlyMultiTrainer
{
    public abstract class GameBase_t
    {
        private Form1 _form;
        private Memory.Mem _m;
        private int _lastMapId;
        private int _lastActCharId;
        public bool _isFirstLoopAfterLoading;
        public string Region;

        protected string FOVAddress;
        protected string ClockAddress;
        protected string DrawDistanceAddress;
        protected string ResetCameraAddress;
        protected string CoinsAddress;
        protected string MapIdAddress;
        protected string GadgetAddress;
        protected string GuardAIAddress;
        protected Character_t ActiveCharacter;

        public List<Character_t> Characters;
        public List<List<Gadget_t>> Gadgets;
        public List<Map_t> Maps;

        protected GameBase_t(Memory.Mem m, Form1 form, string region)
        {
            _form = form;
            _m = m;
            Region = region;
            _isFirstLoopAfterLoading = true;
            Characters = GetCharacters();
            Gadgets = GetGadgets();
            Maps = GetMaps();
        }

        // Methods that are that and nothing else:
        //     public [type] [name]() { [implementation] }
        // Methods that must be implemented by the game (they are game specific):
        //     public abstract [type] [name]();
        // Methods that have default implementation but can be overridden by the game:
        //     public virtual [type] [name]() { [default implementation] }

        // Main loop tick for all games
        public void OnLoopTick()
        {
            // Shared logic
            _form.UpdateUI(_form.trkFOV, ReadFOV() * 10);
            _form.UpdateUI(_form.trkClock, ReadClock() * 10);
            _form.UpdateUI(_form.trkDrawDistance, ReadDrawDistance() * 10);

            var mapId = GetMapId() + 1; // + first item for current map
            if (mapId != 0 && mapId != _lastMapId)
            {
                OnMapChange(mapId);
            }

            if (!IsLoading())
            {
                if (_isFirstLoopAfterLoading)
                {
                    _isFirstLoopAfterLoading = false;
                    if (this is Sly2Handler)
                    {
                        (this as Sly2Handler).Savefile.Init();
                    }
                    else if (this is Sly3Handler)
                    {
                        (this as Sly3Handler).Savefile.Init();

                        // In sly 3, not all characters are available in all maps (e.g. ep1 police station only has sly)
                        // So, we need to filter the character list based on the entities list

                        var list = (this as Sly3Handler).GetFKXList();
                        List<Character_t> newCharacters = new(Characters);
                        for (int i = 0; i < newCharacters.Count; i++)
                        {
                            var character = newCharacters[i];
                            var fkEntity = list.FirstOrDefault(x => x.Name == character.InternalName);
                            if (fkEntity == null || fkEntity.SpawnRule == 0)
                            {
                                // Remove if not found or its spawn rule is 0 (shaman in kaine island)
                                newCharacters.Remove(character);
                                i--;
                                continue;
                            }

                            // Sly 3 ntsc e3 demo doesn't have the same ids as retail
                            // So let's read them on the fly
                            int id = _m.ReadInt((fkEntity.EntityAddress[0] + 0x18).ToString("X"));
                            newCharacters[i].Id = id;
                        }

                        if (newCharacters.Count != 0)
                        {
                            if (!newCharacters.SequenceEqual((List<Character_t>)_form.cmbActChar.DataSource))
                            {
                                _form.UpdateUI(() =>
                                {
                                    var last = _form.cmbActChar.SelectedItem;
                                    _form.cmbActChar.DataSource = newCharacters;
                                    if (newCharacters.Contains(last))
                                    {
                                        // If the new map contains the latest character, automatically select it
                                        _form.cmbActChar.SelectedItem = last;
                                    }
                                });
                            }
                        }
                    }
                }

                UpdateActChar();

                string tabName = "";
                _form.UpdateUI(() =>
                {
                    tabName = _form.tabControlMain.SelectedTab.Name;
                });

                if (tabName == "tabEntities")
                {
                    UpdateEntities();
                }
                else if (tabName == "tabDAG")
                {
                    UpdateDAG();
                }
                else if (tabName == "tabStrings")
                {
                    UpdateStrings();
                }
            }
            else
            {
                _isFirstLoopAfterLoading = true;
                _form.UpdateUI(_form.grpGadgets, false, "Enabled");
                _lastActCharId = 0;
            }

            // Game specific logic
            CustomTick();
        }

        void UpdateActChar()
        {
            // Update active character in the dropdown
            // Sly 1 only has sly as playable
            if (this is not Sly1Handler)
            {
                _form.UpdateUI(() =>
                {
                    if (!_form.cmbActChar.DroppedDown)
                    {
                        var actCharId = _m.ReadInt($"{GetActCharPointer()},18");
                        if (_lastActCharId != actCharId
                         || _isFirstLoopAfterLoading)
                        {
                            _lastActCharId = actCharId;

                            var characters = _form.cmbActChar.DataSource as List<Character_t>;
                            int currentCharacter = characters.FindIndex(x => x.Id == actCharId);
                            if (currentCharacter == -1
                                || characters[currentCharacter].NameForSavefile == "")
                            {
                                _form.grpGadgets.Enabled = false;
                                _form.cmbGadgetL1.SelectedIndex = 0;
                                _form.cmbGadgetL2.SelectedIndex = 0;
                                _form.cmbGadgetR2.SelectedIndex = 0;
                                return;
                            }

                            _form.grpGadgets.Enabled = true;
                            _form.cmbActChar.SelectedIndex = currentCharacter;
                            ActiveCharacter = characters[currentCharacter];
                            List<Gadget_t> characterGadgetsL1 = new(Gadgets[currentCharacter]);
                            List<Gadget_t> characterGadgetsL2 = new(Gadgets[currentCharacter]);
                            List<Gadget_t> characterGadgetsR2 = new(Gadgets[currentCharacter]);
                            _form.cmbGadgetL1.DataSource = characterGadgetsL1;
                            _form.cmbGadgetL2.DataSource = characterGadgetsL2;
                            _form.cmbGadgetR2.DataSource = characterGadgetsR2;
                        }
                    }
                });
            }

            if (!IsActCharAvailable())
            {
                _form.UpdateUI(_form.lblXCoord, DefaultValueFloat);
                _form.UpdateUI(_form.lblYCoord, DefaultValueFloat);
                _form.UpdateUI(_form.lblZCoord, DefaultValueFloat);
                _form.UpdateUI(_form.chkActCharHealthFreeze, DefaultValueInt);
                return;
            }

            // Position and health
            Vector3 position = ReadActCharLocalTranslation();
            _form.UpdateUI(_form.lblXCoord, position.X);
            _form.UpdateUI(_form.lblYCoord, position.Y);
            _form.UpdateUI(_form.lblZCoord, position.Z);
            _form.UpdateUI(_form.chkActCharHealthFreeze, ReadActCharHealth());

            // Fly logic
            if (_form.chkActCharFly.Checked)
            {
                Controller_t controller = GetController();
                string FlyButtonUp = Properties.Settings.Default.FlyButtonUp;
                string FlyButtonDown = Properties.Settings.Default.FlyButtonDown;
                string FlyButtonAccelerate = Properties.Settings.Default.FlyButtonAccelerate;

                if (controller.IsButtonPressed(FlyButtonAccelerate))
                {
                    FreezeActCharSpeedMultiplier(DefaultAmountToIncreaseOrDecreaseTranslationForActChar / 50);
                }
                else
                {
                    UnfreezeActCharSpeedMultiplier();
                    WriteActCharSpeedMultiplier(1);
                }

                if (controller.IsButtonPressed(FlyButtonUp))
                {
                    // up
                    //    unfreeze Z
                    //    set velocity Z to 500, keep freeze
                    UnfreezeActCharLocalTranslationZ();

                    if (controller.IsButtonPressed(FlyButtonAccelerate))
                    {
                        FreezeActCharVelocityZ((DefaultAmountToIncreaseOrDecreaseTranslationForActChar * 7).ToString());
                    }
                    else
                    {
                        FreezeActCharVelocityZ((DefaultAmountToIncreaseOrDecreaseTranslationForActChar * 3).ToString());
                    }

                }
                else if (controller.IsButtonPressed(FlyButtonDown))
                {
                    // down
                    //    unfreeze Z
                    //    set velocity Z to -500, keep freeze
                    UnfreezeActCharLocalTranslationZ();

                    if (controller.IsButtonPressed(FlyButtonAccelerate))
                    {
                        FreezeActCharVelocityZ((-DefaultAmountToIncreaseOrDecreaseTranslationForActChar * 7).ToString());
                    }
                    else
                    {
                        FreezeActCharVelocityZ((-DefaultAmountToIncreaseOrDecreaseTranslationForActChar * 3).ToString());
                    }
                }
                else
                {
                    // idle
                    //    freeze Z to current
                    //    freeze velocity Z to 0

                    // Using position.Z which is read a bit earlier makes the character stutter,
                    // so let's get the latest value possible by reading the position again
                    FreezeActCharLocalTranslationZ(ReadActCharLocalTranslation().Z.ToString());
                    FreezeActCharVelocityZ("0");

                    // It is possible that while in this if scope, the user disabled the fly function
                    // Let's check it again to see if we should keep the position and velocity frozen
                    if (!_form.chkActCharFly.Checked)
                    {
                        UnfreezeActCharVelocityZ();

                        // But only unfreeze the Z position if the checkbox for the z coordinate of the active character is not frozen
                        if (!_form.chkActCharZCoordFreeze.Checked)
                        {
                            UnfreezeActCharLocalTranslationZ();
                        }
                    }
                }
            }

            // Read gadget binds
            if (this is not Sly1Handler)
            {
                _form.UpdateUI(() =>
                {
                    if (!_form.grpGadgets.Visible
                     || !_form.grpGadgets.Enabled)
                    {
                        // For builds or characters that don't have gadgets
                        return;
                    }

                    UpdateActCharGadgetBind(_form.cmbGadgetL1, GADGET_BIND.L1);
                    UpdateActCharGadgetBind(_form.cmbGadgetL2, GADGET_BIND.L2);
                    UpdateActCharGadgetBind(_form.cmbGadgetR2, GADGET_BIND.R2);
                });
            }
        }

        void UpdateActCharGadgetBind(ComboBox cmbGadget, GADGET_BIND bind)
        {
            if (!cmbGadget.DroppedDown
                && !cmbGadget.ContainsFocus)
            {
                if (cmbGadget.DataSource is not List<Gadget_t> gadgets)
                {
                    return;
                }

                int gadgetId = ReadActCharGadgetId(bind);

                // Default to none
                int selectedIndex = 0;

                // Find the index of the gadget in the list only if a gadget is binded
                if (gadgetId != 0 && gadgetId != -1)
                {
                    selectedIndex = gadgets.FindIndex(x => x.Id == gadgetId);
                }

                cmbGadget.SelectedIndex = selectedIndex;
            }
        }

        void UpdateEntities()
        {
            if (_form.trvFKXList.Nodes.Count == 0)
            {
                _form.UpdateUI(() =>
                {
                    if (!_form.txtEntitiesSearch.Focused)
                    {
                        _form.btnRefreshFKXList_Click(_form.btnRefreshFKXList, EventArgs.Empty);
                    }
                });
            }

            int pointerToEntity = 0;
            _form.UpdateUI(() =>
            {
                pointerToEntity = _form.GetPointerToEntityFromSelectedFKXNode();
            });

            if (pointerToEntity == 0)
            {
                return;
            }

            Vector3 localTrans = ReadEntityLocalTranslation(pointerToEntity.ToString("X"));
            _form.UpdateUI(_form.lblFKXEntityXCoord, localTrans.X);
            _form.UpdateUI(_form.lblFKXEntityYCoord, localTrans.Y);
            _form.UpdateUI(_form.lblFKXEntityZCoord, localTrans.Z);

            Vector3 worldTrans = ReadEntityFinalTranslation(pointerToEntity.ToString("X"));
            _form.UpdateUI(_form.lblFKXEntityXCoordWorld, worldTrans.X);
            _form.UpdateUI(_form.lblFKXEntityYCoordWorld, worldTrans.Y);
            _form.UpdateUI(_form.lblFKXEntityZCoordWorld, worldTrans.Z);

            float scale = ReadEntityLocalScale(pointerToEntity.ToString("X"));
            _form.UpdateUI(_form.trkFKXEntityScale, scale * 10);

            // Read rotation only if the edit checkbox is not checked
            if (!_form.chkFKXEntityEditRotation.Checked)
            {
                Matrix4x4 rotationMatrix = ReadEntityWorldTransformation(pointerToEntity.ToString("X"));
                var euler = ExtractEulerAngles(rotationMatrix);
                _form.UpdateUI(_form.trkFKXEntityRotationX, euler.X);
                _form.UpdateUI(_form.trkFKXEntityRotationY, euler.Y);
                _form.UpdateUI(_form.trkFKXEntityRotationZ, euler.Z);
            }
        }

        void UpdateDAG()
        {
            DAG_t DAG;
            Sly2_3_Savefile savefile;
            if (this is Sly2Handler)
            {
                DAG = (this as Sly2Handler).DAG;
                savefile = (this as Sly2Handler).Savefile;
            }
            else
            {
                DAG = (this as Sly3Handler).DAG;
                savefile = (this as Sly3Handler).Savefile;
            }

            if (DAG.Graph == null)
            {
                DAG.Init(savefile);
                DAG.GetDAG();
                if (!DAG.SetGraph())
                {
                    return;
                }

                _form.UpdateUI(DAG.Viewer, true, "Enabled");
            }

            // We check some fields in every node in the loaded dag
            // to see if there is a mismatch between what we have on the dag and the value in-game
            // If there is a mismatch, either the in-game dag was changed by the game
            // or the user forcefully changed a field of the node

            // This variable is used to trigger a redraw
            // UNUSED FOR NOW
            bool redrawGraph = true;

            string currentCheckpointAddress = DAG.GetCurrentCheckpointAddress();
            for (int i = 0; i < DAG.Tasks.Count; i++)
            {
                Task_t task = DAG.Tasks[i];
                Task_t taskInGame = DAG.ReadTask(task.Address, false);

                // When loading a save state or loading a save file, it is possible to have a mismatch between what we have on the dag and the value in-game
                // So, we make sure the task we are reading is the task we are looking for
                // If it's not, then we trigger a refresh of the entire dag
                if (task.Id != taskInGame.Id)
                {
                    _form.UpdateUI(() =>
                    {
                        DAG.TriggerRefresh();
                    });

                    return;
                }

                // If there is a mismatch between what is in game and what we have on the trainer
                // = something changed
                bool areTasksEqual = DAG.IsTaskEqualToTask(task, taskInGame);
                if (!areTasksEqual)
                {
                    redrawGraph = true;
                    task.State = taskInGame.State;
                    task.FocusCount = taskInGame.FocusCount;
                    task.CompleteCount = taskInGame.CompleteCount;
                }

                // Update the node's color
                task.MsaglNode.Attr.FillColor = DAG.GetNodeColorFromState(task.State);

                // Update the cluster's color only if this is the first node of the cluster
                if (task.Cluster.Tasks.FirstOrDefault() == task)
                {
                    task.Cluster.Subgraph.Attr.FillColor = DAG.GetClusterColorFromState(task.State);
                }

                // Check if it's the current checkpoint
                if (task.Address == currentCheckpointAddress)
                {
                    if (!task.MsaglNode.Attr.Styles.Contains(Microsoft.Msagl.Drawing.Style.Dashed))
                    {
                        redrawGraph = true;
                        task.MsaglNode.Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Dashed);
                    }

                    if (!DAG.IsNodeSelected(task.MsaglNode))
                    {
                        task.MsaglNode.Attr.LineWidth = DAG.NodeCurrentCheckpointDefaultLineWidth;
                    }
                }
                else if (task.MsaglNode.Attr.Styles.Any())
                {
                    // previous checkpoint
                    if (task.MsaglNode.Attr.Styles.Contains(Microsoft.Msagl.Drawing.Style.Dashed))
                    {
                        if (DAG.IsNodeSelected(task.MsaglNode))
                        {
                            task.MsaglNode.Attr.LineWidth = DAG.SelectedNodeDefaultLineWidth;
                        }
                        else
                        {
                            task.MsaglNode.Attr.LineWidth = DAG.NodeDefaultLineWidth;
                        }
                    }

                    task.MsaglNode.Attr.ClearStyles();
                }
            }

            for (int i = 0; i < DAG.Clusters.Count; i++)
            {
                Cluster_t cluster = DAG.Clusters[i];
                Cluster_t clusterInGame = DAG.ReadCluster(cluster.Address, true);

                bool areClustersEqual = DAG.IsClusterEqualToCluster(cluster, clusterInGame);
                if (!areClustersEqual)
                {
                    cluster.Suck = clusterInGame.Suck;
                }
            }

            if (redrawGraph)
            {
                DAG.Viewer.Invalidate();
            }
        }

        void UpdateStrings()
        {
            string tabName = "";
            _form.UpdateUI(() =>
            {
                tabName = _form.tabControlStrings.SelectedTab.Name;
            });

            if (tabName == "tabPageLocalized")
            {
                if (!string.IsNullOrEmpty(_form.txtStringsLocalized.Text))
                {
                    return;
                }

                List<(int id, string str)> list;
                if (this is Sly2Handler)
                {
                    list = (this as Sly2Handler).GetStringTable(true);
                }
                else
                {
                    list = (this as Sly3Handler).GetStringTable(true);
                }

                var output = $"Id - String{Environment.NewLine}";
                output += string.Join(Environment.NewLine, list.Select(i => $"{i.id:X} - {i.str}"));
                _form.UpdateUI(_form.txtStringsLocalized, output);
            }
            else if (tabName == "tabPageSavefile")
            {
                if (!string.IsNullOrEmpty(_form.txtStringsSavefile.Text))
                {
                    return;
                }

                List<(int id, string str)> list;
                if (this is Sly2Handler)
                {
                    list = (this as Sly2Handler).Savefile.GetSavefileKeyAddressTable(true);
                }
                else
                {
                    list = (this as Sly3Handler).Savefile.GetSavefileKeyAddressTable(true);
                }

                var output = $"Id + SubId - Address - Group - Property{Environment.NewLine}";
                output += string.Join(Environment.NewLine, list.Select(i => $"{i.str}"));
                _form.UpdateUI(_form.txtStringsSavefile, output);
            }
        }

        public abstract void CustomTick();

        public void OnMapChange(int mapId)
        {
            // On map change
            _isFirstLoopAfterLoading = true;
            _lastMapId = mapId;
            _form.UpdateUI(_form.cmbWarps, Maps[mapId].Warps);

            // Reset entities, dag and the strings which are all map dependent
            if (this is Sly2Handler || this is Sly3Handler)
            {
                DAG_t DAG = this is Sly2Handler ? (this as Sly2Handler).DAG : (this as Sly3Handler).DAG;
                _form.UpdateUI(() =>
                {
                    _form.trvFKXList.Nodes.Clear();
                    _form.txtStringsLocalized.Text = "";
                    DAG.TriggerRefresh();
                });
            }
        }

        public abstract bool IsLoading();

        #region Gadgets
        public virtual long ReadGadgets()
        {
            return _m.ReadLong(GadgetAddress);
        }

        public void ToggleAllGadgets()
        {
            if (this is Sly1Handler)
            {
                int gadgets = (int)(this as Sly1Handler).ReadGadgets();
                if (gadgets == -1)
                {
                    _m.WriteMemory(GadgetAddress, "int", "0");
                }
                else
                {
                    _m.WriteMemory(GadgetAddress, "int", (-1).ToString());
                }
            }
            else if (this is Sly2Handler)
            {
                long gadgets = ReadGadgets();
                if (gadgets == -1)
                {
                    _m.WriteMemory(GadgetAddress, "long", "0");
                }
                else
                {
                    _m.WriteMemory(GadgetAddress, "long", (-1).ToString());
                }
            }
            else
            {
                long gadgets = ReadGadgets();
                if (gadgets == -1)
                {
                    string value = "0x00000200000200FE";
                    // Some of the "gadgets" are actually essential skillset
                    // For example sly's square attack, binocucom, or bentley mines
                    // The following value is the value set by the game when loading a new game
                    if (Region == "NTSC July 16"
                        || Region == "NTSC Regular Demo")
                    {
                        value = "0x00000800000200FE";
                    }
                    else if (Region == "NTSC E3 Demo")
                    {
                        value = "0";
                    }

                    _m.WriteMemory(GadgetAddress, "long", value);
                }
                else
                {
                    _m.WriteMemory(GadgetAddress, "long", (-1).ToString());
                }
            }
        }

        public abstract void FreezeActCharGadgetPower(int value = 0);
        public abstract void UnfreezeActCharGadgetPower();
        public abstract int ReadActCharGadgetId(GADGET_BIND bind);
        public abstract void WriteActCharGadgetId(GADGET_BIND bind, int value);
        #endregion

        #region Coins
        public void SetCoins(int value)
        {
            _m.WriteMemory(CoinsAddress, "int", value.ToString());
        }
        #endregion

        #region Entities
        // Sly 1 only has 1 transformation component (2 4x4 matrices)

        // Sly 2 and 3 have 4 transformation components (2 4x4 matrices per transformation component; 8 4x4 matrices in total)
        // Sometimes, the last 2 transformation components are the same (e.g. not the case for carmelita in sly 3)
        // The first transformation component is the origin
        // The second transformation component is the local transformation
        // The third transformation component is the world transformation
        // The fourth transformation component is (usually) the same as the world transformation. We can call it "final transformation"
        // Each transformation component has 2 4x4 transformation matrices. One at +0x0 and one at +0x40
        // The one at +0x0 is write-able and is relative transformation from the previous transformation component
        // The one at +0x40 is not write-able and it's the multiplication of the matrix at +0x0 and the matrix at +0x40 of the previous transformation component

        public abstract bool EntityHasTransformation(string pointerToEntity);
        #region Origin
        public abstract Matrix4x4 ReadEntityOriginTransformation(string pointerToEntity);
        #endregion

        #region Local
        public abstract Matrix4x4 ReadEntityLocalTransformation(string pointerToEntity);
        public abstract Vector3 ReadEntityLocalTranslation(string pointerToEntity);
        public abstract void WriteEntityLocalTranslation(string pointerToEntity, Vector3 value);
        public abstract void FreezeEntityLocalTranslationX(string pointerToEntity, string value = "");
        public abstract void FreezeEntityLocalTranslationY(string pointerToEntity, string value = "");
        public abstract void FreezeEntityLocalTranslationZ(string pointerToEntity, string value = "");
        public abstract void UnfreezeEntityLocalTranslationX(string pointerToEntity);
        public abstract void UnfreezeEntityLocalTranslationY(string pointerToEntity);
        public abstract void UnfreezeEntityLocalTranslationZ(string pointerToEntity);
        public abstract float ReadEntityLocalScale(string pointerToEntity);
        public abstract void WriteEntityLocalScale(string pointerToEntity, float value);
        #endregion

        #region World
        public abstract Matrix4x4 ReadEntityWorldTransformation(string pointerToEntity);
        public abstract void WriteEntityWorldTransformation(string pointerToEntity, Matrix4x4 value);
        #region Final
        public abstract Vector3 ReadEntityFinalTranslation(string pointerToEntity);
        #endregion

        #endregion

        public void WarpSourceEntityToPoint(string pointerToSourceEntity, Vector3 point)
        {
            if (pointerToSourceEntity == "")
            {
                pointerToSourceEntity = GetActCharPointer();
            }

            if (this is Sly2Handler || this is Sly3Handler)
            {
                // Convert warp position to local space (sly 2 ep1 npc_boar_guard, sly 3 carmelita)
                Matrix4x4 originMatrix = ReadEntityOriginTransformation(pointerToSourceEntity);
                Matrix4x4 warpMatrix = Matrix4x4.CreateTranslation(point);
                Matrix4x4.Invert(originMatrix, out Matrix4x4 originInverse);
                Matrix4x4 local = warpMatrix * originInverse;
                point = local.Translation;
            }

            WriteEntityLocalTranslation(pointerToSourceEntity, point);
        }

        public void WarpSourceEntityToDestEntity(string pointerToSourceEntity, string pointerToDestEntity)
        {
            if (pointerToDestEntity == "")
            {
                pointerToDestEntity = GetActCharPointer();
            }

            Vector3 point = ReadEntityFinalTranslation(pointerToDestEntity);
            WarpSourceEntityToPoint(pointerToSourceEntity, point);
        }

        #endregion

        #region Active character
        public abstract bool IsActCharAvailable();
        public abstract string GetActCharPointer();
        public abstract int ReadActCharId();
        public abstract void WriteActCharId(int id);
        public abstract void FreezeActCharId(string value = "");
        public abstract void UnfreezeActCharId();
        public abstract int ReadActCharHealth();
        public abstract void WriteActCharHealth(int value);
        public abstract void FreezeActCharHealth(int value = 0);
        public abstract void UnfreezeActCharHealth();
        public abstract Matrix4x4 ReadActCharOriginTransformation();
        public abstract Vector3 ReadActCharLocalTranslation();
        public abstract void WriteActCharLocalTranslation(Vector3 value);
        public abstract void FreezeActCharLocalTranslationX(string value = "");
        public abstract void FreezeActCharLocalTranslationY(string value = "");
        public abstract void FreezeActCharLocalTranslationZ(string value = "");
        public abstract void UnfreezeActCharLocalTranslationX();
        public abstract void UnfreezeActCharLocalTranslationY();
        public abstract void UnfreezeActCharLocalTranslationZ();
        public abstract void FreezeActCharVelocityZ(string value = "");
        public abstract void UnfreezeActCharVelocityZ();
        public abstract float ReadActCharSpeedMultiplier();
        public abstract void WriteActCharSpeedMultiplier(float value);
        public abstract void FreezeActCharSpeedMultiplier(float value);
        public abstract void UnfreezeActCharSpeedMultiplier();
        #endregion

        #region Toggles
        public abstract void ToggleUndetectable(bool enableUndetectable);
        public abstract void ToggleInvulnerable(bool enableInvulnerable);
        public abstract void ToggleInfiniteDbJump(bool enableInfDbJump);
        public virtual void ToggleGuardAI(bool disableGuardAI)
        {
            if (disableGuardAI)
            {
                _m.FreezeValue(GuardAIAddress, "int", "1");
            }
            else
            {
                _m.UnfreezeValue(GuardAIAddress);
                _m.WriteMemory(GuardAIAddress, "int", "0");
            }
        }
        #endregion

        #region Camera
        public void ResetCamera()
        {
            _m.WriteMemory(ResetCameraAddress, "int", "1");
        }

        public float ReadFOV()
        {
            return _m.ReadFloat($"{FOVAddress}");
        }

        public void WriteFOV(float value)
        {
            _m.WriteMemory($"{FOVAddress}", "float", value.ToString());
        }

        public void FreezeFOV(float value = 0)
        {
            if (value == 0)
            {
                value = ReadFOV();
            }

            _m.FreezeValue($"{FOVAddress}", "float", value.ToString());
        }

        public void UnfreezeFOV()
        {
            _m.UnfreezeValue($"{FOVAddress}");
        }

        public float ReadDrawDistance()
        {
            return _m.ReadFloat($"{DrawDistanceAddress}");
        }

        public void WriteDrawDistance(float value)
        {
            _m.WriteMemory($"{DrawDistanceAddress}", "float", value.ToString());
        }

        public void FreezeDrawDistance(float value = 0)
        {
            if (value == 0)
            {
                value = ReadDrawDistance();
            }

            _m.FreezeValue($"{DrawDistanceAddress}", "float", value.ToString());
        }

        public void UnfreezeDrawDistance()
        {
            _m.UnfreezeValue($"{DrawDistanceAddress}");
        }
        #endregion

        #region Clock
        public float ReadClock()
        {
            return _m.ReadFloat($"{ClockAddress}");
        }

        public void WriteClock(float value)
        {
            _m.WriteMemory($"{ClockAddress}", "float", value.ToString());
        }

        public void FreezeClock(float value = 0)
        {
            if (value == 0)
            {
                value = ReadClock();
            }

            _m.FreezeValue($"{ClockAddress}", "float", value.ToString());
        }

        public void UnfreezeClock()
        {
            _m.UnfreezeValue($"{ClockAddress}");
        }
        #endregion

        #region Maps
        public virtual int GetMapId()
        {
            return _m.ReadInt(MapIdAddress);
        }
        public abstract void LoadMap(int mapId);
        public abstract void LoadMap(int mapId, int entranceValue);
        public abstract void LoadMap(int mapId, int entranceValue, int mode);
        #endregion

        public abstract Controller_t GetController();
        protected abstract List<Character_t> GetCharacters();
        protected abstract List<List<Gadget_t>> GetGadgets();
        protected abstract List<Map_t> GetMaps();
    }
}
