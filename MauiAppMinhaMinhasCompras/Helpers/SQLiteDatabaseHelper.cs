using MauiAppMinhaMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhaMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        private readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto produto)
        {
            return _conn.InsertAsync(produto);
        }

        public Task<List<Produto>> Update(Produto produto)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            
            return _conn.QueryAsync<Produto>(sql, produto.Descricao, produto.Quantidade, produto.Preco, produto.Id);
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().Where(x => x.Id == id).DeleteAsync();
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM PRODUTO WHERE descricao LIKE '%" + q + "%'";
            
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
