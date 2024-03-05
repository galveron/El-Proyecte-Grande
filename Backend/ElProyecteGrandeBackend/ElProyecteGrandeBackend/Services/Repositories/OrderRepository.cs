using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IUserRepository _userRepository;
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<MarketPlaceContext> _optionsBuilder;

    public OrderRepository(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _config =
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        _optionsBuilder = new DbContextOptionsBuilder<MarketPlaceContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
    }
    public Order GetOrder(int orderId)
    {
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        return dbContext.Orders.Where(o => o.Id == orderId)
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .First();
    }

    public List<Order> GetUserOrders(string userId)
    {
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        var user = _userRepository.GetUser(userId);
        return user.Orders.ToList();
    }

    public void AddOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        
        var userFromDb = dbContext.Users.FirstOrDefault(user1 => user1.Id == order.User.Id);
        
        if (userFromDb == null)
        {
            throw new Exception("User not found for adding product.");
        }
        
        var orderForDb = new Order {User = userFromDb, Date = DateTime.Now, PriceToPay = 0};;
        userFromDb.Orders.Add(orderForDb);
        dbContext.Update(userFromDb);
        dbContext.Orders.Add(orderForDb);
        //dbContext.Update(order);
        //dbContext.Orders.Add(order);
        //dbContext.Update(order.User);
        dbContext.SaveChanges();
    }

    public void DeleteOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        dbContext.Orders.Remove(order);
        dbContext.SaveChanges();
    }

    public void UpdateOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        dbContext.Orders.Update(order);
        dbContext.SaveChanges();
    }
    
    public void AddOrRemoveOrderItems(int orderId, int productId, int quantity)
    {
        if (quantity == 0)
        {
            throw new Exception("You cannot add zero product.");
        }
        
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        var orderForAddToOrderItems = dbContext.Orders
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .SingleOrDefault(order => order.Id == orderId);
        var productToAddToOrderItems = dbContext.Products.FirstOrDefault(product1 => product1.Id == productId);
        
        if (orderForAddToOrderItems == null)
        {
            throw new Exception("Order was not found for adding to OrderItems.");
        }

        if (productToAddToOrderItems == null)
        {
            throw new Exception("Product was not found for adding to OrderItems.");
        }

        var orderItem = orderForAddToOrderItems.OrderItems.SingleOrDefault(orderItem => orderItem.Product.Id == productToAddToOrderItems.Id);
        
        if (orderItem == null)
        {
            if (quantity < 0)
            {
                throw new Exception("The quantity of products cannot be negative.");
            }
            
            orderForAddToOrderItems.OrderItems.Add(new OrderItem
            {
                OrderId = orderForAddToOrderItems.Id,
                Order = orderForAddToOrderItems,
                ProductId = productToAddToOrderItems.Id,
                Product = productToAddToOrderItems,
                Quantity = quantity
            });
            orderForAddToOrderItems.PriceToPay += productToAddToOrderItems.Price * quantity;
        }
        else
        {
            if (orderItem.Quantity + quantity < 0)
            {
                orderForAddToOrderItems.OrderItems.Remove(orderItem);
                orderForAddToOrderItems.PriceToPay -= productToAddToOrderItems.Price * orderItem.Quantity;
            }
            else
            {
                orderItem.Quantity += quantity;
                orderForAddToOrderItems.PriceToPay += productToAddToOrderItems.Price * quantity;
            }
        }
        
        dbContext.Update(orderForAddToOrderItems);
        dbContext.SaveChanges();
    }
    
    public void EmptyOrderItems(int orderId)
    {
        using var dbContext = new MarketPlaceContext(_optionsBuilder.Options);
        var orderForCartEmptying = dbContext.Orders
            .Include(order => order.OrderItems)
            .Single(order => order.Id == orderId);
        
        orderForCartEmptying.OrderItems.Clear();
        orderForCartEmptying.PriceToPay = 0;
        
        dbContext.SaveChanges();
    }
}