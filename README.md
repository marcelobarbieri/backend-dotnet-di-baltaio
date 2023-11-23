<h1>Dominando Injeção de Dependência</h1>

Ref.: Balta.io

> O conteúdo também foi organizado nos **commits**

<!--#region Sumário -->

<h2>Sumário</h2>

<!--#region Fundamentos -->

<details><summary>Fundamentos</summary>

<ul>
    <li><a href="#fund-apresentacao">Apresentação</a></li>
    <li><a href="#fund-oquee">O que é injeção de dependência</a></li>
    <li><a href="#fund-baixoacoplamento">Baixo acoplamento</a></li>
    <li><a href="#fund-mauexemplo">Mau exemplo</a></li>
    <li><a href="#fund-entendendo">Entendendo o problema</a></li>
    <li><a href="#fund-resolvendo">Resolvendo o problema com OOP</a></li>
    <li><a href="#fund-descobre">Cobre o pé, descobre a cabeça</a></li>
    <li><a href="#fund-inversao">Inversão de Controle</a></li>
    <li><a href="#fund-abstracao">Abstração e Implementação</a></li>
    <li><a href="#fund-por-que">Por que abstrair?</a></li>
    <li><a href="#fund-dip">Princípio da Inversão de Dependência</a></li>
    <li><a href="#fund-service-locator">Service Locator</a></li>    
    <li><a href="#fund-add">AddTransient, AddScoped, AddSingleton</a></li>    
    <li><a href="#fund-adddbcontext">AddDbContext</a></li>
    <li><a href="#fund-resumo">Resumo</a></li>    
</ul>

</details>

<!--#endregion -->

<!--#region DI, IoC e DIP na prática -->

<details><summary>DI, IoC e DIP na prática</summary>

<ul>
    <li><a href="#pratica-apresentacao">Apresentação</a></li>
    <li><a href="#pratica-criando">Criando dependências</a></li>    
    <li><a href="#pratica-dip">DIP na prática</a></li>    
    <li><a href="#pratica-servicos">Utilizando serviços</a></li>    
    <li><a href="#pratica-promocode">PromoCode Respository</a></li>    
    <li><a href="#pratica-regras">Removendo as regras de negócio do controlador</a></li>    
</ul>

</details>

<!--#endregion -->

<!--#region Resolvendo as dependências -->

<details><summary>Resolvendo as dependências</summary>

<ul>
    <li><a href="#depend-addtransient">Resolvendo as dependências - AddTransient</a></li>
    <li><a href="#depend-addscoped">Resolvendo as dependências - AddScoped</a></li>
    <li><a href="#depend-addsingleton">Resolvendo as dependências - AddSingleton</a></li>
    <li><a href="#depend-extension-methods">Extension Methods</a></li>
    <li><a href="#depend-outras-formas">Outras formas de DI</a></li>
    <li><a href="#depend-impl-extension-methods">Implementando Extension Methods</a></li>
    <li><a href="#depend-add-parte1">AddTransient, AddScoped e AddSingleton na prática - Parte 1</a></li>
    <li><a href="#depend-add-parte2">AddTransient, AddScoped e AddSingleton na prática - Parte 2</a></li>
    <li><a href="#depend-mais-impl">Registrando mais de uma implementação</a></li>
    <li><a href="#depend-service-descriptor">Service Descriptor</a></li>
    <li><a href="#depend-tryadd-tryaddenumerable">TryAdd e TryAddEnumerable</a></li>
    <li><a href="#depend-multiplas">Resolvendo múltiplas dependências</a></li>
    <li><a href="#depend-tryaddtransient">TryAddTransient</a></li>
    <li><a href="#depend-tryaddenumerable">TryAddEnumerable</a></li>
    <li><a href="#depend-formas">Formas de resolver dependências</a></li>
    <li><a href="#depend-program">Resolvendo dependências no Program.cs</a></li>
    <li><a href="#depend-httpcontext">Resolvendo dependências no HttpContext.cs</a></li>    
    <li><a href="#depend-fromservices">Quando utilizar FromServices</a></li>
    <li><a href="#depend-getrequiredservices">GetRequiredServices</a></li>            
    <li><a href="#depend-getservice">GetService</a></li>            
</ul>

</details>

<!--#endregion -->

<!--#region Perguntas e Exercícios -->

<details><summary>Perguntas e Exercícios</summary>

<ul>
    <li><a href="#perguntas-entrevista">Hora de entrevista</a></li>
    <li><a href="#exercicios">Sugestão de projeto</a></li>
    <li><a href="#conclusao">Conclusão</a></li>
</ul>

</details>

<!--#endregion -->

<!--#endregion -->

<!--#region Fundamentos -->

<h2 id="fund">Fundamentos</h2>

<!--#region Apresentação  -->

<details id="fund-apresentacao"><summary>Apresentacao</summary>

<br/>

Agenda:

- O que é DI (injeção de dependência)?
- O que é IoC (inversão de controle)?
- O que é DIP (princípio da inversão da dependência)?
- Como os itens acima se relacionam
- DI no ASP.NET

Sobre este curso:

- Devs ASP.NET/ .NET
- Buscam aprimorar a teoria
- Querem conhecer mais DI

```c#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MagGet("/", () => "Dependency Injection!");

app.Run();
```

</details>

<!--#endregion -->

<!--#region O que é injeção de dependência  -->

<details id="fund-oquee"><summary>O que é injeção de dependência</summary>

<br/>

> Um termo bem confuso

Dependency Injection

- **Não é um padrão** (Design Pattern)
- Técnica que **implementa o IoC**
  - **IoC - Inversion of Control** (Inversão de Controle)
  - **DIP - Dependency Inversion Principle** (Princípio da Inversão de Dependência)
- Ajuda no baixo acoplamento
- Provê uma melhor divisão de responsabilidades
- O que eu preciso para trabalhar?
  - Quem vai me prover? Não importa.

</details>

<!--#endregion -->

<!--#region Baixo acoplamento -->

<details id="fund-baixoacoplamento"><summary>Baixo acoplamento</summary>

<br/>

