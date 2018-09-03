﻿using System;
using System.Linq;
using TOTVS.Models;

namespace TOTVS.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TotvsContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Clientes.Any())
            {
                return;   // DB has been seeded
            }

            var clientes = new Cliente[]
            {
            new Cliente{Nome="Carson",CPF="97917191010"},
            new Cliente{Nome="Git",CPF="23937196013"},
            new Cliente{Nome="Good",CPF="87442646077"},
            new Cliente{Nome="Adriana",CPF="94842766018"},
            new Cliente{Nome="Yan",CPF="47188406006"}
            };
            foreach (Cliente s in clientes)
            {
                context.Clientes.Add(s);
            }
            context.SaveChanges();

            var produtos = new Produto[]
            {
            new Produto{Descricao="Iphone",ValorIndividual=900.00f},
            new Produto{Descricao="Borracha",ValorIndividual=0.25f},
            new Produto{Descricao="Papel",ValorIndividual=0.05f},
            new Produto{Descricao="Coelho",ValorIndividual=69.95f},
            new Produto{Descricao="Aparador",ValorIndividual=250.00f},
            new Produto{Descricao="Varal",ValorIndividual=120.50f},
            new Produto{Descricao="Uno",ValorIndividual=450.99f}
            };
            foreach (Produto c in produtos)
            {
                context.Produtos.Add(c);
            }
            context.SaveChanges();

            var pedidos = new Pedido[]
            {
            new Pedido{ClienteID=1,DataPedido=new DateTime(2017,1,18)},
            new Pedido{ClienteID=2,DataPedido=new DateTime(2015,3,30)},
            new Pedido{ClienteID=2,DataPedido=new DateTime(2018,8,22)},
            new Pedido{ClienteID=5,DataPedido=new DateTime(2018,8,25)},
            new Pedido{DataPedido=new DateTime(2015,4,23)},
            new Pedido{DataPedido=new DateTime(2017,6,7)}
            };
            foreach (Pedido e in pedidos)
            {
                context.Pedidos.Add(e);
            }
            context.SaveChanges();

            var produtopeditos = new ProdutoPedido[]
            {
            new ProdutoPedido{ProdutoID=1,PedidoID=3,Quantidade=5},
            new ProdutoPedido{ProdutoID=2,PedidoID=2,Quantidade=65},
            new ProdutoPedido{ProdutoID=3,PedidoID=1,Quantidade=2},
            new ProdutoPedido{ProdutoID=1,PedidoID=2,Quantidade=62},
            new ProdutoPedido{ProdutoID=4,PedidoID=3,Quantidade=8},
            new ProdutoPedido{ProdutoID=4,PedidoID=5,Quantidade=235}
            };
            foreach (ProdutoPedido e in produtopeditos)
            {
                context.ProdutoPedidos.Add(e);
            }
            context.SaveChanges();
        }
    }
}