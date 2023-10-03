using MsaTec.Core.Extensions;
using Npgsql;

namespace MsaTec.DAL.Tests;

public class DbConnectionTester
{
    public static bool TestPostgresConnection(string? connectionString)
    {
        if (connectionString.IsEmpty())
            return false;

        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
