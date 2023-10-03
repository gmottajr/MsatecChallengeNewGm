using FluentAssertions;
using MsaTec.Abstractions.Model.Base;
using MsaTec.Abstractions.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Abstractions.Tests.Models;

public class EntityTests
{
    [Fact]
    public void EntityRootBase_Should_Inherit_From_EntityBase()
    {
        typeof(EntityRootBase).Should().BeDerivedFrom<EntityBase>();
    }

    [Fact]
    public void EntityBase_Should_Implement_IEntityValidator()
    {
        typeof(EntityBase).Should().Implement<IEntityValidator>();
    }

    [Fact]
    public void EntityRootBase_Should_Implement_IEntityRoot()
    {
        typeof(EntityRootBase).Should().Implement<IEntityRoot>();
    }

    
}