- Imagina um sistema **grande**
- Cada pedacinho tem que **focar em uma coisa** (dividir para conquistar)
  - **Não dá** pra abraçar o mundo
- Tem que funcionar de forma **independente**
  - Fácil de **entender**
  - Fácil de dar **manutenção**
  - Se precisar **jogar fora e criar outro** é fácil

</details>

<!--#endregion -->

<!--#region Mau exemplo -->

<details id="fund-mauexemplo"><summary>Mau exemplo</summary>

<br/>

- Vamos tomar como base um pedido
- Recebe os parâmetros
- Processa o pedido

```c#
public class OrderController : Controller
{
  [Route("v1/orders")]
  [HttpPost]
  public async Task<string> Place(
    string customerId,
    string zipCode,
    string promoCode,
    int[] products
  )
  {
    // #1 - Recupera o cliente
    // #2 - Calcula o frete
    // #3 - Calcula o total dos produtos
    // #4 - Aplica o cupom de desconto
    // #5 - Gera o pedido
    // #6 - Calcula o total
    // #7 - Retorna
  }
}
```

```c#
// #1 - Recupera o cliente
Customer customer = null;
using (var conn = new SqlConnection("CONN_STRING"))
{
  customer = conn.Query<Customer>
    ("SELECT * FROM CUSTOMER WHERE ID=" + customerId)
    .FirstOrDefault();
}
```

```c#
// #2 - Calcula o frete
decimal deliveryFee = 0;
var request = new HttpRequestMessage(HttpMethod.Get,"URL/" + zipCode);
request.Headers.Add("Accept","application/json");
request.Headers.Add("User-Agent","HttpClientFactory-Sample");

using(HttpClient client = new HttpClient())
{
  var response = await client.SendAsync(request);
  if (response.IsSucessStatusCode)
  {
    deliveryFee = await response.Content.ReadAsAsync<decimal>();
  }
  else
  {
    // Caso não consiga obter a taxa de entrega o valor padrão é 5
    deliveryFee = 5;
  }
}
```

```c#
// #3 - Calcula o total dos produtos
decimal subTotal = 0;
for (int p = 0; p < products.Length; p++)
{
  var product = new Product();
  using (var conn = new SqlConnection("CONN_STRING"))
  {
    product = conn.Query<Product>
      ("SELECT * FROM PRODUCT WHERE ID=" + products[p])
      .FirstOrDefault();
  }
  subTotal += product.Price;
}
```

```c#
// #4 - Aplica o cupom de desconto
decimal discount = 0;
using (var conn = new SqlConnection("CONN_STRING"))
{
  var promo = conn.Query<PromoCode>
    ("SELECT * FROM PROMO_CODES WHERE CODE=" + promoCode)
    .FirstOrDefault();
  if (promo.ExpireDate > DateTime.Now)
  {
    discount = promo.Value;
  }
}
```

```c#
// #5 - Gera o pedido
var order = new Order();
order.Code = Guid.NewGuid().ToString().ToUpper().Substring(0,8);
order.Date = DateTime.Now;
order.DeliveryFee = deliveryFee;
order.Discount = discount;
order.Products = products;
order.SubTotal = subTotal;
```

```c#
// #6 - Calcula o total
order.Total = subtotal - discount + deliveryFee;
```

```c#
//#7 - Retorna
return $"Pedido {order.Code} gerado com sucesso!";
```

</details>

<!--#endregion -->

<!--#region Entendendo o problema -->

<details id="fund-entendendo"><summary>Entendendo o problema</summary>

<br/>

O problema:

- **Difícil** de ler
- **Difícil** de mudar
- Código **não é reusável**
- **Alto acoplamento**
- **Testes?** Pra quê?

```c#
public class OrderController : Controller
{
  [Route("v1/orders")]
  [HttpPost]
  public async Task<string> Place(
    string customerId,
    string zipCode,
    string promoCode,
    int[] products
  )
  {
    // #1 - Recupera o cliente
    Customer customer = null;
    using (var conn = new SqlConnection("CONN_STRING"))
    {
      customer = conn.Query<Customer>
        ("SELECT * FROM CUSTOMER WHERE ID=" + customerId)
        .FirstOrDefault();
    }

    // #2 - Calcula o frete
    decimal deliveryFee = 0;
    var request = new HttpRequestMessage(HttpMethod.Get,"URL/" + zipCode);
    request.Headers.Add("Accept","application/json");
    request.Headers.Add("User-Agent","HttpClientFactory-Sample");

    using(HttpClient client = new HttpClient())
    {
      var response = await client.SendAsync(request);
      if (response.IsSucessStatusCode)
      {
        deliveryFee = await response.Content.ReadAsAsync<decimal>();
      }
      else
      {
        // Caso não consiga obter a taxa de entrega o valor padrão é 5
        deliveryFee = 5;
      }
    }

    // #3 - Calcula o total dos produtos
    decimal subTotal = 0;
    for (int p = 0; p < products.Length; p++)
    {
      var product = new Product();
      using (var conn = new SqlConnection("CONN_STRING"))
      {
        product = conn.Query<Product>
          ("SELECT * FROM PRODUCT WHERE ID=" + products[p])
          .FirstOrDefault();
      }
      subTotal += product.Price;
    }

    // #4 - Aplica o cupom de desconto
    decimal discount = 0;
    using (var conn = new SqlConnection("CONN_STRING"))
    {
      var promo = conn.Query<PromoCode>
        ("SELECT * FROM PROMO_CODES WHERE CODE=" + promoCode)
        .FirstOrDefault();
      if (promo.ExpireDate > DateTime.Now)
      {
        discount = promo.Value;
      }
    }

    // #5 - Gera o pedido
    var order = new Order();
    order.Code = Guid.NewGuid().ToString().ToUpper().Substring(0,8);
    order.Date = DateTime.Now;
    order.DeliveryFee = deliveryFee;
    order.Discount = discount;
    order.Products = products;
    order.SubTotal = subTotal;

    // #6 - Calcula o total
    order.Total = subtotal - discount + deliveryFee;

    //#7 - Retorna
    return $"Pedido {order.Code} gerado com sucesso!";
  }
}

```

