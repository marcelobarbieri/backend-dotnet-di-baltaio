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

<!--#endregion -->
