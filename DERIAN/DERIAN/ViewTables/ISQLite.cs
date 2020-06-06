 
namespace DERIAN.ViewTables
{
    using SQLite;

    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
