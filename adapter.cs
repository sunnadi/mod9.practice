using System;
using static Program;

internal class Program
{
    public interface IInternalDeliveryService
{
    void DeliverOrder(string orderId);
    string GetDeliveryStatus(string orderId);
    decimal CalculateDeliveryCost(string orderId);

    }

public class InternalDeliveryService : IInternalDeliveryService
{
    public void DeliverOrder(string orderId)
    {
        Console.WriteLine("Order DeliverOrder: " + orderId);
    }

    public string GetDeliveryStatus(string orderId)
    {
        return "Status for Order " + orderId;
    }
    public decimal CalculateDeliveryCost(string orderId)
    {
        return 50.00m;

    }
        
    }
    public class GlovoLogisticsServiceA
    {
        public void ShipItem(int itemId)
        {
            Console.WriteLine("GlovoLogisticsServiceA: Shipping item " + itemId);
        }

        public string TrackShipment(int shipmentId)
        {
            return "Tracking status from GlovoLogisticsServiceA for shipment " + shipmentId;
        }

        public decimal CalculateShippingCost(int itemId)
        {
            return 75.00m; 
        }
    }

    public class GlovoLogisticsServiceB
    {
        public void SendPackage(string packageInfo)
        {
            Console.WriteLine("GlovoLogisticsServiceB: Sending package " + packageInfo);
        }

        public string CheckPackageStatus(string trackingCode)
        {
            return "Tracking status from GlovoLogisticsServiceB for package " + trackingCode;
        }

        public decimal GetPackageCost(string packageInfo)
        {
            return 60.00m; 
        }
    }
    public class LogisticsAdapterA : IInternalDeliveryService
    {
        private readonly GlovoLogisticsServiceA _serviceA;

        public LogisticsAdapterA(GlovoLogisticsServiceA serviceA)
        {
            _serviceA = serviceA;
        }

        public void DeliverOrder(string orderId)
        {
            try
            {
                int itemId = int.Parse(orderId);
                _serviceA.ShipItem(itemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in LogisticsAdapterA.DeliverOrder: " + ex.Message);
            }
        }

        public string GetDeliveryStatus(string orderId)
        {
            int itemId = int.Parse(orderId);
            return _serviceA.TrackShipment(itemId);
        }

        public decimal CalculateDeliveryCost(string orderId)
        {
            int itemId = int.Parse(orderId);
            return _serviceA.CalculateShippingCost(itemId);
        }
    }
    public class LogisticsAdapterB : IInternalDeliveryService
    {
        private readonly GlovoLogisticsServiceB _serviceB;

        public LogisticsAdapterB(GlovoLogisticsServiceB serviceB)
        {
            _serviceB = serviceB;
        }

        public void DeliverOrder(string orderId)
        {
            try
            {
                _serviceB.SendPackage(orderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in LogisticsAdapterB.DeliverOrder: " + ex.Message);
            }
        }

        public string GetDeliveryStatus(string orderId)
        {
            return _serviceB.CheckPackageStatus(orderId);
        }

        public decimal CalculateDeliveryCost(string orderId)
        {
            return _serviceB.GetPackageCost(orderId);
        }
    }

    public static class DeliveryServiceFactory
    {
        public static IInternalDeliveryService GetDeliveryService(string type)
        {
            return type switch
            {
                "Internal" => new InternalDeliveryService(),
                "ServiceA" => new LogisticsAdapterA(new GlovoLogisticsServiceA()),
                "ServiceB" => new LogisticsAdapterB(new GlovoLogisticsServiceB()),
                _ => throw new ArgumentException("Unknown delivery service type")
            };
        }
    }

    static void Main(string[] args)
    {
        string typeDelivery = "ServiceA"; 
        IInternalDeliveryService service = DeliveryServiceFactory.GetDeliveryService(typeDelivery);

        string orderId = "46656";
        service.DeliverOrder(orderId);

        string status = service.GetDeliveryStatus(orderId);
        Console.WriteLine(status);

        decimal cost = service.CalculateDeliveryCost(orderId);
        Console.WriteLine("Delivery cost: " + cost);
    }
}











