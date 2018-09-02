using System.Collections.Generic;

namespace TOTVS.Models.ViewModels
{
    public class PedidoIndexData
    {
        public IEnumerable<Pedido> Pedidos { get; set; }
        public IEnumerable<ProdutoPedido> ProdutoPedidos { get; set; }
        public IEnumerable<Produto> Produtos { get; set; }
    }
}