using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CarRental.Domain.Shared;
using CarRental.Domain.VehicleModule;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CarRental.Domain.VehicleImageModule
{
    public class VehicleImage : BaseEntity
    {
        public int VehicleId { get; set; }
        public byte[]? ImageData { get; set; }
        
        // Transient property not stored in database
        private Image<Rgba32>? _cachedImage;

        public VehicleImage() { }
        
        public VehicleImage(int id, int vehicleId, byte[] imageData)
        {
            this.id = id;
            this.VehicleId = vehicleId;
            this.ImageData = imageData;
        }
        
        // Create from an image
        public VehicleImage(int id, int vehicleId, Image<Rgba32> image)
        {
            this.id = id;
            this.VehicleId = vehicleId;
            
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
            this.ImageData = memoryStream.ToArray();
            this._cachedImage = image;
        }

        // Get as Image object
        public Image<Rgba32>? GetImage()
        {
            if (_cachedImage != null)
                return _cachedImage;
                
            if (ImageData == null)
                return null;
                
            using var memoryStream = new MemoryStream(ImageData);
            _cachedImage = Image.Load<Rgba32>(memoryStream);
            return _cachedImage;
        }

        public override string ToString()
        {
            return $"Vehicle Image ID: {Id}, VehicleID: {VehicleId}";
        }

        public override string Validate()
        {
            string validationResult = "VALID";
            return validationResult;
        }

        public override bool Equals(object? obj)
        {
            return obj is VehicleImage vehicleImage &&
                   EqualityComparer<int>.Default.Equals(this.VehicleId, vehicleImage.VehicleId) &&
                   EqualityComparer<int>.Default.Equals(Id, vehicleImage.id);
        }

        public override int GetHashCode()
        {
            int hashCode = 155997214;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + VehicleId.GetHashCode();
            hashCode = hashCode * -1521134295 + (ImageData != null ? ImageData.GetHashCode() : 0);
            return hashCode;
        }
    }
}
