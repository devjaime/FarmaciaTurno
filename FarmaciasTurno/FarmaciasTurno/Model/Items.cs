﻿using System;

namespace FarmaciasTurno.Model
{
    public class Items
    {
        public Items(string icon, string name, Type pageType, string description, params string[] tags)
        {
            Icon = icon;
            Name = name;
            Description = description;
            PageType = pageType;
            Tags = tags ?? new string[0];
        }

        public string Icon { get; }

        public string Name { get; }

        public string Description { get; }

        public Type PageType { get; }

        public string[] Tags { get; }
    }
}