</details>

<!--#endregion -->

<!--#region Resolvendo o problema com OOP -->

<details id="fund-resolvendo"><summary>Resolvendo o problema com OOP</summary>

<br/>

Orientação a Objetos:

- **Abstração**, **encapsulamento**
  - **Simples** e direto
- Pedaços **pequenos**
- **Reusáveis**
- **Testáveis**
- **Legíveis**
- **Fácil** manutenção

Encapsular o código:

```c#
// #2 Calcular o frete

public class DeliveryService 
{
  public decimal GetDeliveryFee(string zipCode)
  {
    var request = new HttpRequestMessage(HttpMethod.Get, "URL/" + zipCode);
    request.Headers.Add("Accept","application/json");
    request.Headers.Add("User-Agent","HttpClientFactory-Sample");

    using (HttpClient client = new HttpClient())
    {
      var response = await client.SendAsync(request);
      if (response.IsSuccessStatusCode)
      {
        deliveryFee = await response.Content.ReadAsAsync<decimal>();
      }
      else
      {
        deliveryFee = 5;
      }
    }
  }
}
```

```c#
public class OrderController : Controller
{
  [Route("v1/orders")]
  [HttpPost]
  public async Task<string> Place(
    string customerId,
    string zipCode,
    string promoCode,
    int[] products
  )
  {
    ...
    var deliveryService = new DeliveryService();
    decimal deliveryFee = deliveryService.GetDeliveryFee(zipCode);
    ...
  }
}
```

</details>

<!--#endregion -->

<!--#region Cobre o pé, descobre a cabeça -->

<details id="fund-descobre"><summary>Cobre o pé, descobre a cabeça</summary>

<br/>

- Está **bem melhor**, mas...
- A **dependência** ainda existe
  - Só mudou de lugar
- Depende de **implementação**
  - Depender da **abstração**

</details>

<!--#endregion -->

<!--#region Inversão de Controle -->

<details id="fund-inversao"><summary>Inversão de Controle</summary>

<br/>

Inversion of Control

- **Inversão de Controle**
- **Externaliza** as responsabilidades
  - **Delega**
- **Cria uma dependência** externa
  - O controller não é mais **responsável** pelo cálculo do frete, agora ele **depende de um serviço**

```c#
public class OrderController : Controller
{
  private readonly DeliveryService _deliveryService;

  OrderController(DeliveryService deliveryService)
  {
    _deliveryService = deliveryService;
  }

  [Route("v1/orders")]
  [HttpPost]
  public async Task<string> Place (
    string customerId,
    string zipCode,
    string promoCode,
    int[] products
  )
  {
    ...
    decimal deliveryFee = _deliveryService.GetDeliveryFee(zipCode);
    ...
  }
}
```

```c#
[TestMethod]
public void ShouldPlaceAnOrder()
{
  var service = new DeliveryService();
  var controller = new OrderController(service);
  ...
}
```

</details>

<!--#endregion -->

<!--#region Abstração e Implementação -->

<details id="fund-abstracao"><summary>Abstração e Implementação</summary>

<br/>

Cobre o pé... descobre a cabeça

- Implementação
  - **Concreto**
  - **Materialização**
  - É o **"Como"**
- Abstração
  - **Contrato**
  - Só as **definições**
  - É o *"O que"**

</details>

<!--#endregion -->

<!--#region Por que abstrair? -->

<details id="fund-por-que"><summary>Por que abstrair?</summary>

<br/>

- **Facilita** as mudanças
  - Imagina um cenário crítico como a troca de um banco de dados
- **Testes de Unidade**
  - Não podem depender de banco, rede ou qualquer outra coisa externa
- Se você depende da abstração, **a implementação não importa**

</details>

<!--#endregion -->

<!--#region Princípio da Inversão de Dependência -->

<details id="fund-dip"><summary>Princípio da Inversão de Dependência</summary>

<br/>

**DIP - Dependency Inversion Principle**

- Princípio da **inversão de dependência**
- Depender de **abstrações** e não de **implementações**

```c#
public interface IDeliveryService
{
  decimal GetDeliveryFee(string zipCode);
}
```

```c#
public class DeliveryService : IDeliveryService
{
  public decimal GetDeliveryFee(string zipCode)
  {
    ...
  }
}
```

```c#
public class OrderController : Controller
{
  private readonly IDeliveryService _deliveryService;

  OrderController(IDeliveryService deliveryService)
  {
    _deliveryService = deliveryService;
  }
  ...
}
```

```c#
public FakeDeliveryService : IDeliveryService
{
  public decimal GetDeliveryFee(string zipCode)
  {
    return 10;
  }
}

[TestMethod]
public void ShouldPlaceAnOrder()
{
  IDeliveryService service = new FakeDeliveryService();
  var controller = new OrderController(service);  
  ...
}
```

</details>

<!--#endregion -->

<!--#region Service Locator -->

<details id="fund-service-locator"><summary>Service Locator</summary>

<br/>

Service Locator e DI no ASP.NET

- SL diz **como resolver** as dependências criadas
  - Funciona como um de-para
- Já temos um pronto no **ASP.NET**
  - Podemos utilizar outros

```c#
// Assim
builder.Services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
// ou
builder.Services.AddScoped<IDeliveryFeeService, DeliveryFeeService>();
// ou
builder.Services.AddSingleton<IDeliveryFeeService, DeliveryFeeService>();
```

</details>

<!--#endregion -->

<!--#region AddTransient, AddScoped, AddSingleton -->

<details id="fund-add"><summary>AddTransient, AddScoped, AddSingleton</summary>

<br/>

AddTransient

- Sempre cria uma **nova instância** do objeto
- Ideal para cenários onde queremos sempre um **novo objeto**

AddScoped

- Cria **um objeto** por transação (requisição)
- Se você chamar 2 ou mais serviços que dependem do **mesmo objeto**, a mesma instância será utilizada
- Ideal para cenários onde queremos **apenas um objeto** por requisição (banco)

Singleton

