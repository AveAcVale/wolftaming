using Vintagestory.API.Client;
using Vintagestory.API.Config;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using System.IO;

namespace WolfTaming
{
    public class EntityPet : EntityAgent
    {

        protected InventoryBase inv;
        public override IInventory GearInventory => inv;
        public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
        {
            base.Initialize(properties, api, InChunkIndex3d);
            if (inv == null) inv = new InventoryPet(Code.Path, "petInv-" + EntityId, api);
            else inv.Api = api;
            inv.LateInitialize(inv.InventoryID, api);
        }

        public override void OnTesselation(ref Shape entityShape, string shapePathForLogging)
        {
            base.OnTesselation(ref entityShape, shapePathForLogging);
            foreach (var slot in GearInventory)
            {
                addGearToShape(slot, entityShape, shapePathForLogging);
            }
        }

        public override void FromBytes(BinaryReader reader, bool forClient)
        {
            base.FromBytes(reader, forClient);

            if (inv == null) { inv = new InventoryPet(Code.Path, "petInv-" + EntityId, null); }
            inv.FromTreeAttributes(getInventoryTree());
        }

        public override void ToBytes(BinaryWriter writer, bool forClient)
        {
            inv.ToTreeAttributes(getInventoryTree());

            base.ToBytes(writer, forClient);
        }

        private ITreeAttribute getInventoryTree()
        {
            if (!WatchedAttributes.HasAttribute("petinventory"))
            {
                ITreeAttribute tree = new TreeAttribute();
                inv.ToTreeAttributes(tree);
                WatchedAttributes.SetAttribute("petinventory", tree);
            }
            return WatchedAttributes.GetTreeAttribute("petinventory");
        }
    }
}