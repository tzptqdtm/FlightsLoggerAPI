using System.Threading.Tasks;

namespace KafkaLib.Interfaces
{
    public interface IKafkaHandler<in Tk, in Tv>
    {
        Task HandleAsync(Tk key, Tv value);
    }
}