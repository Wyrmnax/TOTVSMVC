using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOTVS.Data;
using TOTVS.Models;
using TOTVS.Models.ViewModels;

namespace TOTVS.Controllers
{
    public class PedidosController : Controller
    {
        private readonly TotvsContext _context;

        public PedidosController(TotvsContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index(int? id, int? produtoID)
        {
            var viewModel = new PedidoIndexData();

            viewModel.Pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.ProdutoPedidos)
                .ThenInclude(p => p.Produto)
                .ToListAsync();

            return View(viewModel);
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClienteID,ValorTotal,DataPedido")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidosEdit = new PedidoEditData() { };

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.ProdutoPedidos)
                .ThenInclude(p => p.Produto)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            var produtos = await _context.Produtos
                .Include(p => p.ProdutoPedidos)
                .AsNoTracking()
                .ToListAsync();

            var produtosPedidos = await _context.ProdutoPedidos
                .Where(s => s.PedidoID == id)
                .Include(s => s.Produto)
                .AsNoTracking()
                .ToListAsync();

            pedidosEdit.Pedido = pedido;
            pedidosEdit.Produtos = produtos;
            pedidosEdit.ProdutoPedidos = produtosPedidos;

            if (pedidosEdit.Pedido == null)
            {
                return NotFound();
            }
            PopulateAssignedPedidoData(pedidosEdit.Pedido);

            pedidosEdit.ListaClientes = new SelectList(_context.Clientes.ToList(), "ID", "Nome");

            return View(pedidosEdit);
        }

        private void PopulateAssignedPedidoData(Pedido pedido)
        {
            var allProdutos = _context.Produtos;
            var pedidoProduto = new HashSet<int>(pedido.ProdutoPedidos.Select(c => c.ProdutoID));
            var viewModel = new List<AssignedPedidoData>();
            foreach (var produto in allProdutos)
            {
                viewModel.Add(new AssignedPedidoData
                {
                    ProdutoID = produto.ID,
                    Descricao = produto.Descricao,
                    Assigned = pedidoProduto.Contains(produto.ID)
                });
            }
            ViewData["Produtos"] = viewModel;
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedProdutos, int? SelectedUserID)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoToUpdate = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.ProdutoPedidos)
                .ThenInclude(p => p.Produto)
                .SingleOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<Pedido>(
                pedidoToUpdate,
                "",
                i => i.ID, i => i.DataPedido, i => i.ClienteID))
            {
                if (string.IsNullOrWhiteSpace(pedidoToUpdate.Cliente?.Nome))
                {
                    pedidoToUpdate.Cliente = null;
                }
                UpdatePedidoProdutos(selectedProdutos, pedidoToUpdate);

                pedidoToUpdate.ClienteID = SelectedUserID;
                //var resultData = _context.Clientes.Select(c => c.ID == ListaClientes.SelectedValue);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }

            UpdatePedidoProdutos(selectedProdutos, pedidoToUpdate);
            PopulateAssignedPedidoData(pedidoToUpdate);
            return View(pedidoToUpdate);
        }

        private void UpdatePedidoProdutos(string[] selectedProdutos, Pedido pedidoToUpdate)
        {
            if (selectedProdutos == null)
            {
                pedidoToUpdate.ProdutoPedidos = new List<ProdutoPedido>();
                return;
            }

            var selectedProdutosHS = new HashSet<string>(selectedProdutos);
            var pedidoProdutos = new HashSet<int>
                (pedidoToUpdate.ProdutoPedidos.Select(c => c.Produto.ID));
            foreach (var produto in _context.Produtos)
            {
                if (selectedProdutosHS.Contains(produto.ID.ToString()))
                {
                    if (!pedidoProdutos.Contains(produto.ID))
                    {
                        pedidoToUpdate.ProdutoPedidos.Add(new ProdutoPedido { PedidoID = pedidoToUpdate.ID, ProdutoID = produto.ID });
                    }
                }
                else
                {
                    if (pedidoProdutos.Contains(produto.ID))
                    {
                        ProdutoPedido produtoToRemove = pedidoToUpdate.ProdutoPedidos.SingleOrDefault(i => i.ProdutoID == produto.ID);
                        _context.Remove(produtoToRemove);
                    }
                }
            }
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.ID == id);
        }
    }
}