using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MsaTec.Core.Enums;
using MsaTec.DAL.Data;
using MsaTec.IoC.Contracts;
using MsaTec.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.IoC
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DbContextMsaTec _DbContext;

        private Dictionary<string, ClienteInfo> clientesDictionary = new Dictionary<string, ClienteInfo>
        {
            { "Argentina1", new ClienteInfo { Name = "Santiago Gomez", Email = "santiago@example.com", DataNascimento = new DateTime(1980, 8, 15), Telefones = new List<string> { "+54 9 11 1234-5678", "+54 9 11 8345-675", "+54 9 11 2645-248" } } },
            { "Argentina2", new ClienteInfo { Name = "Valentina Morales", Email = "valentina@example.com", DataNascimento = new DateTime(1983, 6, 20), Telefones = new List<string> { "+54 9 11 2345-6789", "+54 9 11 8765-4321" } } },
            { "Africa1", new ClienteInfo { Name = "Lamidi Sow", Email = "lamidi@example.com", DataNascimento = new DateTime(1987, 4, 3), Telefones = new List<string> { "+221 77 123 4567", "+221 77 987 6543", "+221 76 543 2109" } } },
            { "Africa2", new ClienteInfo { Name = "Fatima Nkosi", Email = "fatima@example.com", DataNascimento = new DateTime(1984, 9, 12), Telefones = new List<string> { "+27 71 234 5678", "+27 71 876 5432", "+27 72 345 6789" } } },
            { "Turkey1", new ClienteInfo { Name = "Emre Yilmaz", Email = "emre@example.com", DataNascimento = new DateTime(1982, 2, 8), Telefones = new List<string> { "+90 532 123 4567", "+90 532 765 4321", "+90 533 246 8021" } } },
            { "Turkey2", new ClienteInfo { Name = "Aylin Demir", Email = "aylin@example.com", DataNascimento = new DateTime(1986, 11, 25), Telefones = new List<string> { "+90 533 234 5678", "+90 533 876 5432", "+90 534 567 8901" } } },
            { "Malaysia1", new ClienteInfo { Name = "Ravi Patel", Email = "ravi@example.com", DataNascimento = new DateTime(1981, 5, 19), Telefones = new List<string> { "+60 12-345 6789", "+60 12-987 6543", "+60 13-246 8021" } } },
            { "Malaysia2", new ClienteInfo { Name = "Nadia Wong", Email = "nadia@example.com", DataNascimento = new DateTime(1989, 7, 7), Telefones = new List<string> { "+60 17-987 6543", "+60 17-123 4567", "+60 18-345 6789" } } },
            { "Thailand1", new ClienteInfo { Name = "Sarun Chai", Email = "sarun@example.com", DataNascimento = new DateTime(1983, 10, 30), Telefones = new List<string> { "+66 85 123 4567", "+66 85 765 4321", "+66 86 246 8021" } } },
            { "Thailand2", new ClienteInfo { Name = "Arunee Siriwong", Email = "arunee@example.com", DataNascimento = new DateTime(1987, 12, 12), Telefones = new List<string> { "+66 81 234 5678", "+66 81 876 5432", "+66 82 345 6789" } } },
            { "Canada1", new ClienteInfo { Name = "Michael Johnson", Email = "michael@example.com", DataNascimento = new DateTime(1975, 3, 8), Telefones = new List<string> { "+1 416-123-4567", "+1 416-234-5678", "+1 416-345-6789" } } },
            { "Canada2", new ClienteInfo { Name = "Emily White", Email = "emily@example.com", DataNascimento = new DateTime(1980, 9, 15), Telefones = new List<string> { "+1 416-111-2222", "+1 416-333-4444" } } },
            { "Indonesia1", new ClienteInfo { Name = "Budi Santoso", Email = "budi@example.com", DataNascimento = new DateTime(1988, 1, 25), Telefones = new List<string> { "+62 812-3456-7890", "+62 812-9876-5432" } } },
            { "Indonesia2", new ClienteInfo { Name = "Dewi Susanto", Email = "dewi@example.com", DataNascimento = new DateTime(1985, 7, 12), Telefones = new List<string> { "+62 813-1234-5678", "+62 813-8765-4321" } } },
            { "Philippines1", new ClienteInfo { Name = "Juan dela Cruz", Email = "juan@example.com", DataNascimento = new DateTime(1982, 5, 30), Telefones = new List<string> { "+63 917-123-4567", "+63 917-1234-5678", "+63 615-723-4887" } } },
            { "Japan1", new ClienteInfo { Name = "Yuki Tanaka", Email = "yuki@example.com", DataNascimento = new DateTime(1985, 3, 12), Telefones = new List<string> { "+81 90-1234-5678", "+81 90-2345-6789", "+81 80-3456-7890" } } },
            { "Japan2", new ClienteInfo { Name = "Haruto Suzuki", Email = "haruto@example.com", DataNascimento = new DateTime(1988, 7, 8), Telefones = new List<string> { "+81 80-9876-5432", "+81 80-8765-4321", "+81 90-7654-3210" } } },
            { "Japan3", new ClienteInfo { Name = "Airi Nakamura", Email = "airi@example.com", DataNascimento = new DateTime(1982, 11, 20), Telefones = new List<string> { "+81 90-1111-2222", "+81 90-3333-4444", "+81 80-5555-6666" } } },
            { "France1", new ClienteInfo { Name = "Lucas Dupont", Email = "lucas@example.com", DataNascimento = new DateTime(1981, 6, 25), Telefones = new List<string> { "+33 6 12 34 56 78", "+33 6 98 76 54 32", "+33 6 11 22 33 44" } } },
            { "France2", new ClienteInfo { Name = "Elise Martin", Email = "elise@example.com", DataNascimento = new DateTime(1984, 9, 18), Telefones = new List<string> { "+33 6 55 44 33 22", "+33 6 11 22 33 44", "+33 6 99 88 77 66" } } },
            { "France3", new ClienteInfo { Name = "Mathis Bernard", Email = "mathis@example.com", DataNascimento = new DateTime(1979, 12, 5), Telefones = new List<string> { "+33 6 77 88 99 00", "+33 6 33 22 11 00", "+33 6 66 77 88 99" } } },
            { "Belgium1", new ClienteInfo { Name = "Lena De Boer", Email = "lena@example.com", DataNascimento = new DateTime(1983, 2, 14), Telefones = new List<string> { "+32 470 12 34 56", "+32 470 98 76 54", "+32 470 11 22 33" } } },
            { "Belgium2", new ClienteInfo { Name = "Noah Janssens", Email = "noah@example.com", DataNascimento = new DateTime(1987, 5, 7), Telefones = new List<string> { "+32 470 55 44 33", "+32 470 11 22 33", "+32 470 99 88 77" } } },
            { "Belgium3", new ClienteInfo { Name = "Emma Maes", Email = "emma@example.com", DataNascimento = new DateTime(1980, 10, 12), Telefones = new List<string> { "+32 470 77 88 99", "+32 470 33 22 11", "+32 470 66 77 88" } } },
            { "Germany1", new ClienteInfo { Name = "Max Fischer", Email = "max@example.com", DataNascimento = new DateTime(1982, 4, 30), Telefones = new List<string> { "+49 1512 3456789", "+49 1512 9876543", "+49 1512 1122334" } } },
            { "Germany2", new ClienteInfo { Name = "Sophie Becker", Email = "sophie@example.com", DataNascimento = new DateTime(1985, 8, 22), Telefones = new List<string> { "+49 1512 5544332", "+49 1512 1122334", "+49 1512 9988776" } } },
            { "Germany3", new ClienteInfo { Name = "Finn Schneider", Email = "finn@example.com", DataNascimento = new DateTime(1978, 11, 15), Telefones = new List<string> { "+49 1512 7766554", "+49 1512 3322111", "+49 1512 6677889" } } },
            { "Latvia1", new ClienteInfo { Name = "Anastasija Ozola", Email = "anastasija@example.com", DataNascimento = new DateTime(1983, 7, 17), Telefones = new List<string> { "+371 21 123 456", "+371 21 987 654", "+371 21 234 567" } } },
            { "Latvia12", new ClienteInfo { Name = "Eva Liepa", Email = "eva@example.com", DataNascimento = new DateTime(1984, 2, 10), Telefones = new List<string> { "+371 21 123 456", "+371 21 987 654", "+371 21 234 567" } } },
            { "Latvia2", new ClienteInfo { Name = "Matiss Berzins", Email = "matiss@example.com", DataNascimento = new DateTime(1986, 6, 15), Telefones = new List<string> { "+371 22 345 678", "+371 22 876 543", "+371 22 432 109" } } },
            { "Latvia3", new ClienteInfo { Name = "Zane Vilks", Email = "zane@example.com", DataNascimento = new DateTime(1982, 9, 22), Telefones = new List<string> { "+371 20 111 222", "+371 20 333 444", "+371 20 555 666" } } },
            { "Philippines4", new ClienteInfo { Name = "Jose Cruz", Email = "jose@example.com", DataNascimento = new DateTime(1981, 3, 5), Telefones = new List<string> { "+63 917 123 4567", "+63 917 987 6543", "+63 917 234 5678" } } },
            { "Philippines2", new ClienteInfo { Name = "Maria Santos", Email = "maria@example.com", DataNascimento = new DateTime(1983, 8, 18), Telefones = new List<string> { "+63 917 111 2222", "+63 917 333 4444", "+63 917 555 6666" } } },
            { "Philippines3", new ClienteInfo { Name = "Carlos Reyes", Email = "carlos@example.com", DataNascimento = new DateTime(1980, 12, 12), Telefones = new List<string> { "+63 917 777 8888", "+63 917 999 0000", "+63 917 666 5555" } } },
            { "Israel3", new ClienteInfo { Name = "Aviva Cohen", Email = "aviva@example.com", DataNascimento = new DateTime(1980, 11, 28), Telefones = new List<string> { "+972 50-987-6543", "+972 52-123-4567", "+972 52-234-5678" } } },
            { "Israel4", new ClienteInfo { Name = "Eitan Levi", Email = "eitan@example.com", DataNascimento = new DateTime(1985, 4, 16), Telefones = new List<string> { "+972 50-111-2222", "+972 50-333-4444", "+972 52-555-6666" } } },
            { "Israel5", new ClienteInfo { Name = "Tamar Avraham", Email = "tamar@example.com", DataNascimento = new DateTime(1982, 7, 9), Telefones = new List<string> { "+972 50-777-8888", "+972 50-999-0000", "+972 52-666-5555" } } },
            { "Morocco1", new ClienteInfo { Name = "Ahmed Rahal", Email = "ahmed@example.com", DataNascimento = new DateTime(1981, 6, 14), Telefones = new List<string> { "+212 6 11 22 33 44", "+212 6 55 66 77 88", "+212 6 99 88 77 66" } } },
            { "Morocco2", new ClienteInfo { Name = "Fatima Benali", Email = "fatima2@gmailexample.com", DataNascimento = new DateTime(1984, 9, 27), Telefones = new List<string> { "+212 6 77 88 99 00", "+212 6 33 22 11 00", "+212 6 66 77 88 99" } } },
            { "Morocco3", new ClienteInfo { Name = "Youssef Abidi", Email = "youssef@example.com", DataNascimento = new DateTime(1980, 12, 3), Telefones = new List<string> { "+212 6 21 22 23 24", "+212 6 25 26 27 28", "+212 6 29 30 31 32" } } },
            { "Brazil3", new ClienteInfo { Name = "Camila Oliveira", Email = "camila@example.com", DataNascimento = new DateTime(1987, 2, 8), Telefones = new List<string> { "+55 11 7777-8888", "+55 11 9999-0000", "+55 11 6666-5555" } } },
            { "Brazil4", new ClienteInfo { Name = "Felipe Santos", Email = "felipe@example.com", DataNascimento = new DateTime(1983, 5, 16), Telefones = new List<string> { "+55 11 1111-2222", "+55 11 3333-4444", "+55 11 5555-6666" } } },
            { "Brazil5", new ClienteInfo { Name = "Amanda Silva", Email = "amanda@example.com", DataNascimento = new DateTime(1980, 8, 22), Telefones = new List<string> { "+55 11 9999-8888", "+55 11 7777-6666", "+55 11 5555-4444" } } },
            { "Switzerland1", new ClienteInfo { Name = "Sophie Müller", Email = "sophie1@example3.com", DataNascimento = new DateTime(1983, 9, 5), Telefones = new List<string> { "+41 76 123 45 67", "+41 76 234 56 78", "+41 76 345 67 89" } } },
            { "Switzerland2", new ClienteInfo { Name = "Lukas Fischer", Email = "lukas@example.com", DataNascimento = new DateTime(1987, 2, 18), Telefones = new List<string> { "+41 78 987 65 43", "+41 78 876 54 32", "+41 78 765 43 21" } } },
            { "Switzerland3", new ClienteInfo { Name = "Nina Keller", Email = "nina@example.com", DataNascimento = new DateTime(1980, 11, 12), Telefones = new List<string> { "+41 79 234 56 78", "+41 79 345 67 89", "+41 79 456 78 90" } } },
            { "Norway1", new ClienteInfo { Name = "Erik Larsen", Email = "erik@example.com", DataNascimento = new DateTime(1982, 4, 9), Telefones = new List<string> { "+47 900 12 345", "+47 900 23 456", "+47 900 34 567" } } },
            { "Norway2", new ClienteInfo { Name = "Mette Olsen", Email = "mette@example.com", DataNascimento = new DateTime(1986, 7, 22), Telefones = new List<string> { "+47 901 11 222", "+47 901 33 444", "+47 901 55 666" } } },
            { "Norway3", new ClienteInfo { Name = "Andreas Haugen", Email = "andreas@example.com", DataNascimento = new DateTime(1980, 12, 16), Telefones = new List<string> { "+47 902 77 888", "+47 902 99 000", "+47 902 66 555" } } },
            { "Finland1", new ClienteInfo { Name = "Ella Virtanen", Email = "ella@example.com", DataNascimento = new DateTime(1981, 5, 14), Telefones = new List<string> { "+358 45 123 45 67", "+358 45 234 56 78", "+358 45 345 67 89" } } },
            { "Finland2", new ClienteInfo { Name = "Mikko Kinnunen", Email = "mikko@example.com", DataNascimento = new DateTime(1985, 8, 27), Telefones = new List<string> { "+358 45 987 65 43", "+358 45 876 54 32", "+358 45 765 43 21" } } },
            { "Finland3", new ClienteInfo { Name = "Sari Mäkelä", Email = "sari@example.com", DataNascimento = new DateTime(1970, 10, 6), Telefones = new List<string> { "+358 45 234 56 78", "+358 45 345 67 89", "+358 45 456 78 90" } } },
            { "USA3", new ClienteInfo { Name = "Michael Johnson", Email = "michael4@yahooexample.com", DataNascimento = new DateTime(1982, 3, 19), Telefones = new List<string> { "+1 555-7777", "+1 555-9999", "+1 555-6666" } } },
            { "USA4", new ClienteInfo { Name = "Emily Davis", Email = "emilyscort@example.com", DataNascimento = new DateTime(1986, 6, 25), Telefones = new List<string> { "+1 555-1111", "+1 555-3333", "+1 555-4444" } } },
            { "USA5", new ClienteInfo { Name = "David Smith", Email = "david@example.com", DataNascimento = new DateTime(1980, 9, 14), Telefones = new List<string> { "+1 555-8888", "+1 555-2222", "+1 555-3333" } } },
            { "Netherlands1", new ClienteInfo { Name = "Lotte van der Berg", Email = "lotte@example.com", DataNascimento = new DateTime(1981, 11, 12), Telefones = new List<string> { "+31 6 12 34 56 78", "+31 6 98 76 54 32", "+31 6 11 22 33 44" } } },
            { "Netherlands2", new ClienteInfo { Name = "Thomas de Jong", Email = "thomas@example.com", DataNascimento = new DateTime(1985, 4, 8), Telefones = new List<string> { "+31 6 55 44 33 22", "+31 6 11 22 33 44", "+31 6 99 88 77 66" } } },
            { "Netherlands3", new ClienteInfo { Name = "Eva Bakker", Email = "evaBakker@protoemailexample.com", DataNascimento = new DateTime(1980, 7, 23), Telefones = new List<string> { "+31 6 77 88 99 00", "+31 6 33 22 11 00", "+31 6 66 77 88 99" } } },
            { "Switzerland6", new ClienteInfo { Name = "Sophie Müller", Email = "sophie3@example.com", DataNascimento = new DateTime(1983, 9, 5), Telefones = new List<string> { "+41 76 123 45 67", "+41 76 234 56 78", "+41 76 345 67 89" } } },
            { "Switzerland4", new ClienteInfo { Name = "Lukas Fischer", Email = "lukas@example.com", DataNascimento = new DateTime(1987, 2, 18), Telefones = new List<string> { "+41 78 987 65 43", "+41 78 876 54 32", "+41 78 765 43 21" } } },
            { "Switzerland5", new ClienteInfo { Name = "Nina Keller", Email = "nina@example.com", DataNascimento = new DateTime(1980, 11, 12), Telefones = new List<string> { "+41 79 234 56 78", "+41 79 345 67 89", "+41 79 456 78 90" } } },
            { "Norway4", new ClienteInfo { Name = "Erik Larsen", Email = "erik@example.com", DataNascimento = new DateTime(1982, 4, 9), Telefones = new List<string> { "+47 900 12 345", "+47 900 23 456", "+47 900 34 567" } } },
            { "Norway5", new ClienteInfo { Name = "Mette Olsen", Email = "mette@example.com", DataNascimento = new DateTime(1986, 7, 22), Telefones = new List<string> { "+47 901 11 222", "+47 901 33 444", "+47 901 55 666" } } },
            { "Norway6", new ClienteInfo { Name = "Andreas Haugen", Email = "andreas@example.com", DataNascimento = new DateTime(1980, 12, 16), Telefones = new List<string> { "+47 902 77 888", "+47 902 99 000", "+47 902 66 555" } } },
            { "Finland4", new ClienteInfo { Name = "Ella Virtanen", Email = "ella@example.com", DataNascimento = new DateTime(1981, 5, 14), Telefones = new List<string> { "+358 45 123 45 67", "+358 45 234 56 78", "+358 45 345 67 89" } } },
            { "Finland5", new ClienteInfo { Name = "Mikko Kinnunen", Email = "mikko@example.com", DataNascimento = new DateTime(1985, 8, 27), Telefones = new List<string> { "+358 45 987 65 43", "+358 45 876 54 32", "+358 45 765 43 21" } } },
            { "Finland6", new ClienteInfo { Name = "Sari Mäkelä", Email = "sari@example.com", DataNascimento = new DateTime(1980, 10, 6), Telefones = new List<string> { "+358 45 234 56 78", "+358 45 345 67 89", "+358 45 456 78 90" } } },

        };

        public DbInitializer(DbContextMsaTec dbContext)
        {
            _DbContext = dbContext;
        }
        public void Initialize()
        {
            var uniqueEmails = new HashSet<string>();
            foreach (var addCliente in clientesDictionary) 
            { 
                if(!_DbContext.Clientes.Any(clt => clt.Nome == addCliente.Value.Name))
                {
                    var rnd = new Random();
                    
                    if (!uniqueEmails.Add(addCliente.Value.Email))
                    {
                        var processaEmaillist = addCliente.Value.Email.Split('@');
                        var gotEmail = $"{processaEmaillist[0]}{rnd.Next(1, 3000)}@{processaEmaillist[1]}";
                        addCliente.Value.Email = gotEmail;
                        uniqueEmails.Add(gotEmail);
                    }
                    
                    var addingPhoe = new List<Telefone>();
                    addCliente.Value.Telefones.ForEach(tel => {
                        var itemTel = new Telefone
                        {
                            Numero = ExtractTelefoneNumber(tel),
                            Tipo = GetRandomTipoTelefone()
                        };
                        addingPhoe.Add(itemTel);
                    });

                    var cliente = new Cliente
                    {
                        Nome = addCliente.Value.Name,
                        Email = addCliente.Value.Email,
                        DataNascimento = addCliente.Value.DataNascimento,
                        Telefones = addingPhoe
                    };
                    //cliente.Telefones.AddRange(addingPhoe);
                    _DbContext.Clientes.Add(cliente);                
                }
            }
            _DbContext.SaveChanges();
        }

        private static string ExtractTelefoneNumber(string telefone)
        {
            return telefone.Replace("+", "").Replace(" ", "").Replace("-", "");
        }

        private static TipoTelefoneEnum GetRandomTipoTelefone()
        {
            Random _random = new Random();

            var values = Enum.GetValues(typeof(TipoTelefoneEnum));
            var randomIndex = _random.Next(values.Length);
            var tipoTelefoneEnum = (TipoTelefoneEnum)values.GetValue(randomIndex);
            return tipoTelefoneEnum;
        }

        private class ClienteInfo
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime DataNascimento { get; set; }
            public List<string> Telefones { get; set; } = new List<string>();
        }
    }
}
