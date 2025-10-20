using SQLite;

namespace MauiAppMinhaMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao;
        
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public String Descricao { 
            get =>_descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("por favor, preencha a descrição");
                }
                _descricao = value;
            } 
        }
        
        public Double Quantidade { get; set; }
        
        public Double Preco { get; set; }
        
        public Double Total { get => Quantidade * Preco; }
    }
}
