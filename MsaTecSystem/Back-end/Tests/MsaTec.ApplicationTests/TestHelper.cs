using MsaTec.Application.ViewModels;
using MsaTec.Core.Enums;
using MsaTec.Model;
using MsaTec.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.Application.Tests;

public static class TestHelper
{
    public static Dictionary<string, ClienteInfo> GenerateClientes()
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

    public static List<Cliente> LoadClienteEntitiesData()
    {
        var clientes = new List<Cliente>();

        foreach (var clnt in GenerateClientes())
        {
            var clienteId = Guid.NewGuid();
            var cliente = new Cliente
            {
                Id = clienteId,
                Nome = clnt.Value.Name,
                Email = clnt.Value.Email,
                DataNascimento = clnt.Value.DataNascimento,
                Telefones = new List<Telefone> { new Telefone() { Id = Guid.NewGuid(), ClienteId = clienteId, Numero = ExtractTelefoneNumber(clnt.Value.Telefone), Tipo = GetRandomTipoTelefone() } },

            };

            clientes.Add(cliente);

        }

        return clientes;
    }

    public static List<ClienteViewModel> LoadClienteViewModelsData()
    {
        var clientes = new List<ClienteViewModel>();

        foreach (var clnt in GenerateClientes())
        {
            var clienteId = Guid.NewGuid();
            var cliente = new ClienteViewModel
            {
                Id = clienteId,
                Nome = clnt.Value.Name,
                Email = clnt.Value.Email,
                DataNascimento = clnt.Value.DataNascimento,
                Telefones = new List<TelefoneViewModel> { new TelefoneViewModel() {Id = Guid.NewGuid(),
                     Numero = ExtractTelefoneNumber(clnt.Value.Telefone),
                    Tipo = (int)GetRandomTipoTelefone(),
                    ClienteId = clienteId } },

            };

            clientes.Add(cliente);

        }

        return clientes;
    }

    public static List<ClienteViewModelList> LoadClienteViewModelListsData()
    {
        var clientes = new List<ClienteViewModelList>();

        foreach (var clnt in GenerateClientes())
        {
            var clienteId = Guid.NewGuid();
            var cliente = new ClienteViewModelList
            {
                Id = clienteId,
                Nome = clnt.Value.Name,
                Email = clnt.Value.Email,
                TelefonePrincipal = new TelefoneViewModelList()
                {
                    Id = Guid.NewGuid(),
                    Numero = ExtractTelefoneNumber(clnt.Value.Telefone),
                    Tipo = GetRandomTipoTelefone().GetDescription(),
                    ClienteId = clienteId
                },

            };
            cliente.TelefonePrincipal.ClienteId = cliente.Id;
            clientes.Add(cliente);

        }

        return clientes;
    }

    public static string ExtractTelefoneNumber(string telefone)
    {
        return telefone.Replace("+", "").Replace(" ", "").Replace("-", "");
    }

    public static TipoTelefoneEnum GetRandomTipoTelefone()
    {
        Random _random = new Random();

        var values = Enum.GetValues(typeof(TipoTelefoneEnum));
        var randomIndex = _random.Next(values.Length);
        var tipoTelefoneEnum = (TipoTelefoneEnum)values.GetValue(randomIndex);
        return tipoTelefoneEnum;
    }

    public class ClienteInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
    }

}
