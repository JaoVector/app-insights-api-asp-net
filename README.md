# Application Insights com ASP Net Core
## Descrição
Foi feita uma Web Api com Asp Net Core 6.0, para implementar um CRUD simples utilizando o Azure Database e também o Blob Storage. O intuito do projeto é apresentar
o Application Insights, que é um recurso do Azure para monitoramento de aplicações locais e na Nuvem.

### Rotas Utilizadas para Produtos
+ POST https://localhost:7127/api/Produto
+ GET https://localhost:7127/api/Produto
+ GET https://localhost:7127/api/Produto/{id}
+ PUT https://localhost:7127/api/Produto/{id}
+ DELETE https://localhost:7127/api/Produto/{id}
#
### Rotas Utilizadas para Imagens
+ POST https://localhost:7127/api/Imagem
+ GET https://localhost:7127/api/Imagem
+ GET https://localhost:7127/api/Imagem/{id}
+ DELETE https://localhost:7127/api/Imagem/DeletaImagemPelaUrl
#
### Modelo de Dados Produtos
+ __ProdutoId__ utilizado para identificar o produto em cada ação.
+ __Nome__ utilizado para dar um titulo ao produto.
+ __Descricao__ utilizado para descrever o conteúdo do produto.
+ __Preco__ utilizado para atribuir um valor ao produto.
+ __Imagens__ utilizado para atribuir imagens aos produtos.
#
### Modelo de Dados Imagens
+ __ImageId__ identifica a imagem em cada operação.
+ __ProdutoId__ faz referência ao produto no qual a imagem pertence.
+ __ImagemName__ titulo da imagem.
+ __ImagemURL__ Url utilizada pela imagem para ser exposta e consultada no Blob.

### Como acessar a aplicação:
Para acessar o doc swagger para utilizar a aplicação basta consultar no navegador a URL: https://localhost:7127/swagger/index.html
