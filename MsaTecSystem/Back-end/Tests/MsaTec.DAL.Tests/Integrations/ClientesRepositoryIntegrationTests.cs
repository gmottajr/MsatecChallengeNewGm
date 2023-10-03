using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Core.Enums;
using MsaTec.DAL.Data;
using MsaTec.DAL.Repositories.Contracts;
using MsaTec.Model;
using Xunit.Abstractions;

namespace MsaTec.DAL.Tests.Integrations;

public  class ClientesRepositoryIntegrationTests : IClassFixture<DbContextFixture>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly DbContextMsaTec _dbContext;
    public ClientesRepositoryIntegrationTests(DbContextFixture fixture)
    {
        _clienteRepository = fixture.ServiceProvider.GetRequiredService<IClienteRepository>();
        _dbContext = fixture.ServiceProvider.GetRequiredService<DbContextMsaTec>();
        _dbContext.SetTestMode();
    }

    [Fact]
    public void AppSettingsJson_ShouldExistInTestDirectory()
    {
        // Arrange
        var testDirectory = Directory.GetCurrentDirectory();
        var appSettingsPath = Path.Combine(testDirectory, "appsettings.json");

        // Act & Assert
        Assert.True(File.Exists(appSettingsPath), $"The 'appsettings.json' file does not exist in the test directory: '{testDirectory}'");
    }

    [Fact]
    public async Task InsertAndGetByIdAsync_ShouldInsertAndReturnCliente()
    {
        // Arrange and Act
        List<Cliente> clientes = await InsertData();

        foreach (var cliente in clientes)
        {
            var insertedCliente = await _clienteRepository.GetByIdAsync(cliente.Id);

            // Assert
            Assert.NotNull(insertedCliente);
            Assert.Equal(cliente.Id, insertedCliente?.Id);
        }

        await ClearDatabaseAsync();

    }


    [Fact]
    public async Task Update_ShouldUpdateCliente()
    {
        // Arrange and Act
        List<Cliente> clientes = await InsertData();

        // Act
        Random _random = new Random();
        foreach (var cliente in clientes)
        {
            var gotNum = _random.Next(1, 100);
            cliente.Nome = $"Updated Name {gotNum}";
            await _clienteRepository.Update(cliente);
            var updatedCliente = await _clienteRepository.GetByIdAsync(cliente.Id);

            // Assert
            Assert.NotNull(updatedCliente);
            Assert.Equal($"Updated Name {gotNum}", updatedCliente?.Nome);
        }

        await ClearDatabaseAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCliente()
    {
        // Arrange and Act
        List<Cliente> clientes = await InsertData();

        // Act
        Random _random = new Random();
        foreach (var cliente in clientes)
        {
            // Act
            await _clienteRepository.DeleteAsync(cliente.Id);
            var deletedCliente = await _clienteRepository.GetByIdAsync(cliente.Id);

            // Assert
            Assert.Null(deletedCliente);
        }

        await ClearDatabaseAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllClientes()
    {
        // Arrange
        // Arrange and Act
        List<Cliente> clientesOring = await InsertData();
        
        // Act
        var clientes = await _clienteRepository.GetAllAsync();

        // Assert
        Assert.NotNull(clientes);
        Assert.Equal(clientesOring.Count, clientes.Count());

        foreach(var cliente in clientesOring)
        {
            var gotYou = clientes.FirstOrDefault(c => c.Id == cliente.Id);
            Assert.NotNull(gotYou);
            gotYou.Nome.Should().Be(cliente.Nome);
            gotYou.Email.Should().Be(cliente.Email);
        }

        await ClearDatabaseAsync();
    }

    private async Task ClearDatabaseAsync()
    {
        var insertedClientes = _dbContext.Clientes.ToList();
        var telefonesInserted = (from c in insertedClientes
                                 from t in c.Telefones
                                 select t).ToList();
        // Cleanup
        _dbContext.Telefones.RemoveRange(telefonesInserted);
        _dbContext.Clientes.RemoveRange(insertedClientes);
        await _dbContext.SaveChangesAsync();
    }

    private static Dictionary<string, ClienteInfo> GenerateClientes()
    {
        var clientes = new Dictionary<string, ClienteInfo>
            {
                { "Brazil1", new ClienteInfo { Name = "Fernanda Silva", Email = "fernanda@example.com", DataNascimento = new DateTime(1985, 5, 10), Telefone = "+55 11 98765-4321" } },
                { "Brazil2", new ClienteInfo { Name = "Rafael Costa", Email = "rafael@example.com", DataNascimento = new DateTime(1990, 3, 15), Telefone = "+55 11 91234-5678" } },
                // Add more Brazilian names, emails, birth dates, and phone numbers here...

                { "USA1", new ClienteInfo { Name = "John Smith", Email = "john@example.com", DataNascimento = new DateTime(1982, 7, 20), Telefone = "+1 555-1234" } },
                { "USA2", new ClienteInfo { Name = "Emily Johnson", Email = "emily@example.com", DataNascimento = new DateTime(1988, 11, 5), Telefone = "+1 555-5678" } },
                // Add more American names, emails, birth dates, and phone numbers here...

                { "India1", new ClienteInfo { Name = "Amit Patel", Email = "amit@example.com", DataNascimento = new DateTime(1989, 2, 12), Telefone = "+91 98765 43210" } },
                { "India2", new ClienteInfo { Name = "Priya Singh", Email = "priya@example.com", DataNascimento = new DateTime(1985, 9, 8), Telefone = "+91 98765 67890" } },
                // Add more Indian names, emails, birth dates, and phone numbers here...

                { "China1", new ClienteInfo { Name = "Li Wei", Email = "li@example.com", DataNascimento = new DateTime(1987, 12, 25), Telefone = "+86 138 1234 5678" } },
                { "China2", new ClienteInfo { Name = "Zhang Min", Email = "zhang@example.com", DataNascimento = new DateTime(1983, 4, 30), Telefone = "+86 139 5678 1234" } },
                // Add more Chinese names, emails, birth dates, and phone numbers here...

                { "Italy1", new ClienteInfo { Name = "Marco Rossi", Email = "marco@example.com", DataNascimento = new DateTime(1984, 6, 18), Telefone = "+39 345 678 9012" } },
                { "Italy2", new ClienteInfo { Name = "Alessia Russo", Email = "alessia@example.com", DataNascimento = new DateTime(1986, 10, 8), Telefone = "+39 333 987 6543" } },
                // Add more Italian names, emails, birth dates, and phone numbers here...

                { "Israel1", new ClienteInfo { Name = "Eli Cohen", Email = "eli@example.com", DataNascimento = new DateTime(1981, 9, 2), Telefone = "+972 50-123-4567" } },
                { "Israel2", new ClienteInfo { Name = "Maya Levi", Email = "maya@example.com", DataNascimento = new DateTime(1986, 4, 15), Telefone = "+972 52-987-6543" } },
                // Add more Israeli names, emails, birth dates, and phone numbers here...

                { "France1", new ClienteInfo { Name = "Jean Dupont", Email = "jean@example.com", DataNascimento = new DateTime(1978, 12, 7), Telefone = "+33 6 12 34 56 78" } },
                { "France2", new ClienteInfo { Name = "Sophie Martin", Email = "sophie@example.com", DataNascimento = new DateTime(1985, 3, 22), Telefone = "+33 6 98 76 54 32" } }
                // Add more French names, emails, birth dates, and phone numbers here...
             };

        return clientes;
    }

    public TipoTelefoneEnum GetRandomTipoTelefone()
    {
        Random _random = new Random();

        var values = Enum.GetValues(typeof(TipoTelefoneEnum));
        var randomIndex = _random.Next(values.Length);
        var tipoTelefoneEnum = (TipoTelefoneEnum)values.GetValue(randomIndex);
        return tipoTelefoneEnum;
    }

    private string ExtractTelefoneNumber(string telefone)
    {
        return telefone.Replace("+", "").Replace(" ", "").Replace("-", "");
    }

    private async Task<List<Cliente>> InsertData()
    {
        var clientes = new List<Cliente>();

        foreach (var clnt in GenerateClientes())
        {
            var cliente = new Cliente
            {
                Nome = clnt.Value.Name,
                Email = clnt.Value.Email,
                DataNascimento = clnt.Value.DataNascimento,
                Telefones = new List<Telefone> { new Telefone() { Numero = ExtractTelefoneNumber(clnt.Value.Telefone), Tipo = GetRandomTipoTelefone() } },

            };

            clientes.Add(cliente);

            // Act
            await _clienteRepository.InsertAsync(cliente);
        }

        return clientes;
    }
}
