using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateSaver
{
    public class LocalDbService
    {
        private const string DB_NAME = "dateSaver_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Date>();
        }


        // CRUD Operations

        // Create
        public async Task Create(Date date)
        {
            await _connection.InsertAsync(date);
        }

        // Read
        public async Task<List<Date>> GetDates()
        {
            return await _connection.Table<Date>().ToListAsync();
        }
        public async Task<Date> GetById(int id)
        {
            return await _connection.Table<Date>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Update
        public async Task Update(Date date)
        {
            await _connection.UpdateAsync(date);
        }

        // Delete
        public async Task Delete(Date date)
        {
            await _connection.DeleteAsync(date);
        }
    }
}
