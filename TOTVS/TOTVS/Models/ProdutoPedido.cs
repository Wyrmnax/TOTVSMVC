namespace TOTVS.Models
{
    public class ProdutoPedido
    {
        public int ProdutoID { get; set; }
        public int PedidoID { get; set; }
        public Produto Produto { get; set; }
        public Pedido Pedido { get; set; }

        public int? Quantidade { get; set; }
    }
}