namespace BasketCase.Core.Models
{
    public enum KeyEnum
    {
        BasketBooking,
        BasketService
    }

    public class KeyGenerator
    {
        public static string ReturnRedisKey(int userId, string correlationId) => $"{KeyEnum.BasketBooking}_{userId}_{correlationId}";
    }
}
