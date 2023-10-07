using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsaTec.Core.Enums;
using MsaTec.DAL.Data;
using MsaTec.Model;

namespace MsaTec.DAL.Tests.Integrations
{
    public class DbIntegrationTests : IClassFixture<DbContextFixture>
    {
        private readonly DbContextMsaTec _dbContext;

        public DbIntegrationTests(DbContextFixture fixture)
        {
            _dbContext = fixture.ServiceProvider.GetRequiredService<DbContextMsaTec>();
            _dbContext.SetTestMode();
        }

        [Fact]
        public async Task InsertAndDeleteClientesWithTelefones()
        {
             if (_dbContext.Clientes.Count() > 0)
            {
                _dbContext.Telefones.RemoveRange(_dbContext.Telefones);
                _dbContext.Clientes.RemoveRange(_dbContext.Clientes);
                await _dbContext.Commit();
            }

            // Arrange
            var telefones = new List<Telefone>
            {
                new Telefone { Numero = "123456789", Tipo = TipoTelefoneEnum.Comercial },
                new Telefone { Numero = "987654321", Tipo = TipoTelefoneEnum.Pessoal }
            };

            var clientes = new List<Cliente>();
            var rnd = new Random();
            for (var i = 1; i <= 20; i++)
            {
                var cliente = new Cliente
                {
                    Nome = $"Cliente{i}",
                    Email = $"cliente{rnd.Next(1, 4000)}_{i}@example.com",
                    DataNascimento = DateTime.UtcNow.AddYears(-i),
                    Telefones = telefones
                };

                if (!_dbContext.Clientes.Any(c => c.Email == cliente.Email))
                    clientes.Add(cliente);
            }

            foreach (var clnt in GenerateClientes())
            {
                var cliente = new Cliente
                {
                    Nome = clnt.Value.Name,
                    Email = clnt.Value.Email,
                    DataNascimento = clnt.Value.DataNascimento,
                    Telefones = new List<Telefone> { new Telefone() { Numero = ExtractTelefoneNumber(clnt.Value.Telefone), Tipo = GetRandomTipoTelefone() } },
                    
                };

                if (!_dbContext.Clientes.Any(c => c.Email == cliente.Email)) 
                  clientes.Add(cliente);
            }

            await _dbContext.Clientes.AddRangeAsync(clientes);
            await _dbContext.Commit();

            // Assert
            var insertedClientes = _dbContext.Clientes.ToList();
            Assert.Equal(clientes.Count(), insertedClientes.Count);
            Assert.All(insertedClientes, c => Assert.NotEqual(Guid.Empty, c.Id));
            Assert.All(insertedClientes, c => Assert.True(c.Telefones.Count > 0));

            var telefonesInserted = (from c in insertedClientes
                                from t in c.Telefones
                                select t).ToList();
            // Cleanup
            _dbContext.Telefones.RemoveRange(telefonesInserted);
            _dbContext.Clientes.RemoveRange(insertedClientes);
            await _dbContext.SaveChangesAsync();

            // Verify cleanup
            var remainingClientes = _dbContext.Clientes.ToList();
            Assert.Empty(remainingClientes);
        }

        private string ExtractTelefoneNumber(string telefone)
        {
            return telefone.Replace("+", "").Replace(" ", "").Replace("-", "");
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
    }
}
