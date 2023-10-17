﻿using CommonControls;
using CommonControls.Services.ToolCreation;
using Microsoft.Extensions.DependencyInjection;
using System;
using View3D;

namespace TestCommon.Utility
{
    public class TestApplication
    {
        readonly DependencyContainer[] _dependencyContainers = new DependencyContainer[]
        {
            new CommonControls_DependencyInjectionContainer(false),
            new View3D_DependencyContainer(),
            new Test_DependencyContainer(),
        };

        IServiceProvider? _serviceProvider;

        public TestApplication()
        {
        }

        public T GetService<T>() where T : notnull => _serviceProvider!.GetRequiredService<T>();

        public TestApplication Build()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider(validateScopes: true);
            RegisterTools(GetService<IToolFactory>());

            return this;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            foreach (var container in _dependencyContainers)
                container.Register(services);
        }

        void RegisterTools(IToolFactory factory)
        {
            foreach (var container in _dependencyContainers)
                container.RegisterTools(factory);
        }
    }

}