- Padrão que visa garantir **apenas uma instância** de um objeto para **aplicação toda**
- Um bom exemplo são as **configurações**
  - Uma vez carregadas, **ficam até a aplicação reiniciar**

AddSingleton

- Cria **um objeto** quando a aplicação inicia
- **Mantém este objeto** na memória até a aplicação parar ou reiniciar
- Sempre devolver a **mesma instância** deste objeto, com os mesmos valores
- **CUIDADO**

</details>

<!--#endregion -->

<!--#region AddDbContext -->

<details id="fund-adddbcontext"><summary>AddDbContext</summary>

<br/>

- Item **especial** do tipo **Scoped**
- Utilizado exclusivamente com **Entity Framework**
- Garante que a conexão só dura **até o fim da requisição**

```c#
builder
  .Services
  .AddDbContext<BlogDataContext>(x => x.UseSqlServer(connStr));
```

</details>

<!--#endregion -->

<!--#region Resumo -->

<details id="fund-resumo"><summary>Resumo</summary>

<br/>

- **DI** (técnica que aplica IoC)
- **IoC** (padrão de design, desacoplamento)
- **DIP** (príncipio, depender das abstrações)
- **Service Locator** (de-para)

</details>

<!--#endregion -->

<!--#endregion -->

<!--#region DI, IoC e DIP na prática -->

<h2 id="pratica">DI, IoC e DIP na prática</h2>

<!--#region Apresentação -->

<details id="pratica-apresentacao"><summary>Apresentação</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

</details>

<!--#endregion -->

<!--#region Criando dependências -->

<details id="pratica-criando"><summary>Criando dependências</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Refatoração do bloco #1 existente no **OrderController**:

```c#
        ...
        // #1 - Recupera o cliente
        Customer customer = null;
        await using (var conn = new SqlConnection("CONN_STRING"))
        {
            const string query = "SELECT [Id], [Name], [Email] FROM CUSTOMER WHERE ID=@id";
            customer = await conn.QueryFirstAsync<Customer>(query, new { id = customerId });
        }
        ...
```

Criação de um novo item **Repositories/CustomerRepository.cs**:

```c#
using Dapper;
using DependencyStore.Models;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories;

public class CustomerRepository
{
    private readonly SqlConnection _connection;

    public CustomerRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<Customer?> GetByIdAsync(string customerId)
    {
        const string query = "SELECT [Id], [Name], [Email] FROM CUSTOMER WHERE ID=@id";
        return await _connection
            .QueryFirstOrDefaultAsync<Customer>(query, new 
            { 
                id = customerId 
            });

    }
}
```

</details>

<!--#endregion -->

<!--#region DIP na prática -->

<details id="pratica-dip"><summary>DIP na prática</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Criação de um novo item **Repositories/Contracts/ICustomerRepository.cs**:

```c#
using DependencyStore.Models;

namespace DependencyStore.Repositories.Contracts;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(string customerId);
}
```

**CustomerRepository** passa a implementar a interface:

```c#
...
public class CustomerRepository : ICustomerRepository
...
```

Refatoração do **OrderController**:

```c#
public class OrderController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public OrderController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(string customerId, string zipCode, string promoCode, int[] products)
    {
        // #1 - Recupera o cliente
        Customer? customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return NotFound();
        
        ...
```

</details>

<!--#endregion -->

<!--#region Utilizando serviço -->

<details id="pratica-servicos"><summary>Utilizando serviço</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Refatoração do bloco #2 existente no **OrderController**:

```c#
...
        // #2 - Calcula o frete
        decimal deliveryFee = 0;
        var client = new RestClient("https://consultafrete.io/cep/");
        var request = new RestRequest()
            .AddJsonBody(new
            {
                zipCode
            });
        deliveryFee = await client.PostAsync<decimal>(request, new CancellationToken());
        // Nunca é menos que R$ 5,00
        if (deliveryFee < 5)
            deliveryFee = 5;
...
```

Criação do item **Services/Contracts/IDeliveryFeeService.cs**:

```c#
namespace DependencyStore.Services.Contracts;

public interface IDeliveryFeeService
{
    Task<decimal> GetDeliveryFeeAsync(string zipCode);
}
```


Criação do item **Services/DeliveryFeeService.cs**

```c#
using DependencyStore.Services.Contracts;
using RestSharp;

namespace DependencyStore.Services
{
    public class DeliveryFeeService : IDeliveryFeeService
    {
        public async Task<decimal> GetDeliveryFeeAsync(string zipCode)
        {
            var client = new RestClient("https://consultafrete.io/cep/");
            var request = new RestRequest()
                .AddJsonBody(new
                {
                    ZipCode = zipCode
                });
            var response = await client.PostAsync<decimal>(request);
            return response < 5 ? 5 : response;
        }
    }
}
```

Inserção da nova dependência no **OrderController**:

```c#
...

public class OrderController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;

    public OrderController(
        ICustomerRepository customerRepository,
        IDeliveryFeeService deliveryFeeService)
    {
        _customerRepository = customerRepository;
        _deliveryFeeService = deliveryFeeService;
    }

    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(string customerId, string zipCode, string promoCode, int[] products)
    {
        ...

        // #2 - Calcula o frete
        decimal deliveryFee = await _deliveryFeeService.GetDeliveryFeeAsync(zipCode);

        ...
```

</details>

<!--#endregion -->

<!--#region PromoCode Repository -->

<details id="pratica-promocode"><summary>PromoCode Repository</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Criação de um novo item **Repositories/Contracts/IPromoCodeRepository.cs**:

```c#
using DependencyStore.Models;

namespace DependencyStore.Repositories.Contracts;

public interface IPromoCodeRepository
{
    Task<PromoCode?> GetPromoCodeAsync(string promoCode);
}
```

Criação de um novo item **Repositories/PromoCodeRepository.cs**:

```c#
using Dapper;
using DependencyStore.Models;
using DependencyStore.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Repositories;

public class PromoCodeRepository : IPromoCodeRepository
{
    private readonly SqlConnection _connection;

    public PromoCodeRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<PromoCode?> GetPromoCodeAsync(string promoCode)
    {
        var query = $"SELECT * FROM PROMO_CODES WHERE CODE={promoCode}";
        return await _connection.QueryFirstOrDefaultAsync<PromoCode>(query);
    }
}
```

