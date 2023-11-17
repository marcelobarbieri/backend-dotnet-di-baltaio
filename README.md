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

<!--#endregion -->
