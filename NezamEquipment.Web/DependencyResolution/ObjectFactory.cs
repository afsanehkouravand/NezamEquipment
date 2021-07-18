﻿using System;
using System.Threading;
using NezamEquipment.Web.DependencyResolution.Registery;
using StructureMap;

namespace NezamEquipment.Web.DependencyResolution
{
    public static class ObjectFactory
    {
        private static readonly Lazy<Container> ContainerBuilder =
            new Lazy<Container>(DefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return ContainerBuilder.Value; }
        }

        private static Container DefaultContainer()
        {
            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<AutoMapperRegistry>();
                c.AddRegistry<AspNetIdentityRegistery>();
            });
        }
    }
}