Refatoração do **OrderController**:

```c#
...

public class OrderController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;
    private readonly IPromoCodeRepository _promoCodeRespository;

    public OrderController(
        ICustomerRepository customerRepository,
        IDeliveryFeeService deliveryFeeService,
        IPromoCodeRepository promoCodeRespository)
    {
        _customerRepository = customerRepository;
        _deliveryFeeService = deliveryFeeService;
        _promoCodeRespository = promoCodeRespository;
    }

    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(string customerId, string zipCode, string promoCode, int[] products)
    {
        ...

        PromoCode? cupom = await _promoCodeRespository.GetPromoCodeAsync(promoCode);
        
        ...
```

</details>

<!--#endregion -->

<!--#region Removendo as regras de negócio do controlador -->

<details id="pratica-regras"><summary>Removendo as regras de negócio do controlador</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Ajustes no modelo **Order.cs** com a implementação do construtor, alteração do tipo de dado da lista de produtos **Products** e as fórmulas para as propriedades **SubTotal** e **Total** :

```c#
namespace DependencyStore.Models;

public class Order
{
    public Order(
        decimal deliveryFee,
        decimal discount,
        List<Product> products)
    {
        Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
        Date = DateTime.Now;
        DeliveryFee = deliveryFee;
        Discount = discount;
    }

    public string Code { get; set; }
    public DateTime Date { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Discount { get; set; }
    public List<Product> Products { get; set; }

    public decimal SubTotal => Products.Sum(x => x.Price);
    public decimal Total => SubTotal - Discount + DeliveryFee;
}
```

Refatoração do controlador **OrderController**:

```c#
...

    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(string customerId, string zipCode, string promoCode, int[] products)
    {
      
      ...

        decimal discount = cupom?.Value ?? 0M;
        Order order = new Order(deliveryFee, discount, new List<Product>());
        return Ok($"Pedido {order.Code} gerado com sucesso!");
    }
    
    ...
```

</details>

<!--#endregion -->

<!--#endregion -->

<!--#region Resolvendo as dependências -->

<h2 id="pratica">Resolvendo as dependências</h2>

<!--#region Resolvendo as dependências - AddTransient -->

<details id="depend-addtransient"><summary>Resolvendo as dependências - AddTransient</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Se o projeto for executado o controlador falhará porque existem dependências não resolvidas.

As dependências devem ser resolvidas antes de adicionar serviços para os controladores **builder.Services.AddControllers()** no **Program.cs**:

```c# 
...
using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services;
using DependencyStore.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
builder.Services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
...
```

</details>

<!--#endregion -->

<!--#region Resolvendo as dependências - AddScoped -->

<details id="depend-addtransient"><summary>Resolvendo as dependências - AddScoped</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Para as conexões com o banco teremos apenas um objeto por requisição.

Precisamos de uma única instância do banco de dados para a implementação das interfaces que fazem uso do **SqlConnection**.

Se for utilizado **AddTransient** cada interface instanciará um objeto de conexão ao banco de dados para o **SqlConnection**, não desejado. Não faz sentido, pois dados estão sendo manipulados dentro de uma mesma instância do objeto

Por isso resolve-se a dependência do **SqlConnection** no **Program.cs** com  **AddScoped**, antes de resolver a dependência das interfaces que a utilizam:

```c#
using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services;
using DependencyStore.Services.Contracts;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<SqlConnection>(); 
//ou
builder.Services.AddScoped(x => new SqlConnection("CONN_STRING"));
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
builder.Services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
```

Porém para banco de dados é recomendado utilizar o **AddDbContext** ao invés do **AddContext**

</details>

<!--#endregion -->

<!--#region Resolvendo as dependências - AddSingleton -->

<details id="depend-addsingleton"><summary>Resolvendo as dependências - AddSingleton</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Criação do item **Configuration.cs** com a propriedade **DeliveryFeeServiceUrl** para recuperar Url que será utilizada pela classe **DeliveryFeeService** a partir do **AppSettings.json**. 

```c#
namespace DependencyStore;

public class Configuration
{
    public string DeliveryFeeServiceUrl { get; set; } = "";
}
```

```json
{
  ...
  "DeliveryFeeServiceUrl": "https://consultafrete.io/cep/"
}
```

Injeção da dependência **Configuration** na classe **DeliveryFeeService** e sua utilização no método **GetDeliveryFeeAsync**.

```c#
namespace DependencyStore.Services
{
    public class DeliveryFeeService : IDeliveryFeeService
    {
        private readonly Configuration _configuration;

        public DeliveryFeeService(Configuration configuration)
            => _configuration = configuration;

        public async Task<decimal> GetDeliveryFeeAsync(string zipCode)
        {
            var client = new RestClient(_configuration.DeliveryFeeServiceUrl);
            
            ...
```

A melhor forma para resolver esse tipo de dependência do **Configuration** que possui somente uma instância é com a utilização do **AddSingleton** e estará disponível para toda a aplicação, pois as configurações são as mesmas para toda a aplicação. 

Recomendável utilizar **AddSingleton** para configurações do sistemas. 
Se houverem configurações customizadas por usuário esse modelo não funcionará.

```c#
...
builder.Services.AddSingleton<Configuration>();
...
```

</details>

<!--#endregion -->

<!--#region Extension Methods -->

<details id="depend-extension-methods"><summary>Extension Methods</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Resolvendo a bagunça

```c#
...

builder.Services.AddScoped(new SqlConnection());
builder.Services.AddTransient<IProductRepository,ProductRepository>();
builder.Services.AddTransient<ICustomerRepository,CustomerRepository>();
builder.Services.AddTransient<IDiscountRepository,DiscountRepository>();
builder.Services.AddTransient<IOrderRepository,OrderRepository>();
builder.Services.AddTransient<IRoleRepository,RoleRepository>();
builder.Services.AddTransient<ICartRepository,CartRepository>();

...
```

Extension Methods

