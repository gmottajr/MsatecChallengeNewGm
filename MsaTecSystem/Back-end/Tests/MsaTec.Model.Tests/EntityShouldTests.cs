using FluentAssertions;
using MsaTec.Abstractions.Model.Base;
using MsaTec.Abstractions.Model.Contracts;
using MsaTec.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Model.Tests
{
    public class EntityShouldTests
    {
        [Fact]
        public void Telefone_Should_Inherit_From_EntityRootBase()
        {
            typeof(Telefone).Should().BeDerivedFrom<EntityRootBase>();
        }

        [Fact]
        public void Telefone_Should_Implement_IEntityValidator()
        {
            typeof(Telefone).Should().Implement<IEntityValidator>();
        }

        [Fact]
        public void Cliente_Should_Inherit_From_EntityRootBase()
        {
            typeof(Cliente).Should().BeDerivedFrom<EntityRootBase>();
        }

        [Fact]
        public void Cliente_Should_Implement_IEntityValidator()
        {
            typeof(Cliente).Should().Implement<IEntityValidator>();
        }

        [Fact]
        public void Cliente_Should_Contain_Properties()
        {
            var clienteProperties = typeof(Cliente).GetProperties().Select(p => p.Name);

            clienteProperties.Should().Contain("Nome");
            clienteProperties.Should().Contain("Email");
            clienteProperties.Should().Contain("DataNascimento");
            clienteProperties.Should().Contain("Telefones");
        }
    }
}
