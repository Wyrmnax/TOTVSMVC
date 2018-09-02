using System;
using System.Collections.Generic;

namespace TOTVS.Models
{
    public class Pedido
    {
        public int ID { get; set; }
        public int? ClienteID { get; set; }
        public Cliente Cliente { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }

        public ICollection<ProdutoPedido> ProdutoPedidos { get; set; }
    }
}