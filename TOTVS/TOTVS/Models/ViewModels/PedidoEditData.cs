using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TOTVS.Models.ViewModels
{
    public class PedidoEditData
    {
        [Display(Name = "Cliente")]
        public int SelectedUserID { get; set; }

        public IEnumerable<SelectListItem> ListaClientes { get; set; }
        public Pedido Pedido { get; set; }

        public IEnumerable<ProdutoPedido> ProdutoPedidos { get; set; }
        public IEnumerable<Produto> Produtos { get; set; }
    }
}