- Permitem **adicionar comportamentos** as classes *built-in* do .NET
- Como por exemplo o **WebApplicationBuilder.cs**
  - Mesmo se a classe for selada

```c#
public sealed class WebApplicationBuilder
{
  ...
  public IServiceCollection Services { get; }
  ...
}
```

Criação de uma nova classe e seu método de extensão, desde que eles sejam estáticos, receba o nome da classe que deseja-se extender, neste caso **IServiceCollection** e contenha na sua frente a palavra reservada **this**. As dependências estão sendo resolvidas dentro dos métodos **AddRepositories** e **AddServices**.

```c#
public static class DependenciesExtension
{
  public static void AddRepositories(this IServiceCollection services)
  {
    services.AddTransient<ICustomerRepository,CustomerRepository>();
    services.AddTransient<IPromoCodeRepository,PromoCodeRepository>();
    services.AddTransient<IDeliveryFeeService,DeliveryFeeService>();
  }

  public static void AddServices(this IServiceCollection services)
  {
    services.AddTransient<IDeliveryFeeService,DeliveryFeeService>();
  }
}
```

O **Program.cs** ficaria da forma abaixo:

```c#
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

...
```

</details>

<!--#endregion -->

<!--#region Outras formas de DI -->

<details id="depend-outras-formas"><summary>Outras formas de DI</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

As interfaces não são necessárias para ter ou resolver dependências. 

As interfaces são necessárias para implementação do **DIP - Dependency Inversion Principle** 

```c#
public static void AddRepositories(this IServiceCollection services)
{
  services.AddTransient<CustomerRepository>();
  services.AddTransient<new CustomerRepository>();
}
```

</details>

<!--#endregion -->

<!--#region Implementando Extension Methods -->

<details id="depend-impl-extension-methods"><summary>Implementando Extension Methods</summary>

<br/>

[Projeto 1](./Projetos/Projeto%201/)

Criação do item **Extensions/DependenciesExtension.cs**:

```c#
using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services;
using DependencyStore.Services.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Extensions;

public static class DependenciesExtension
{
    public static void AddConfiguration (this IServiceCollection services)
    {
        services.AddSingleton<Configuration>();        
    }

    public static void AddSqlConnection
        (this IServiceCollection services,
        string connectionString)
    {
        services.AddScoped<SqlConnection>(x 
            => new SqlConnection(connectionString));
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
    }

    public static void AddServices(this IServiceCollection services) 
    {
        services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
    }
}
```

Ajuste no **AppSettings.json** para informar a **Connection String**:

```json
{
  ...

  "ConnectionStrings": {
    "DefaultConnection": "CONN_STRING"
  }
}
```

Refatoração do **Program.cs**:

```c#
...

builder.Services.AddConfiguration();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSqlConnection(connStr);
builder.Services.AddRepositories();
builder.Services.AddServices();

...
```

</details>

<!--#endregion -->

<!--#region AddTransient, AddScoped e AddSingleton na prática - Parte 1 -->

<details id="depend-impl-extension-methods"><summary>AddTransient, AddScoped e AddSingleton na prática - Parte 1</summary>

<br/>

[Projeto 2](./Projetos/Projeto%202/)

Ciclos de Vida

</details>

<!--#endregion -->

<!--#region AddTransient, AddScoped e AddSingleton na prática - Parte 2 -->

<details id="depend-impl-extension-methods"><summary>AddTransient, AddScoped e AddSingleton na prática - Parte 2</summary>

<br/>

[Projeto 2](./Projetos/Projeto%202/)

Ciclos de Vida

```json
{
    "id": "70471d16-533b-4646-b455-8a5d83a536ae",
    "primaryServiceId": "d6992245-4004-4f00-a86b-58d03e6b3eb3",
    "secondaryService": {
        "id": "bfd7778f-97db-4414-9af4-ef80387f691e",
        "primaryServiceId": "d6992245-4004-4f00-a86b-58d03e6b3eb3"
    },
    "tertiaryService": {
        "id": "9170ab1f-1916-4424-9666-41330779c547",
        "primaryServiceId": "d6992245-4004-4f00-a86b-58d03e6b3eb3",
        "secondaryServiceId": "bfd7778f-97db-4414-9af4-ef80387f691e",
        "secondaryServiceNewInstanceId": "bfd7778f-97db-4414-9af4-ef80387f691e"
    }
}
```

</details>

<!--#endregion -->

<!--#region Registrando mais de uma implementação -->

<details id="depend-mais-impl"><summary>Registrando mais de uma implementação</summary>

<br/>

Um pouco mais...

- Se podemos ter mais de uma implementação por interface...
- O que acontece quando registramos mais de um serviço?

```c#
public interface IService
{
}

public class ServiceOne : IService
{
}

public class ServiceTwo : IService
{
}
```

```c#
builder.Services.AddTransient<IService, ServiceOne>();
builder.Services.AddTransient<IService, ServiceTwo>();
```

Sempre o último...

- Neste caso, como **não especificamos** a implementação, sempre será retornado a **última registrada**
- No exemplo seria o **ServiceTwo**

```c#
private readonly IService _service;]

public OrderController(IService service)
  => _service = service;

[Route("/")]
[HttpGet]
public IActionResult Get()
{
  return Ok(new
  {
    _service.GetType().Name
  });
}
```

Inclusive pode isto aqui

```c#
builder.Services.AddTransient<IService, ServiceOne>();
builder.Services.AddTransient<IService, ServiceOne>();
builder.Services.AddTransient<IService, ServiceOne>();
builder.Services.AddTransient<IService, ServiceTwo>();
```

Não significa que ele somente registrou o último

```c#
private readonly IEnumerable<IService> _service;

public OrderController(IEnumerable<IService> service)
  => _service = service;

[Route("/")]
[HttpGet]
public IActionResult Get()
{
  return Ok(_service.Select(x => x.GetType().Name));
}
```

```json
[
  "ServiceOne",
  "ServiceOne",
  "ServiceOne",
  "ServiceTwo"
]
```

Em resumo...

- Os serviços **estão sendo registrados**
- Porém o comportamento quando resolvemos um serviço é de **obter apenas o último**

