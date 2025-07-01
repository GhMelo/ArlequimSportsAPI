# ArlequimSportsAPI

-O sistema se propõe a ser um gerenciador interno de vendas de artigos de esportes, com controle de estoque, funcionários e pedidos
-Sistema feito respeitando a divisão de camadas de DDD
-Utiliza JWT token para autenticação
-Middleware para o CorrelationId que faz o tracing das rotas e quais foram seus resultados
-Middleware de tratamento global de exceções e inserção de logs no MongoDB
-Utiliza Confluent Kafka para fazer a conexão ao Kafka
-Utiliza Zookeeper como broker do Kafka
-Sistema Code First - Utilizando entity framework para fazer o instanciamento o banco e suas entidades

## Instalação

Para rodar o projeto, clone o projeto
na pasta SistemaArlequimSports/ArlequimSportsAPI/ArlequimSportsAPI/ entre em appsettings.json  modifique a string de conexão do mongodb para uma propria, pois essa deixei em nuvem em um cluster pessoal
navegue ate a pasta que tem o docker-compose (SistemaArlequimSports/ArlequimSportsAPI/ArlequimSportsAPI/)
Digite o comando no powershell docker compose up --build e espere tudo buildar no container

Alem de fazer os builds, o sistema também faz o insert de StatusPedido e TipoOperação que são enumerables no Domain da API. Arquivo seed.sql

## Requisições e Testes 

Swagger - http://localhost:5000/swagger/index.html

Post Usuário - O tipo 1 define que é administrador, 0 é vendedor
{
  "nome": "Administrador",
  "email": "administrador@arlequim.com",
  "senha": "String123.",
  "tipo": 1
}

Post Auth/login - O token que vem é o bearer que vai ser colocado no Swagger 
{
  "email": "administrador@arlequim.com",
  "senha": "String123."
}

Bearer SeuToken

Post EsporteModalidade - Para no futuro fazer filtragem por modalidade, promoções, etc
{
  "descricao": "Boxe"
}

Post Produto - Produto base que vai ser utilizado no pedido e no ProdutoEstoque
{
  "nome": "Luva Everlast",
  "descricao": "Luva Everlast 13 oncas",
  "preco": 155.50,
  "esporteModalidadeId": 1
}

Post Produto 
{
  "nome": "Luva Everlast",
  "descricao": "Luva Everlast 15 oncas",
  "preco": 140.50,
  "esporteModalidadeId": 1
}

Post ProdutoEstoque - Aonde é controlado o estoque, a nota fiscal seria a nota da compra dos produtos para estoque, qual a quantidade e qual produto que essa nota se refere
{
  "notaFiscal": "string",
  "produtos": [
    {
      "produtoId": 1,
      "quantidade": 55
    },
    {
      "produtoId": 2,
      "quantidade": 30
    }
  ]
}

Post Pedido - O email do cliente é o que vai receber a confirmação da compra, o documento é uma string aberta para diferentes tipos de documentos e formatos
a API verifica se tem estoque, faz a movimentação e remove do estoque, salvando o nome de quem fez, qual tipo de operação e de qual estoque
{
  "documentoCliente": "CPF: 03997322135",
  "emailCliente": "emailquevaireceberoemaildeconfirmacao@gmail.com",
  "produtos": [
    {
      "produtoId": 1,
      "quantidade": 20
    },
    {
      "produtoId": 2,
      "quantidade": 15
    }

  ]
}

Ao fazer o post do pedido a API vai mandar uma mensagem para o Kafka na fila

Clique no link que foi enviado ao email - MUITO IMPORTANTE que o link do email seja acessado pelo mesmo computador que esta rodando a aplicação docker

Vai chamar o localhost para confirmar o email, em circunstancias normais seria uma rota mais protegida e não apontada para o próprio localhost

Ao confirmar o email, a API vai enviar uma mensagem para o Kafka mas para o serviço de pagamento

Atualmente esse serviço não verifica se o pagamento foi aceito, foi feito para demonstrar o conceito de microserviços, ele vai fazer uma chamada para a API atualizando o status do pedido

Get Pedido e la vai estar como a situação que ficou o pedido

## Observações aos analistas:

-Peço que olhem as regras de negocio que implementei baseado em movimento de estoque, qual produto de qual estoque esta sendo feito o pedido, nem todas as entidades tem rotas pois não devem ser modificadas
-Compreendo que o sistema tem muitas falhas, como a não implementação dos testes(Utilizaria NUnit), não validação de entidades ao ser inserido ao banco, o docker-compose esta na pasta errada, porem tive pouco tempo devido a mudança de cidade
