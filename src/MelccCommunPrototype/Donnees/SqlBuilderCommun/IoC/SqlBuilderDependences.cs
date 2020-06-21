using Autofac;
using AutoMapper;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Demo.Commun
{
    [ExcludeFromCodeCoverage]
    public static class SqlBuilderDependences
    {
        public static IContainer Build<TContext>(this ContainerBuilder builder, string connectionStrings, string prefix = null) where TContext : ContextBase
        {
            builder.RegisterType<RegisterConnection>()
                .WithParameter("connectionStrings", connectionStrings)
                .WithParameter("prefix", prefix)
                .InstancePerLifetimeScope();

            builder.RegisterType<ConnectionFactory>().As<IConnectionFactory>().InstancePerRequest();
            builder.RegisterType<ContextBase>().InstancePerLifetimeScope();
            builder.RegisterType<SqlBuilder>().As<ISqlBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<SqlBuilderClauses>().As<ISqlBuilderClauses>().InstancePerLifetimeScope();
            builder.RegisterType<BuilderParams>().As<IBuilderParams>().InstancePerLifetimeScope();
            builder.RegisterType<BuilderCount>().InstancePerLifetimeScope();
            //builder.RegisterType<TablesVerificateur>().InstancePerLifetimeScope();
            builder.RegisterType<ContextSchema>().InstancePerLifetimeScope();
            builder.RegisterType<AssemblieHelper>().As<IAssemblieHelper>().InstancePerLifetimeScope();

            builder.RegisterType<TContext>().InstancePerLifetimeScope();
            builder.RegisterType<TablesVerificateur>()
                .WithParameter("references", typeof(TContext)).InstancePerLifetimeScope();

            var container = builder.Build();
            return container;
        }

        public static void RegisterMaps(this ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                    .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }


}