```c#
private readonly IService _service;

public OrderController(IService service)
  => _service = service;

[Route("/")]
[HttpGet]
public IActionResult Get()
{
  return Ok(new
  {
    _service.GetType().Name
  });
}
```


</details>

<!--#endregion -->

<!--#region Service Descriptor -->

<details id="depend-service-descriptor"><summary>Service Descriptor</summary>

<br/>

- Descreve **como resolver** uma dependência
- Determina o **tipo** e **tempo de vida** dela
- **AddTransient**, **AddScoped** e **AddSingleton** são **wrapers** deste item

```c#
var descriptor = new ServiceDescriptor(
  typeof(IService), // Abstração
  typeof(ServiceOne), // Implementação
  ServiceLifetime.Singleton // Tempo de vida
);

builder.Services.Add(descriptor);
```

</details>

<!--#endregion -->

<!--#region TryAdd e TryAddEnumerable -->

<details id="depend-tryadd-tryaddenumerable"><summary>TryAdd e TryAddEnumerable</summary>

<br/>

TryAdd*

- Inverte o comportamento
- Não dá erro, mas não duplica
- Compara apenas a abstração
  - Não registra duas implementações para uma mesma abstração (interface)

```c#
builder.Services.TryAddTransient<IService, ServiceOne>();
builder.Services.TryAddTransient<IService, ServiceOne>();
builder.Services.TryAddTransient<IService, ServiceOne>();
builder.Services.TryAddTransient<IService, ServiceTwo>();
```

- Só vai registrar o **primeiro** item
- Como já existe uma implementação registrada para a interface **IService** vai **ignorar as próximas tentativas** de registro

```json
["ServiceOne"]
```

TryAddEnumerable

- TryAddEnumerable
- Permite registrar ambos (1 e 2)
- Porém não permite duplicar (2 e 2, por exemplo)
- Único (Interface e implementação)

</details>

<!--#endregion -->

<!--#region Resolvendo múltiplas dependências -->

<details id="depend-multiplas"><summary>Resolvendo múltiplas dependências</summary>

<br/>

[Projeto 2](./Projetos/Projeto%202/)

Program.cs

```c#
...

builder.Services.AddTransient<IService, PrimaryService>();

...

app.MapGet("/", (IService service) 
    => Results.Ok(service.GetType().Name));

...

public interface IService
{

}
```

```c#
public class PrimaryService : IService { }
public class SecondaryService : IService { }
public class TertiaryService : IService { }
```

```ps
dotnet run
```

```json
"PrimaryService"
```

</details>

<!--#endregion -->

<!--#region TryAddTransient -->

<details id="depend-tryaddtransient"><summary>TryAddTransient</summary>

<br/>

[Projeto 2](./Projetos/Projeto%202/)


Program.cs:

```c#
...

builder.Services.AddTransient<IService, PrimaryService>();
builder.Services.AddTransient<IService, PrimaryService>();
builder.Services.AddTransient<IService, SecondaryService>();

...

app.MapGet("/", (IService service) 
    => Results.Ok(service.GetType().Name));

...

public interface IService
{

}
```

```c#
public class PrimaryService : IService { }
public class SecondaryService : IService { }
public class TertiaryService : IService { }
```

Erro ocorrido pois existe mais de uma dependência para o serviço:

```ps
dotnet run 

System.AggregateException: 'Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: IService Lifetime: Transient ImplementationType: DependencyInjectionLifetimeSample.Services.SecondaryService': Unable to resolve service for type 'DependencyInjectionLifetimeSample.Services.PrimaryService' while attempting to activate 'DependencyInjectionLifetimeSample.Services.SecondaryService'.)'
```

---

Dada uma interface temos uma implementação.

Program.cs:

```c#
...

builder.Services.TryAddTransient<IService, PrimaryService>();
builder.Services.TryAddTransient<IService, PrimaryService>();
builder.Services.TryAddTransient<IService, SecondaryService>();

...

app.MapGet("/", (IEnumerable<IService> services) 
    => Results.Ok(services.Select(x => x.GetType().Name)));

...
```

```json
[
    "PrimaryService"
]
```

---

Program.cs:

```c#
...

builder.Services.AddTransient<IService, PrimaryService>();
builder.Services.AddTransient<IService, PrimaryService>();
//builder.Services.AddTransient<IService, SecondaryService>();

...

app.MapGet("/", (IEnumerable<IService> services) 
    => Results.Ok(services.Select(x => x.GetType().Name)));

...
```

```json
[
    "PrimaryService",
    "PrimaryService"
]
```


</details>

<!--#endregion -->

<!--#region TryAddEnumerable -->

<details id="depend-tryaddenumerable"><summary>TryAddEnumerable</summary>

<br/>

[Projeto 2](./Projetos/Projeto%202/)

Ajuda a prevenir várias implementações para a mesma interface.

Program.cs:

```c#
...

var descriptor = new ServiceDescriptor(
    typeof(IService),
    typeof(PrimaryService),
    ServiceLifetime.Transient);
builder.Services.TryAddEnumerable(descriptor);

...
```

```json
[
    "PrimaryService"
]
```

---

Não permite outra implementação para a mesma interface.

Program.cs

```c#
...

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>()); // permite
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, SecondaryService>()); // não permite

...
```

```ps
System.AggregateException: 'Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: IService Lifetime: Singleton ImplementationType: DependencyInjectionLifetimeSample.Services.SecondaryService': Unable to resolve service for type 'DependencyInjectionLifetimeSample.Services.PrimaryService' while attempting to activate 'DependencyInjectionLifetimeSample.Services.SecondaryService'.)'
```

---

Program.cs

```c#
...

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>()); // permite
//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, SecondaryService>()); // não permite

...
```

```json
[
    "PrimaryService"
]
```

</details>

<!--#endregion -->

<!--#region Formas de resolver dependências -->

<details id="depend-formas"><summary>Formas de resolver dependências</summary>

<br/>

Resolvendo Dependências

- Construtor
- Na assinatura do método
- No program
- No HttpContext

> No construtor

