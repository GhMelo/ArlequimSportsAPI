﻿namespace Domain.Entity
{
    public class PedidoProduto : EntityBase
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoEstoqueId { get; set; }
        public virtual Pedido Pedido { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual ProdutoEstoque ProdutoEstoque { get; set; }
    }
}
