using Akka.DI.AutoFac;
using Akka.TestKit.Xunit2;
using Autofac;
using Minaret.Actors;
using Minaret.Helpers;
using Minaret.Helpers.Interfaces;
using Moq;
using Xunit;

namespace Minaret.Tests
{
    public class MinaretTestKit : TestKit
    {
        private IContainer _container;
        public MinaretTestKit()
        {
            ConfigureDependencyResolver();
        }

        protected T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        private void ConfigureDependencyResolver()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ClusterManagerActor>();

            var clusterWrapper = new Mock<IClusterWrapper>();
            builder.RegisterInstance(clusterWrapper.Object).As<IClusterWrapper>();
            builder.RegisterInstance(clusterWrapper);
            builder.RegisterInstance(Sys);

            _container = builder.Build();
            new AutoFacDependencyResolver(_container, Sys);
        }
    }
}
