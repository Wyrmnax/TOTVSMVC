using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TOTVS.Models
{
    public class Pedido
    {
        public int ID { get; set; }
        public int? ClienteID { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }

        public ICollection<ProdutoPedido> ProdutoPedidos { get; set; }
    }
}
