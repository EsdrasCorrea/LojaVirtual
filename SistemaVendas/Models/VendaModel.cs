using Newtonsoft.Json;
using SistemaVendas.Uteis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVendas.Models
{
    public class VendaModel
    {
        public string Id { get; set; }
        public string Cliente_Id { get; set; }
        public string Vendedor_Id { get; set; }
        public double Total { get; set; }
        public string ListaProdutos { get; set; }
        public object DAL { get; private set; }

        public List<ClienteModel> RetornarListaClientes()
        {
            return new ClienteModel().ListarTodosClientes();
        }

        public List<VendedorModel> RetornarListaVendedores()
        {
            return new VendedorModel().ListarTodosVendedores();
        }

        public List<ProdutoModel> RetornarListaProdutos()
        {
            return new ProdutoModel().ListarTodosProdutos();
        }

        public void Inserir()
        
        {
            DAL objDAL = new DAL();

            string dataVenda = DateTime.Now.Date.ToString("yyyy/MM/dd");

            string sql = "INSERT INTO VENDA(data, total, Vendedor_id, Cliente_id)" +
                $"VALUES('{dataVenda}',{Total.ToString().Replace(",",".")},{Vendedor_Id},{Cliente_Id})";
            objDAL.ExecutarComandoSql(sql);

            //Recuperar o Id da Venda
            sql = $"select id from venda where data='{dataVenda}' and vendedor_id={Vendedor_Id} and cliente_id={Cliente_Id} order by id desc limit 1";
            DataTable dt = objDAL.RetDataTable(sql);
            string id_venda = dt.Rows[0]["id"].ToString();

            //Serealizar o JSON da lista de produtos selecionados e gravá-los na tabela itens_venda
            List<ItemVendaModel> lista_produto = JsonConvert.DeserializeObject<List<ItemVendaModel>>(ListaProdutos);
            for (int i = 0; i < lista_produto.Count; i++)
            {
                sql = "insert into itens_venda(Venda_id, Produto_id, qtde_produto, preco_produto)" +
                    $"values({id_venda}, {lista_produto[i].CodigoProduto.ToString()}," +
                    $"{lista_produto[i].QtdeProduto.ToString()}," +
                    $"{lista_produto[i].PrecoUnitario.ToString().Replace(",",".")})";
                objDAL.ExecutarComandoSql(sql);
            }
        }
    }
}
