using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FoodDelivery.Utils;

public class PostgresUtils
{
    public static bool HasErrorCode(DbUpdateException exception, string errorCode)
    {
        if (exception.InnerException is not PostgresException postgresException)
        {
            return false;
        }
        return postgresException.SqlState == errorCode;
    }
}