using AutoMapper;
using FluentAssertions;
using MsaTec.Application.ProfilerMapping;
using MsaTec.Application.ViewModels;
using MsaTec.Core.Enums;
using MsaTec.Core.Extensions;
using MsaTec.Model;

namespace MsaTec.Application.Tests;

public class AutoMapperTests
{
    private readonly IMapper _mapper;

    public AutoMapperTests()
    {
        // Configure AutoMapper with the MappingProfile
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile(new ClienteProfilerMapping());
            config.AddProfile(new TelefoneProfilerMapping());
        });

        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public void EntityToViewModel_ShouldMapCorrectly()
    {
        // Arrange and Act
        foreach (var clienteEntity in TestHelper.LoadClienteEntitiesData())
        {
            // Act
            var clienteViewModel = _mapper.Map<ClienteViewModel>(clienteEntity);

            // Assert
            clienteViewModel.Should().NotBeNull();
            clienteViewModel.Nome.Should().Be(clienteEntity.Nome);
            clienteViewModel.Email.Should().Be(clienteEntity.Email);
            clienteViewModel.DataNascimento.Should().Be(clienteEntity.DataNascimento);
            clienteViewModel.Telefones.Should().NotBeNull();
            clienteViewModel.Telefones.Should().HaveCount(1);
            clienteViewModel.Telefones.ForEach(t => {
                var gotTel = clienteEntity.Telefones.FirstOrDefault(tel => tel.Id == t.Id);
                gotTel.Should().NotBeNull();
                if (gotTel != null)
                  t.Numero.Should().Be(gotTel.Numero);
            });


        }

    }

    [Fact]
    public void ViewModelToEntity_ShouldMapCorrectly()
    {
        // Arrange and Act
        foreach (var clienteViewModel in TestHelper.LoadClienteViewModelsData())
        {
            // Act
            var clienteEntity = _mapper.Map<Cliente>(clienteViewModel);

            // Assert
            clienteEntity.Should().NotBeNull();
            clienteEntity.Nome.Should().Be(clienteViewModel.Nome);
            clienteEntity.Email.Should().Be(clienteViewModel.Email);
            clienteEntity.DataNascimento.Should().Be(clienteViewModel.DataNascimento);
            clienteEntity.Telefones.Should().NotBeNull();
            clienteEntity.Telefones.Should().HaveCount(1);
            clienteEntity.Telefones.ForEach(t => {
                var gotTel = clienteViewModel.Telefones.FirstOrDefault(tel => tel.Id == t.Id);
                gotTel.Should().NotBeNull();
                if (gotTel != null)
                    t.Numero.Should().Be(gotTel.Numero);
            });
        }
    }

    [Fact]
    public void EntityToViewModelList_ShouldMapCorrectly()
    {
        // Arrange and Act
        foreach(var cliente in TestHelper.LoadClienteEntitiesData())
        {
            var clienteViewModelList = _mapper.Map<ClienteViewModelForList>(cliente);

            // Assert
            clienteViewModelList.Should().NotBeNull();
            clienteViewModelList.Nome.Should().Be(cliente.Nome);
            clienteViewModelList.Email.Should().Be(cliente.Email);
            clienteViewModelList.TelefonePrincipal.Should().NotBeNull();
            clienteViewModelList.TelefonePrincipal.Numero.Should().Be(cliente.Telefones[0].Numero);
            clienteViewModelList.TelefonePrincipal.Tipo.Should().Be(cliente.Telefones[0].Tipo.GetDescription());
        }
    }

    [Fact]
    public void ViewModelToEntityList_ShouldMapCorrectly()
    {
        // Arrange and Act
        foreach (var clienteViewModelList in TestHelper.LoadClienteViewModelListsData())
        {
            // Act
            var clienteEntity = _mapper.Map<Cliente>(clienteViewModelList);

            // Assert
            clienteEntity.Should().NotBeNull();
            clienteEntity.Nome.Should().Be(clienteViewModelList.Nome);
            clienteEntity.Email.Should().Be(clienteViewModelList.Email);
            clienteEntity.Telefones.Should().NotBeNull();
            clienteEntity.Telefones.Should().HaveCount(0);

        }
    }

    [Fact]
    public void MapClienteToClienteViewModelForList_ShouldSelectPessoalPhoneNumberIfAvailable()
    {
        // Arrange
        var cliente = new Cliente
        {
            Nome = "Cliente test",
            Email = "clienteTest@email.com",
            DataNascimento = new DateTime(1993, 12, 23),
            Telefones = new List<Telefone>
            {
                new Telefone { Tipo = TipoTelefoneEnum.Pessoal, Numero = "12345" },
                new Telefone { Tipo = TipoTelefoneEnum.Comercial, Numero = "67890" }
            }
        };

        // Act
        var clienteViewModel = _mapper.Map<ClienteViewModelForList>(cliente);

        // Assert
        clienteViewModel.TelefonePrincipal.Should().NotBeNull();
        clienteViewModel.TelefonePrincipal.Numero.Should().Be("12345"); // Pessoal phone number should be selected.
    }

    [Fact]
    public void MapClienteToClienteViewModelForList_ShouldSelectAnyPhoneNumberIfPessoalNotAvailable()
    {
        // Arrange
        var cliente = new Cliente
        {
            Nome = "Cliente test",
            Email = "clienteTest@email.com",
            DataNascimento = new DateTime(1993, 12, 23),
            Telefones = new List<Telefone>
            {
                new Telefone { Tipo = TipoTelefoneEnum.Comercial, Numero = "67890" },
                new Telefone { Tipo = TipoTelefoneEnum.Outros, Numero = "54321" }
            }
        };

        // Act
        var clienteViewModel = _mapper.Map<ClienteViewModelForList>(cliente);

        // Assert
        clienteViewModel.TelefonePrincipal.Should().NotBeNull();
        clienteViewModel.TelefonePrincipal.Numero.Should().Be("67890"); // Any phone number should be selected as Pessoal is not available.
    }

}
