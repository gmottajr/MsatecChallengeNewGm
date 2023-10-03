using FluentAssertions;
using MsaTec.Abstractions.Model.Base;
using MsaTec.Abstractions.Model.Contracts;
using MsaTec.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Architecture.Tests.Model;

public class EntityShouldInheritsOrImplements
{
    [Fact]
    public void Correctly()
    {
        var modelAssembly = typeof(Cliente).Assembly; // Assuming EntityRootBase is defined in the Model assembly

        var types = modelAssembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface && !type.IsSealed)
            .ToList();

        foreach (var type in types)
        {
            type.Should().Match(typeToCheck =>
                typeToCheck.IsSubclassOf(typeof(EntityRootBase)) || // Inherit from EntityRootBase
                typeToCheck.IsSubclassOf(typeof(EntityBase)) ||    // Inherit from EntityBase
                typeToCheck.GetInterfaces().Any(interfaceType =>   // Implement IEntityValidator and (IEntityRoot or IEntity)
                    interfaceType == typeof(IEntityValidator) &&
                    (interfaceType == typeof(IEntityRoot) || interfaceType == typeof(IEntity))
                ),
                $"Type {type.FullName} should inherit from EntityRootBase or EntityBase, or implement IEntityValidator and (IEntityRoot or IEntity)."
            );
        }
    }
}
