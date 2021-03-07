﻿using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    public partial class ItemUser
    {
        protected new class Lineage : World.Character.Lineage
        {
            public Lineage(string firstCharacterName, int initialLegacy, string name) : 
                base(firstCharacterName, initialLegacy, name) { }

            protected override void OnSpawnSuccessor()
            {
                Point point = World.GetRandomPassablePosition();
                new ItemUser(point.Y, point.X, firstCharacterName, new Settings(), this);
            }
        }
    }
}
