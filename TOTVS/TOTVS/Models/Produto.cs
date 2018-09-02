using System.Collections.Generic;

namespace TOTVS.Models
{
    public class Produto
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public float ValorIndividual { get; set; }

        public ICollection<ProdutoPedido> ProdutoPedidos { get; set; }
    }
}