- **Private Readonly?** Variável somente leitura que pode ser atribuída somente no construtor.
  - **Qual a diferença de const?** Obrigatória a atribuição de valor somente na sua declaração.

```c#
private readonly IWeatherService _service;

public WeatherController(IWeatherService service)
  => _service = service;

[HttpGet("/")]
public IEnumerable<WeatherForecast> Get()
  => _service.Get();
```  

> FromServices

Recomendável quando utilizado em **um único método**.
Para **vários métodos** recomenda-se resolver no construtor.

- Obtém direto dos serviços
- No **.NET 7** não precisa mais especificar **[FromServices]**, assim como não precisa do **FromBody** e **FromRoute** por exemplo.

```c#
[HttpGet("/")]
public IEnumerable<WeatherForecast> Get(
  [FromServices] IWeatherService service)
  => service.Get();
```

</details>

<!--#endregion -->

<!--#region Resolvendo dependências no Program.cs -->

<details id="depend-program"><summary>Resolvendo dependências no Program.cs</summary>

<br/>

No Program.cs

Código explicitado:

```c#
var app = builder.Build(); 
using(var scope = app.Services.CreateScope()) 
{
  var services = scope.ServiceProvider; 

  var repository = services.GetRequiredService<ICustomerRepository>(); 
  repository.CreateAsync(new Customer{"André Baltieri"});
}
```

- Deve ser resolvido apos **var app = builder.Build();** no build da aplicação, no início da sua execução. Cuidado para não sobrecarregar o início da aplicação;
- **using(var scope = app.Services.CreateScope())** garante que a aplicação e seus serviços já estejam registrados;
- **var services = scope.ServiceProvider** fornece todos os serviços registrados. Dada uma implementação ou uma abstração é provida a sua instância;
- **var repository = services.GetRequiredService<ICustomerRepository>()** recupera a instância de um serviço registrado.

</details>

<!--#endregion -->

<!--#region Resolvendo dependências no HttpContext -->

<details id="depend-httpcontext"><summary>Resolvendo dependências no HttpContext</summary>

<br/>

> Via **HttpContext**

Utilizado quando fora do controlador e escopo da aplicação. Sendo obrigado a utilizar o **HttpContext**. Pode ser utilizado em **middlewares** ou qualquer outro lugar no ASP.NET que tenha acesso ao **HttpContext**.

Basta saber como acessar o **HttpContext**.

- Podemos *recuperar* os serviços registrados utilizando o **HttpContext**

```c#
public async Task OnActionExecutionAsync(
  ActionExecutionContext context, // contexto de execução da ação, dentro dele tem-se o HttpContext
  ActionExecutionDelegate next
)
{
  var service = context
    .HttpContext
    .RequestServices
    .GetService<IWeatherService>(); // obtem a instância de uma classe baseada na interface

  // o service pode ser nulo, precisa ser tratado

  var forecasts = service.Get();
}
```

</details>

<!--#endregion -->

<!--#region Quando utilizar FromServices -->

<details id="depend-fromservices"><summary>Quando utilizar FromServices</summary>

<br/>

[Projeto 3](./Projetos/Projeto%203/)

Utilizar **[FromServices]** quando for utilizar somente em um método do controlador, e utilizar a injeção da dependência no construtor do controlador se for utilizado em vários métodos.

[WeatherService.cs](./Projetos/Projeto%203/Services/WeatherService.cs)

[WeatherForecastController.cs](./Projetos/Projeto%203/Controllers/WeatherForecastController.cs)

</details>

<!--#endregion -->

<!--#region GetRequiredServices -->

<details id="depend-getrequiredservices"><summary>GetRequiredServices</summary>

<br/>

[Projeto 3](./Projetos/Projeto%203/)

[Program.cs](./Projetos/Projeto%203/Program.cs)

```c#
...

// resolução da dependência
builder.Services.AddTransient<IWeatherService, WeatherService>();

...

// instância do serviço
using (var scope = app.Services.CreateScope())
{
    var service = scope
        .ServiceProvider
        .GetRequiredService<IWeatherService>();
    service.Get();
}

...
```


</details>

<!--#endregion -->

<!--#region GetService -->

<details id="depend-getservice"><summary>GetService</summary>

<br/>

[Projeto 3](./Projetos/Projeto%203/)

[ApiKeyAttribute.cs](./Projetos/Projeto%203/Attributes/ApiKeyAttribute.cs)

```c#
...

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    ...
    
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        var service = context
            .HttpContext
            .RequestServices
            .GetService<IWeatherService>();
        var forecasts = service?.Get();

        ...
```

[WeatherForecastController.cs](./Projetos/Projeto%203/Controllers/WeatherForecastController.cs):

```c#
...

    [ApiKey]
    [HttpGet("/")]
    public IEnumerable<WeatherForecast> Get()
        => _service.Get();

...
```

</details>

<!--#endregion -->

<!--#endregion -->

<!--#region Perguntas e Exercícios -->

<h2 id="perguntas">Perguntas e Exercícios</h2>

<!--#region Hora de entrevista -->

<details id="perguntas-entrevista"><summary>Hora de entrevista</summary>

<br/>

**Qual a diferença entre AddTransient, AddScoped e AddSingleton?**

**Qual a finalidade do atributo FromServices?**

**Podemos resolver dependências fora dos controladores?**

**De forma resumida, você consegue me dizer o que é injeção de dependência?**

**O que é Inversão de Controle?**

**O que é Inversão de Dependência?** DIP

**Qual a relação entre injeção de dependência, inversão de controle e inversão de dependência?**

</details>

<!--#endregion -->

<!--#region Sugestão de projeto -->

<details id="exercicios"><summary>Sugestão de projeto</summary>

<br/>

- Reserva de quarto
  - Utilizar AddTransient, Scoped e Singleton
  - Separar em serviços e repositórios
  - Postar o resultado no repositório do GitHub do curso

[Ponto de Partida](./Assets/Demos/06%20-%20exercicios/)

</details>

<!--#endregion -->

<!--#region Conclusão -->

<details id="conclusao"><summary>Conclusão</summary>

<br/>

</details>

<!--#endregion -->

<!--#endregion -->
