using MediatR;

namespace Restaurants.Applications.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand:IRequest<string>
{
    public int RestaurantId { get; set; }
    public Stream File { get; set; }= default!;
    public string FileName { get; set; } = default!;
}
